using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace VP_Unpack
{
    public static class OutputConsole
    {
        public static TextBox output;

        static OutputConsole()
        {
            output = Globals.mainForm.outputConsole;
        }

        public static void SendMessage(string message,bool Output,string OutputPath)
        {
            //"C:/Users/mag11/Documents/GitHub/VP-UnpackFork/VP Unpack/obj/OutputLog.txt"
            string[] s = new string[output.Lines.Length + 1];
            output.Lines.CopyTo(s, 0);
            s[output.Lines.Length] = message;
            output.Lines = s;
            output.SelectionStart = output.Text.Length;
            output.ScrollToCaret();
            if (Output) {
                File.AppendAllText(OutputPath, string.Format("{0}{1}", message, Environment.NewLine));
            }
        }
     
    public static void ClearMessages()
        {
            output.Lines = null;
        }
    }
}
