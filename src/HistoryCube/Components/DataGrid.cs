using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Library.Windows.WindowsForms.Helpers;

namespace Library.Windows.Components
{
    public class LibraryDataGrid : DataGridView
    {
        #region Constructor
        public LibraryDataGrid()
        {
            this.CellContentDoubleClick += new DataGridViewCellEventHandler(LibraryDataGrid_CellContentDoubleClick);
            this.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            this.BackgroundColor = Color.WhiteSmoke;
            this.ReadOnly = true;
            this.AutoGenerateColumns = false;
            this.AllowUserToAddRows = false;
            this.AllowUserToOrderColumns = true;
            this.MultiSelect = false;
        }
        #endregion

        #region Events
        void LibraryDataGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            var text = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            int maxSize=5000;
            if (text.Length > maxSize)
            {
                text = text.Remove(maxSize);
                text += "...";
            }

            WindowsHelperMethods.ShowInformationMessage(text);
        }

        #endregion
    }
}
