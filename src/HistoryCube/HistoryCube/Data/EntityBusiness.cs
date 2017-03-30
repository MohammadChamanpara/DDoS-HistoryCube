using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Globalization;
using System.Data;
using System.Reflection;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Data.EntityClient;
using Library.Log;
using Library.Settings;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Library.Data
{
    
    public class EntityBusiness<TContext, TEntity> : IDisposable, ISettingEnabled
        where TEntity : EntityObject
        where TContext : ObjectContext, new()
    {
        #region Constructors
        public SettingService SettingService
        {
            get
            {
                return new SettingService(SettingService.ConfigurationFileNames.CurrentAssemblyConfigFile, "EntityBusiness", this);
            }
        }
        public EntityBusiness() : this(new TContext()) {
            SettingService.LoadSettings();
        }
        public EntityBusiness(TContext context)
        {
            this.Context = context;
            this.Context.MetadataWorkspace.LoadFromAssembly(typeof(TContext).Assembly);
            SettingService.LoadSettings();
        }
        #endregion

        #region Constant
        private const string SqlDependencyCookie = "MS.SqlDependencyCookie";
        #endregion

        #region Variables
        SqlDependency sqlDependency;
        #endregion

        #region Properties
        public TContext Context { get; private set; }

        private ObjectSet<TEntity> entities;
        public ObjectSet<TEntity> Entities
        {
            get
            {
                if ((entities == null))
                    entities = Context.CreateObjectSet<TEntity>();
                return entities;
            }
        }

        public String FullyQualifiedEntitySetName
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[] { this.Entities.EntitySet.EntityContainer.Name, this.Entities.EntitySet.Name });
            }
        }

        public Boolean Connected
        {
            get
            {
                try
                {
                    if (this.Context.Connection.State != ConnectionState.Open)
                        HelperMethods.CallWithTimeout
                        (
                            this.Context.Connection.Open,
                            this.Context.Connection.ConnectionTimeout
                        );
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private RefreshMode refreshMode = RefreshMode.StoreWins;
        public RefreshMode RefreshMode { get { return refreshMode; } set { refreshMode = value; } }

        private Boolean autoRefresh = false;
        public Boolean AutoRefresh
        {
            get { return autoRefresh; }
            set
            {
                try
                {
                    if (value == autoRefresh)
                        return;

                    if (value == true)
                        StartDataBaseNotification();
                    else
                        StopDataBaseNotification();

                    autoRefresh = value;
                }
                catch (Exception exception)
                {
                    LogService.LogException(exception, "بروز خطا هنگام فعال کردن آگاهی از تغییر پایگاه داده در برنامه.");
                }
            }
        }


        public string ProviderConnectionstring
        {
            get
            {
                return (this.Context.Connection as EntityConnection).StoreConnection.ConnectionString;
            }
        }
        private string EnableBrokerCommand
        {
            get { return "ALTER DATABASE {0} SET NEW_BROKER WITH ROLLBACK IMMEDIATE".FormatWith(this.DatabaseName); }
        }
        public string DatabaseName
        {
            get
            {
                return (this.Context.Connection as EntityConnection).StoreConnection.Database;
            }
        }
        [DisplayName("وضعیت فعال بودن ثبت رویداد ها ")]
        [SettingProperty(false)]
        public bool EnableLog { get; set; }

        #endregion

        #region Methods

        private static List<PropertyInfo> GetKeyColumns(System.Type type)
        {
            List<PropertyInfo> retval = new List<PropertyInfo>();
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                IEnumerable<Attribute> attributes = prop.GetCustomAttributes(true).OfType<System.Attribute>().AsEnumerable();
                EdmScalarPropertyAttribute edmScalarPropertyAttribute = attributes.OfType<EdmScalarPropertyAttribute>().FirstOrDefault();
                if (edmScalarPropertyAttribute != null)
                {
                    if (edmScalarPropertyAttribute.EntityKeyProperty == true)
                        retval.Add(prop);
                }
            }
            return retval;
        }
        private EntityKey GetEntityKey(params object[] keyValues)
        {
            List<PropertyInfo> KeyColumns = GetKeyColumns(typeof(TEntity));
            if (KeyColumns.Count == 0)
                throw new ArgumentException("Entity has no key columns");
            if (keyValues.Length != KeyColumns.Count)
                throw new ArgumentException("KeyValues count must be equal with key count");

            KeyValuePair<string, object>[] keyValuesPairs = new KeyValuePair<string, object>[keyValues.Length];
            for (int i = 0; i < keyValues.Length; i++)
            {
                keyValuesPairs[i] = new KeyValuePair<string, object>(KeyColumns[i].Name, Convert.ChangeType(keyValues[i], KeyColumns[i].PropertyType));
            }

            return new EntityKey(FullyQualifiedEntitySetName, keyValuesPairs);

        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return this.Entities;
        }
        public virtual void Create(TEntity entity)
        {
            Entities.AddObject(entity);
            Context.SaveChanges();

            if (this.EnableLog)
                Library.Log.LogService.LogInformation("Create {0} with key={1}".FormatWith(typeof(TEntity).Name, entity.EntityKey.EntityKeyValues[0].Value));
        }
        public virtual void Create(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                Entities.AddObject(entity);
            Context.SaveChanges();
            if (this.EnableLog)
            {
                var entityKeys=string.Join(",", entities.Select(x => x.EntityKey.EntityKeyValues[0].Value));
                Library.Log.LogService.LogInformation("Create {0} with key={1}".FormatWith(typeof(TEntity).Name, entityKeys));
            }
        }
        public virtual void Edit(TEntity entity)
        {
            var original = this.GetEntity(entity);
            Entities.ApplyCurrentValues(entity);
            Context.SaveChanges();
            if (this.EnableLog)
                Library.Log.LogService.LogInformation("Edit {0} with key={1}".FormatWith(typeof(TEntity).Name, original.EntityKey.EntityKeyValues[0].Value));
        }
        public virtual void Delete(object key)
        {
            var original = this.GetEntity(key);
            Entities.DeleteObject(original);
            Context.SaveChanges();
            if (this.EnableLog)
                Library.Log.LogService.LogInformation("Delete {0} with key={1}".FormatWith(typeof(TEntity).Name, key.ToString()));
        }

        public virtual TEntity GetEntity(object key)
        {
            EntityKey entityKey = GetEntityKey(key);
            return GetEntity(entityKey);
        }
        public virtual TEntity GetEntity(TEntity entity)
        {
            EntityKey entityKey = entity.EntityKey ?? Context.CreateEntityKey(this.Entities.EntitySet.Name, entity);
            return GetEntity(entityKey);
        }
        public virtual TEntity GetEntity(EntityKey entityKey)
        {
            return (TEntity)Context.GetObjectByKey(entityKey);
        }

        public virtual void AttachEntity(TEntity entity)
        {
            try
            {
                this.Entities.Attach(entity);
            }
            catch (Exception)
            {

                return;
            }
            
        }

        public void Dispose()
        {
            StopDataBaseNotification();
            this.Context.Dispose();
        }

        public void OnCollectionChanged(SqlNotificationEventArgs e)
        {
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, e);
        }

        private void BindSqlDependency()
        {
            var oldCookie = CallContext.GetData(SqlDependencyCookie);
            try
            {
                UnBindSqlDependency();
                sqlDependency = new SqlDependency();
                CallContext.SetData(SqlDependencyCookie, sqlDependency.Id);
                sqlDependency.OnChange += sqlDependency_OnChange;
                this.Context.Refresh(this.RefreshMode, this.Entities);
            }
            finally
            {
                CallContext.SetData(SqlDependencyCookie, oldCookie);
            }
        }
        private void UnBindSqlDependency()
        {
            if (sqlDependency != null)
                sqlDependency.OnChange -= sqlDependency_OnChange;
        }

        private void StartDataBaseNotification()
        {
            try
            {
                SqlDependency.Start(this.ProviderConnectionstring);
            }
            catch (InvalidOperationException)
            {
                this.Context.ExecuteStoreCommand(this.EnableBrokerCommand);
                SqlDependency.Start(this.ProviderConnectionstring);
            }
            BindSqlDependency();
        }
        private void StopDataBaseNotification()
        {
            SqlDependency.Stop(this.ProviderConnectionstring);
            UnBindSqlDependency();
        }

        #endregion

        #region Event Handlers
        public event OnChangeEventHandler CollectionChanged;
        #endregion

        #region Events
        private void sqlDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info == SqlNotificationInfo.Invalid)
                throw new HelperClasses.LibraryException("A statement was provided that cannot be notified.");

            BindSqlDependency();

            this.OnCollectionChanged(e);
        }
        #endregion

        //public bool EnableLog { get; set; }
    }
}