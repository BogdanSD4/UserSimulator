using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ProBotTelegramClient.Inputs.InputHook.LowLevelHook
{
	class LowLevelKeyboardHook : IDisposable
	{
		private const int WH_KEYBOARD_LL = 13;
		private const int WM_KEYDOWN = 0x0100;
		private const int WM_SYSKEYDOWN = 0x0104;
		private const int WM_KEYUP = 0x0101;
		private const int WM_SYSKEYUP = 0x0105;

		private IntPtr _hookID = IntPtr.Zero;
		private LowLevelKeyboardProc _hookProc;

		public LowLevelKeyboardHook()
		{
			_hookProc = HookCallback;
			_hookID = SetHook(_hookProc);
		}

		private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

		private IntPtr SetHook(LowLevelKeyboardProc proc)
		{
			using (Process curProcess = Process.GetCurrentProcess())
			using (ProcessModule curModule = curProcess.MainModule)
			{
				return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
			}
		}

		private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0)
			{
				int vkCode = Marshal.ReadInt32(lParam);
				if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
				{
					KeyEventArgs args = new KeyEventArgs((Keys)vkCode);
					KeyDown?.Invoke(this, args);
				}
				else if (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
				{
					KeyEventArgs args = new KeyEventArgs((Keys)vkCode);
					KeyUp?.Invoke(this, args);
				}
			}

			return CallNextHookEx(_hookID, nCode, wParam, lParam);
		}

		public event EventHandler<KeyEventArgs> KeyDown;
		public event EventHandler<KeyEventArgs> KeyUp;

		public void Dispose()
		{
			UnhookWindowsHookEx(_hookID);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);
	}
}