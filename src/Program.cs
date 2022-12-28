using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PSDViewer
{
    static class Program
    {
        static string dir => Directory.GetCurrentDirectory();
        static Random ran = new Random();

        public class PSD
        {
            public string name = "";
            public string value = "";

            public void Copy()
            {
                Clipboard.SetText(value);
            }

            public PSD(string name, string value)
            {
                this.name = name;
                this.value = value;
            }

            public PSD(string name)
            {
                this.name = name;

                for(int i = 0; i < 16; i++)
                {
                    int number = ran.Next(33, 127);
                    number = number == 91 ? 94 : number;
                    value += (char)number;
                }
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static List<PSD> LoadFile() 
        {
            List<PSD> output = new List<PSD>();
            try
            {
                StreamReader sr = File.OpenText(dir + "\\psds\\psd.txt");

                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    line = EncryptorLIB.Encryptor.DecryptWithKey(line, Form1.key);
                    string[] x = line.Split('[');

                    if (x.Length != 2)
                        continue;

                    output.Add(new PSD(x[0], x[1]));
                }

                sr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }

            return output;
        }

        public static bool AddPassword(PSD password)
        {
            try
            {
                StreamWriter file = new StreamWriter(dir + "\\psds\\psd.txt", append: true);
                string line = EncryptorLIB.Encryptor.EncryptWithKey(password.name + "[" + password.value, Form1.key);
                file.WriteLine(line);
                file.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }

            return true;
        }

        public static bool RemovePassword(PSD password)
        {
            try
            {
                StreamReader sr = File.OpenText(dir + "\\psds\\psd.txt");

                string encryptedData = EncryptorLIB.Encryptor.EncryptWithKey(password.name + "[" + password.value, Form1.key);
                string line;
                string data = "";

                while((line = sr.ReadLine()) != null)
                {
                    if(encryptedData != line)
                    {
                        data += line + "\n";
                    }
                }

                sr.Close();

                StreamWriter sw = new StreamWriter(dir + "\\psds\\psd.txt", append: false);
                sw.Write(data);
                sw.Close();

            } catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }

            return true;
        }

        public static bool ChangePassword(PSD newPassword, PSD originalPassword)
        {
            try
            {
                StreamReader sr = File.OpenText(dir + "\\psds\\psd.txt");

                string encryptedData = EncryptorLIB.Encryptor.EncryptWithKey(originalPassword.name + "[" + originalPassword.value, Form1.key), 
                    newData = EncryptorLIB.Encryptor.EncryptWithKey(newPassword.name + "[" + newPassword.value, Form1.key);
                string line;
                string data = "";

                while ((line = sr.ReadLine()) != null)
                {
                    if (encryptedData != line)
                    {
                        data += line + "\n";
                    } else
                    {
                        data += newData + "\n";
                    }
                }

                sr.Close();

                StreamWriter sw = new StreamWriter(dir + "\\psds\\psd.txt", append: false);
                sw.Write(data);
                sw.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }

            return true;
        }
    }
}
