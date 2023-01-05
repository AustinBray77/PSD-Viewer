using System.Windows.Forms;

namespace PSD_Viewer
{
    //Class for creating prompts for the user to enter data
    static class Prompt
    {
        //Prompt for a yes or no question
        public static bool YesNoDialog(string caption)
        {
            //Creates a 200 x 200 window
            Form prompt = new Form()
            {
                Width = 350,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text ="Are you sure?",
                StartPosition = FormStartPosition.CenterScreen
            };

            //Creates a label with a yes or no button
            Label label = new Label() { Left = 50, Top = 20, Text = caption, Width = 250};
            Button yes = new Button() { Text = "Yes", Top=50, Left = 50, Width = 100, Height = 50, DialogResult = DialogResult.OK };
            Button no = new Button() { Text = "No", Top = 50, Left=200, Width = 100, Height = 50, DialogResult = DialogResult.No };

            //Sets the prompt to close with the answer when either button is pressed
            yes.Click += (sender, e) => { prompt.Close(); };
            no.Click += (sender, e) => { prompt.Close(); };

            //Adds the elements to the window
            prompt.Controls.AddRange(new Control[] { label, yes, no });

            //Returns the result of the window (true -> yes, false -> no)
            return prompt.ShowDialog() == DialogResult.OK;
        }

        //Prompt for a single text answer
        public static string ShowSingleDialog(string text, string caption, bool isPassword=false)
        {
            //Creates a 500 x 150 window
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            //Creates the label and text box
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Width = 400 };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            
            //If the user is to enter a password, make sure the text box hides the characters
            if (isPassword) { textBox.PasswordChar = '*'; }

            //Creates the confirmation button
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            
            //Adds the elements to the window
            prompt.Controls.AddRange(new Control[] { textBox, confirmation, textLabel });
            prompt.AcceptButton = confirmation;

            //Returns the text in the text box based on the result of the form (if it is closed or exited, return nothing)
            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        //Prompt for getting two text inputs from the user
        public static (string, string) ShowDoubleDialog((string, string) text, string caption)
        {
            //Creates a new 500 x 225 window
            Form prompt = new Form()
            {
                Width = 500,
                Height = 225,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            //Creates 2 labels and 2 textboxes
            Label textLabel1 = new Label() { Left = 50, Top = 20, Text = text.Item1, Width = 400 };
            TextBox textBox1 = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Label textLabel2 = new Label() { Left = 50, Top = 100, Text = text.Item2, Width = 400 };
            TextBox textBox2 = new TextBox() { Left = 50, Top = 130, Width = 400 };
            
            //Assume the second box is always a password field
            textBox2.PasswordChar = '*';

            //Creates the confirmation button
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 150, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            
            //Adds the elements to the window
            prompt.Controls.AddRange(new Control[] { textBox1, textLabel1, textBox2, textLabel2, confirmation });
            prompt.AcceptButton = confirmation;

            //Returns the text in the text box based on the result of the form (if it is closed or exited, return nothing)
            return prompt.ShowDialog() == DialogResult.OK ? (textBox1.Text, textBox2.Text) : ("", "");
        }

        //Specific prompt for generating a password (single use function)
        //Return order: (account name, passwoord length, does contain spec chars, does contain nums, does contain uppercase
        public static (string, int, bool, bool, bool) GeneratePasswordDialog()
        {
            //Creates a 500 x 225 window
            Form prompt = new Form()
            {
                Width = 500,
                Height = 225,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Generate Password",
                StartPosition = FormStartPosition.CenterScreen
            };

            //Creates a 2 labels, a textbox (Account name), and 3 checkboxes and a trackbar (both are for password parameteres)
            Label textLabel1 = new Label() { Left = 50, Top = 20, Text = "Account Name:", Width = 400 };
            TextBox textBox1 = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Label textLabel2 = new Label() { Left = 50, Top = 75, Text = "Password Size:", Width = 400 };
            TrackBar trackBar = new TrackBar() { Left = 50, Top = 100, Width = 400, Minimum = 8, Maximum = 32, Value=16 };
            CheckBox checkBox1 = new CheckBox() { Left = 50, Top = 150, Width = 150, Text = "Special Characters", Checked = true };
            CheckBox checkBox2 = new CheckBox() { Left = 200, Top = 150, Width = 100, Text = "Numbers", Checked = true };
            CheckBox checkBox3 = new CheckBox() { Left = 300, Top = 150, Width = 100, Text = "UpperCase", Checked = true };

            //Creates the confirmationo button
            Button confirmation = new Button() { Text = "Ok", Left = 400, Width = 75, Top = 150, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            //Adds the elements to the window
            prompt.Controls.AddRange(new Control[] {textLabel1, textBox1, textLabel2, trackBar, checkBox1, checkBox2, checkBox3, confirmation});

            //Returns the text in the text box based on the result of the form (if it is closed or exited, return nothing)
            return prompt.ShowDialog() == DialogResult.OK ? (textBox1.Text, trackBar.Value, checkBox1.Checked, checkBox2.Checked, checkBox3.Checked) : ("", 0, false, false, false);
        }
    }
}
