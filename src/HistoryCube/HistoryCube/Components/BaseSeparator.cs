using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Library.Windows.Components
{
    public abstract class BaseSeparator:Label
    {
        public BaseSeparator()
        {
            this.BorderStyle = BorderStyle.Fixed3D;
            this.AutoSize = false;
            this.Text = "";
        }
    }
}
