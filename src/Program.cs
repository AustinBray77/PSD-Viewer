using System;
using System.Windows.Forms;
using System.IO;

namespace PSD_Viewer
{
    //Entry point class
    static class Program
    {
        //Static instance for the directory, random, encryption key and font for the window
        public static string Dir => Directory.GetCurrentDirectory();
        public static Random Random = new Random();
        public static string Key = "";
        public const string Font = "Verdana";

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
    }
}
