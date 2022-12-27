using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSDViewer
{
    public partial class Form1 : Form
    {
        public static string key = "";

        public Form1()
        {
            InitializeComponent();
        }

        Panel[] panels;

        private void Form1_Load(object sender, EventArgs e)
        {
            key = Prompt.ShowSingleDialog("Please Enter your password", "Password Prompt", true);

            if(String.IsNullOrEmpty(key))
            {
                Close();
                return;
            }

            Reload();
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            for(int i = 0; i < panels.Length; i++)
            {
                panels[i].Location = new Point(0, 35 + i * 75 - (int)((75 * (panels.Length + 2) - Size.Height) * (vScrollBar1.Value / (float)vScrollBar1.Maximum)));
            }

            OnPaint(null);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            vScrollBar1.Visible = 75 * panels.Length > Size.Height;
            vScrollBar1.Location = new Point(Size.Width - 40, 0);
            vScrollBar1.Size = new Size(25, Size.Height-50);

            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].Size = new Size(Size.Width - 50, 75);
                panels[i].Location = new Point(0, 35 + i * 75 - (int)((75 * (panels.Length + 1) - Size.Height) * (vScrollBar1.Value / (float)vScrollBar1.Maximum)));

                panels[i].Controls[0].Size = new Size(Size.Width - 200, 75);
                panels[i].Controls[1].Location = new Point(Size.Width - 200, 0);
                panels[i].Controls[2].Location = new Point(Size.Width - 200, 25);
                panels[i].Controls[3].Location = new Point(Size.Width - 125, 25);
            }
        }

        private void Reload()
        {
            Controls.Clear();

            this.vScrollBar1.Location = new System.Drawing.Point(325, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(25, 500);
            this.vScrollBar1.TabIndex = 0;
            this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);

            Controls.Add(vScrollBar1);

            List<Program.PSD> psds = Program.LoadFile();
            panels = new Panel[psds.Count];

            for (int i = 0; i < psds.Count; i++)
            {
                TextBox label = new TextBox();
                label.Text = psds[i].name;
                label.Font = new Font("Yu Gothic UI Semibold", 18, FontStyle.Regular);
                label.ReadOnly = true;
                label.BorderStyle = 0;
                label.BackColor = BackColor;
                label.TabStop = false;
                label.Multiline = true;
                label.Size = new Size(150, 75);
                label.Location = new Point(0, 0);

                Button buttonCopy = new Button();
                buttonCopy.Text = "Copy";
                buttonCopy.Size = new Size(150, 25);
                buttonCopy.Location = new Point(Size.Width - 200, 0);

                int x = i;
                buttonCopy.Click += new EventHandler((obj, _event) => { psds[x].Copy(); MessageBox.Show("Password for " + psds[x].name + " copied."); });

                Button buttonChange = new Button();
                buttonChange.Text = "Change";
                buttonChange.Size = new Size(75, 25);
                buttonChange.Location = new Point(Size.Width - 200, 25);

                Button buttonRemove = new Button();
                buttonRemove.Text = "Remove";
                buttonRemove.Size = new Size(75, 25);
                buttonRemove.Location = new Point(Size.Width - 125, 25);

                Panel panel = new Panel();
                panel.Size = new Size(Size.Width, 75);
                panel.Controls.Add(label);
                panel.Controls.Add(buttonCopy);
                panel.Controls.Add(buttonChange);
                panel.Controls.Add(buttonRemove);
                panel.Location = new Point(0, 35 + i * 75);

                Controls.Add(panel);
                panels[i] = panel;
            }

            vScrollBar1.Visible = 75 * panels.Length > Size.Height - 39;

            ToolStripPanel tspTop = new ToolStripPanel();
            tspTop.Dock = DockStyle.Top;
            ToolStrip tsTop = new ToolStrip();
            tsTop.Items.Add("Add Password");
            tsTop.Items.Add("Generate Password");
            tsTop.Items[0].Click += Add_Password_Click;
            tsTop.Items[1].Click += Generate_Password_Click;
            tspTop.Join(tsTop);
            Controls.Add(tspTop);
            Controls.SetChildIndex(tspTop, 0);
        }

        private void Add_Password_Click(object sender, EventArgs e)
        {
            (string, string) data = Prompt.ShowDoubleDialog(("Account Name:", "Password:"), "Add A Password");
            Program.AddPassword(new Program.PSD(data.Item1, data.Item2));
            Reload();
        }
        private void Generate_Password_Click(object sender, EventArgs e)
        {
            string accountName = Prompt.ShowSingleDialog("Account Name:", "Create A Password");
            Program.AddPassword(new Program.PSD(accountName));
            Reload();
        }
    }
}
