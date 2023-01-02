using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PSD_Viewer
{
    //Class for the main window of the program
    public partial class Form1 : Form
    {
        //Contrustor, initializes the form (boiler plate)
        public Form1()
        {
            InitializeComponent();
        }

        //Private array foor the account panels (groups)
        Panel[] _panels;

        //Method called when the window initially loads
        private void Form1_Load(object sender, EventArgs e)
        {
            //Gets the encryption key from the user
            Program.Key = Prompt.ShowSingleDialog("Please Enter your password", "Password Prompt", true);

            //If the key is not given, exit the program
            if(string.IsNullOrEmpty(Program.Key))
            {
                Close();
                return;
            }

            //Otherwise reload all elements on the window
            Reload();
        }

        //Method called when the value of the scrollbar on the window is changed
        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            //Loop through each panel and move it based on the amount the scrollbar is moved (scrolling effect)
            for(int i = 0; i < _panels.Length; i++)
            {
                _panels[i].Location = new Point(0, 35 + i * 75 - (int)((75 * (_panels.Length+3) - Size.Height) * (vScrollBar1.Value / (float)vScrollBar1.Maximum)));
            }

            //Repaint the winodw
            OnPaint(null);
        }

        //Method called when the size of the window is changed
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //Move the scrollbar and make it visible if there is overflow from the account panels
            vScrollBar1.Visible = 75 * _panels.Length > Size.Height;
            vScrollBar1.Location = new Point(Size.Width - 40, 0);
            vScrollBar1.Size = new Size(25, Size.Height-25);

            //Loop through each panel
            for (int i = 0; i < _panels.Length; i++)
            {
                //Change the width and location of the panel
                _panels[i].Size = new Size(Size.Width - 50, 75);
                _panels[i].Location = new Point(0, 35 + i * 75 - (int)((75 * (_panels.Length+3) - Size.Height) * (vScrollBar1.Value / (float)vScrollBar1.Maximum)));

                //Change the size of the account name label and the location of the three accompanying buttons
                _panels[i].Controls[0].Size = new Size(Size.Width - 200, 75);
                _panels[i].Controls[1].Location = new Point(Size.Width - 200, 0);
                _panels[i].Controls[2].Location = new Point(Size.Width - 200, 25);
                _panels[i].Controls[3].Location = new Point(Size.Width - 125, 25);
            }
        }

        //Method called to reload the account panel elements
        private void Reload()
        {
            //Clears all elements on the window
            Controls.Clear();

            //Resets the scrollbar (boiler plate)
            vScrollBar1.Location = new Point(Size.Width - 40, 0);
            vScrollBar1.Name = "vScrollBar1";
            vScrollBar1.Size = new Size(25, Size.Height - 25);
            vScrollBar1.TabIndex = 0;
            vScrollBar1.ValueChanged += new System.EventHandler(vScrollBar1_ValueChanged);
            vScrollBar1.Visible = 75 * _panels.Length > Size.Height;

            //Adds it to the controls
            Controls.Add(vScrollBar1);

            //Gets the password list from the stored file
            List<Account> psds = IO.LoadFile();
            _panels = new Panel[psds.Count];

            //Creates the panel for each password
            for (int i = 0; i < psds.Count; i++)
            {
                //Label creation
                TextBox label = new TextBox();
                label.Text = psds[i].Name;
                label.Font = new Font(Program.Font, 14, FontStyle.Regular);
                label.ReadOnly = true;
                label.BorderStyle = 0;
                label.BackColor = BackColor;
                label.TabStop = false;
                label.Multiline = true;
                label.Size = new Size(150, 75);
                label.Location = new Point(0, 0);

                //Copy button creation
                Button buttonCopy = new Button();
                buttonCopy.Text = "Copy";
                buttonCopy.Font = new Font(Program.Font, 10, FontStyle.Regular);
                buttonCopy.Size = new Size(150, 25);
                buttonCopy.Location = new Point(Size.Width - 200, 0);

                int x = i;
                //On button copy click, copy the password and inform the user of their action
                buttonCopy.Click += new EventHandler((obj, _event) => { psds[x].Copy(); MessageBox.Show("Password for " + psds[x].Name + " copied."); });

                //Change button creation
                Button buttonChange = new Button();
                buttonChange.Text = "Change";
                buttonChange.Font = new Font(Program.Font, 10, FontStyle.Regular);
                buttonChange.Size = new Size(75, 25);
                buttonChange.Location = new Point(Size.Width - 200, 25);

                //on button change click...
                buttonChange.Click += new EventHandler((obj, _event) =>
                {
                    //Prompt the user to enter their new password
                    string newPass = Prompt.ShowSingleDialog("New Password:", "Set New Password", true);

                    //Return if nothing is given
                    if (newPass == "") return;

                    //Change the password in the file
                    Account newPSD = new Account(psds[x].Name, newPass);
                    bool success = IO.ChangePassword(newPSD, psds[x]);

                    //If the password failed to remove inform the user
                    if (!success)
                    {
                        MessageBox.Show("Password failed to change due to file error...");
                        return;
                    }

                    //Show the user that the password has been changed and reload the elements
                    MessageBox.Show($"Password for {psds[x].Name} has been changed.");
                    Reload();

                });

                //Remove button creation
                Button buttonRemove = new Button();
                buttonRemove.Text = "Remove";
                buttonRemove.Font = new Font(Program.Font, 10, FontStyle.Regular);
                buttonRemove.Size = new Size(75, 25);
                buttonRemove.Location = new Point(Size.Width - 125, 25);

                //On button remove click...
                buttonRemove.Click += new EventHandler((obj, _event) =>
                {
                    //Confirm action from the user
                    if(!Prompt.YesNoDialog("Are you sure you want to remove this password?"))
                    {
                        return;
                    }

                    //Remove the password from the file
                    bool success = IO.RemovePassword(psds[x]);

                    //If the password failed to remove inform the user
                    if (!success)
                    {
                        MessageBox.Show("Password failed to remove due to file error...");
                        return;
                    }

                    //Inform the user of the successful removal and reload the elements
                    MessageBox.Show($"Password for {psds[x].Name} has been removed.");
                    Reload();
                });

                //Creates a new panel to hold all the elements
                Panel panel = new Panel();
                panel.Size = new Size(Size.Width, 75);
                panel.Controls.Add(label);
                panel.Controls.Add(buttonCopy);
                panel.Controls.Add(buttonChange);
                panel.Controls.Add(buttonRemove);
                panel.Location = new Point(0, 35 + i * 75);

                //Add the panel and save it in the array
                Controls.Add(panel);
                _panels[i] = panel;
            }

            //Creates a panel at the top
            ToolStripPanel tspTop = new ToolStripPanel();
            tspTop.Dock = DockStyle.Top;

            //Creates a strip with 2 items
            ToolStrip tsTop = new ToolStrip();
            tsTop.Items.Add("Add Password");
            tsTop.Items.Add("Generate Password");
            tsTop.Items[0].Click += Add_Password_Click;
            tsTop.Items[1].Click += Generate_Password_Click;
            tsTop.Font = new Font(Program.Font, 12, FontStyle.Regular);

            //Adds the strip and top panel to the window
            tspTop.Join(tsTop);
            Controls.Add(tspTop);
            Controls.SetChildIndex(tspTop, 0);
        }

        //Method called when the add password button is clicked
        private void Add_Password_Click(object sender, EventArgs e)
        {
            //Gets account name and password from the user
            (string, string) data = Prompt.ShowDoubleDialog(("Account Name:", "Password:"), "Add A Password");
            
            //If no data is given, return
            if (data.Item1 == "" || data.Item2 == "") return;
            
            //Add the password to the file
            bool success = IO.AddPassword(new Account(data.Item1, data.Item2));
            
            //If the password failed to add inform the user
            if(!success)
            {
                MessageBox.Show("Password failed to add due to file error...");
                return;
            }

            //Inform the user of the success and reload the elements
            MessageBox.Show("Password Added.");
            Reload();
        }

        //Method called whne the generate password button is clicked
        private void Generate_Password_Click(object sender, EventArgs e)
        {
            //Gets password parameters from the user
            (string, int, bool, bool, bool) data = Prompt.GeneratePasswordDialog();

            //If no data is given, return
            if (data.Item1 == "") return;

            //Generates a new password and adds it to the file
            bool success = IO.AddPassword(new Account(data.Item1, (data.Item2, data.Item3, data.Item4, data.Item5)));

            //If the password failed to add inform the user
            if (!success)
            {
                MessageBox.Show("Password failed to generate due to file error...");
                return;
            }

            //Inform the user of the success and reload the elements
            MessageBox.Show("Password Generated and Added.");
            Reload();
        }
    }
}
