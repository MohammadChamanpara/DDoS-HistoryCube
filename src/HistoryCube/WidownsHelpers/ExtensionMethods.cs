using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Configuration;
using System.Xml;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.Windows.Forms;

namespace Library.Windows.WindowsForms.Helpers
{
    public static class ExtensionMethods
    {
        #region DataGridView
        public static int GetSelectedIndex(this DataGridView datagrid)
        {
            return (datagrid.CurrentRow == null) ? 0 : datagrid.CurrentRow.Index;
        }
        public static void SetSelectedIndex(this DataGridView datagrid, int index)
        {
            if (index >= datagrid.Rows.Count)
                return;
            datagrid.CurrentCell = datagrid.Rows[index].Cells[0];
        }

        #endregion
    }
}
