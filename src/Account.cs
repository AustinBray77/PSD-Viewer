using System.Windows.Forms;

namespace PSD_Viewer
{
    //Class for holding accounts
    public class Account
    {
        //(Account) Name and the (Password) Value
        public string Name = "";
        public string Password = "";

        //Method used to copy the password to the clipboard
        public void Copy() => Clipboard.SetText(Password);

        //Base constructor
        public Account(string name, string value)
        {
            this.Name = name;
            this.Password = value;
        }

        //Method for generating a password
        public Account(string name, (int, bool, bool, bool) psdOptions)
        {
            this.Name = name;

            //Exclude certain ASCII ranges based on the password optioons
            int min = psdOptions.Item2 ? 33 :
                psdOptions.Item3 ? 48 :
                    psdOptions.Item4 ? 65 : 97;

            int max = psdOptions.Item2 ? 127 : 122;

            //Loop for the length of the password
            for (int i = 0; i < psdOptions.Item1; i++)
            {
                //Generate a random ascii number
                int number = Program.Random.Next(min, max);

                //Removes the '[' character
                number = number == 91 ? 94 : number;

                //If the password should not have spec chars, exclude their ranges
                if (!psdOptions.Item2)
                {
                    number = number >= 58 && number <= 64 ? number - 7 : number;
                    number = number >= 91 && number <= 96 ? number - 5 : number;
                }
                //If the password should not have numbers, exclude its range
                if (!psdOptions.Item3)
                {
                    number = number >= 48 && number <= 57 ? number + 17 : number;
                }
                //If the password should not have upper case letters, exlude its range
                if (!psdOptions.Item4)
                {
                    number = number >= 65 && number <= 90 ? number + 30 : number;
                }

                //Add the new number as a character to the password
                Password += (char)number;
            }
        }
    }
}
