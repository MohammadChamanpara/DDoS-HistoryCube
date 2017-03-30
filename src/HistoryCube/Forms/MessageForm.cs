using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Library.Windows.Forms
{
    public partial class MessageForm : Form
    {
        public enum FormMode { Question, Message };
        public MessageForm(string caption, string messageFormat, object[] messageArguments, Bitmap icon, FormMode formMode)
        {
            InitializeComponent();
            this.Text = caption;
            messageLabel.Text = string.Format(messageFormat.Trim(), messageArguments);
            pictureBoxIcon.Image = icon;
            okButton.Visible = (formMode == FormMode.Message);
            yesButton.Visible = (formMode == FormMode.Question);
            noButton.Visible = (formMode == FormMode.Question);
        }
        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void yesButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Yes;
            Close();
        }
        private void noButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
        }
        private void MessageForm_Load(object sender, EventArgs e)
        {
            this.Width = messageLabel.Width + pictureBoxIcon.Width + pictureBoxCopy.Width + pictureBoxCopy.Left + 30;
            this.Height = messageLabel.Height + 130;
            pictureBoxIcon.Left = this.Width - pictureBoxIcon.Width - 20;
            messageLabel.Left = pictureBoxIcon.Left - messageLabel.Width;
        }

        private void pictureBoxCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(messageLabel.Text);
        }

    }
}
