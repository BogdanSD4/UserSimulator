using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ProBotTelegramClient.Inputs.InputHook.LowLevelHook
{
    class LowLevelMouseHook : IDisposable
    {
        private const int WH_MOUSE_LL = 14;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_MBUTTONDOWN = 0x0207;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONUP = 0x0205;
        private const int WM_MBUTTONUP = 0x0208;
		private const int WM_MOUSEWHEEL = 0x020A;
		private const int WM_MOUSEHWHEEL = 0x020E;

		private IntPtr _hookID = IntPtr.Zero;
        private LowLevelMouseProc _hookProc;

        public LowLevelMouseHook()
        {
            _hookProc = HookCallback;
            _hookID = SetHook(_hookProc);
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

		private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0)
			{
				int x = Marshal.ReadInt32(lParam);
				int y = Marshal.ReadInt32(lParam + 4);
				MouseEventArgs args = null;

                if (wParam == WM_LBUTTONDOWN)
                    args = new MouseEventArgs(MouseButtons.Left, 0, x, y, 0);
                else if (wParam == WM_RBUTTONDOWN)
                    args = new MouseEventArgs(MouseButtons.Right, 0, x, y, 0);
                else if (wParam == WM_MBUTTONDOWN)
                    args = new MouseEventArgs(MouseButtons.Middle, 0, x, y, 0);
                else if (wParam == WM_LBUTTONUP)
                    args = new MouseEventArgs(MouseButtons.Left, 0, x, y, 0);
                else if (wParam == WM_RBUTTONUP)
                    args = new MouseEventArgs(MouseButtons.Right, 0, x, y, 0);
                else if (wParam == WM_MBUTTONUP)
                    args = new MouseEventArgs(MouseButtons.Middle, 0, x, y, 0);
                else if (wParam == WM_MOUSEWHEEL)
                {
					MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    int delta = 1;
                    if (hookStruct.flags < 0) delta = -1;
                    args = new MouseEventArgs(MouseButtons.None, 0, x, y, delta);
                }

				if (args != null)
				{
					if (wParam == WM_LBUTTONDOWN || wParam == WM_RBUTTONDOWN || wParam == WM_MBUTTONDOWN)
						MouseDown?.Invoke(this, args);
					else if (wParam == WM_LBUTTONUP || wParam == WM_RBUTTONUP || wParam == WM_MBUTTONUP)
						MouseUp?.Invoke(this, args);
					else if (wParam == WM_MOUSEWHEEL || wParam == WM_MOUSEHWHEEL)
						MouseMove?.Invoke(this, args);
				}
			}

			return CallNextHookEx(_hookID, nCode, wParam, lParam);
		}

		public event EventHandler<MouseEventArgs> MouseDown;
        public event EventHandler<MouseEventArgs> MouseUp;
        public event EventHandler<MouseEventArgs> MouseMove;

        public void Dispose()
        {
            UnhookWindowsHookEx(_hookID);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

		[StructLayout(LayoutKind.Sequential)]
		private struct MSLLHOOKSTRUCT
		{
			public int pt;
			public int mouseData;
			public int flags;
			public int time;
			public IntPtr dwExtraInfo;
		}
	}
}