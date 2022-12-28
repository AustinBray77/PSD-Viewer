using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSDViewer
{
    class Prompt
    {
        public static bool YesNoDialog(string caption)
        {
            Form prompt = new Form()
            {
                Width = 200,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Button yes = new Button() { Text = "Yes", Top=50, Width = 50, Height = 50, DialogResult = DialogResult.OK };
            Button no = new Button() { Text = "No", Top = 50, Left =50, Width = 50, Height = 50, DialogResult = DialogResult.No };

            yes.Click += (sender, e) => { prompt.Close(); };
            no.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(yes);
            prompt.Controls.Add(no);

            return prompt.ShowDialog() == DialogResult.OK;
        }

        public static string ShowSingleDialog(string text, string caption, bool isPassword=false)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Width = 400 };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            if (isPassword) { textBox.PasswordChar = '*'; }
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        public static (string, string) ShowDoubleDialog((string, string) text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 225,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel1 = new Label() { Left = 50, Top = 20, Text = text.Item1, Width = 400 };
            TextBox textBox1 = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Label textLabel2 = new Label() { Left = 50, Top = 100, Text = text.Item2, Width = 400 };
            TextBox textBox2 = new TextBox() { Left = 50, Top = 130, Width = 400 };
            textBox2.PasswordChar = '*';
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 150, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox1);
            prompt.Controls.Add(textLabel1);
            prompt.Controls.Add(textBox2);
            prompt.Controls.Add(textLabel2);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? (textBox1.Text, textBox2.Text) : ("", "");
        }
    }
}
