using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HistoryCube.Model;

namespace HistoryCube
{
    public static class DB
    {
        public static Stone_DBEntities NewContext
        {
            get
            {
                return new Model.Stone_DBEntities();
            }
        }
    }
}
