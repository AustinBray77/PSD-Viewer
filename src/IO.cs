using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PSD_Viewer
{
    //Class for handling all file input and output
    class IO
    {
        //Method called to load the storage file
        public static List<Account> LoadFile()
        {
            //Creates a list for the output
            List<Account> output = new List<Account>();
            try
            {
                //Loads the storage file
                StreamReader sr = File.OpenText(Program.Dir + "\\psds\\psd.txt");

                //Reads each line from the file
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    //Decrypts the line using the key
                    line = EncryptorLIB.Encryptor.DecryptWithKey(line, Program.Key);
                    
                    //Splits the line at the split character
                    string[] x = line.Split('[');

                    //Continue if the line didnot split correctly
                    if (x.Length != 2)
                        continue;

                    //Otherwise add the new account using the lines data
                    output.Add(new Account(x[0], x[1]));
                }

                //Close the file
                sr.Close();
            }
            //Catch any exception and show them to the user then exit
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }

            //Returns the output if there were no errors
            return output;
        }

        //Method to add a password to the file
        public static bool AddPassword(Account account)
        {
            try
            {
                //Opens the storage file
                StreamWriter file = new StreamWriter(Program.Dir + "\\psds\\psd.txt", append: true);

                //Encrypt the password
                string line = EncryptorLIB.Encryptor.EncryptWithKey(account.Name + "[" + account.Password, Program.Key);

                //Write to the file and close it
                file.WriteLine(line);
                file.Close();
            }
            //Catch any exception and show them to the user then return false
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //Returns that the operation was successful
            return true;
        }

        //Method to remove a password from the file
        public static bool RemovePassword(Account account)
        {
            try
            {
                //Opens the storage file
                StreamReader sr = File.OpenText(Program.Dir + "\\psds\\psd.txt");

                //Encrypts the given account
                string encryptedData = EncryptorLIB.Encryptor.EncryptWithKey(account.Name + "[" + account.Password, Form1.Key);
                
                //Loops through each line of the file
                string line;
                string data = "";
                while ((line = sr.ReadLine()) != null)
                {
                    //Read all lines except the one for the given account
                    if (encryptedData != line)
                    {
                        data += line + "\n";
                    }
                }

                //Close the file instance
                sr.Close();

                //Open the file again for writing
                StreamWriter sw = new StreamWriter(Program.Dir + "\\psds\\psd.txt", append: false);
                
                //Write the newly changed data to the file and return
                sw.Write(data);
                sw.Close();

            }
            //Catch any exception and show them to the user then return false
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //Returns that the operation was successful
            return true;
        }

        //Method to change the password of an account
        public static bool ChangePassword(Account newPassword, Account originalPassword)
        {
            try
            {
                //Opens the storage file
                StreamReader sr = File.OpenText(Program.Dir + "\\psds\\psd.txt");

                //Encrypts the old and new data
                string encryptedData = EncryptorLIB.Encryptor.EncryptWithKey(originalPassword.Name + "[" + originalPassword.Password, Program.Key),
                    newData = EncryptorLIB.Encryptor.EncryptWithKey(newPassword.Name + "[" + newPassword.Password, Program.Key);
                
                //Loops through each line of the file
                string line;
                string data = "";
                while ((line = sr.ReadLine()) != null)
                {
                    //If the line is not equal to the old account, add it unchanged
                    if (encryptedData != line)
                    {
                        data += line + "\n";
                    }
                    //Otherwise swap the line with the new data
                    else
                    {
                        data += newData + "\n";
                    }
                }

                //Close the file
                sr.Close();

                //Opens the file for writing
                StreamWriter sw = new StreamWriter(Program.Dir + "\\psds\\psd.txt", append: false);
                
                //Writes the data to the file and closes it
                sw.Write(data);
                sw.Close();

            }
            //Catch any exception and show them to the user then return false
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //Returns that the operation was successful
            return true;
        }
    }
}
