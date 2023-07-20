using Microsoft.VisualBasic.Devices;
using ProBotTelegramClient.Inputs.InputHook.LowLevelHook;
using ProBotTelegramClient.Inputs.InputsCompilerDir;
using ProBotTelegramClient.Inputs.InputsData;
using ProBotTelegramWinForm;
using ProBotTelegramWinForm.Inputs;
using System.Diagnostics;
using System.Windows.Forms;
using UserSimulator.Inputs.InputHook;

namespace ProBotTelegramClient.Inputs.InputHook
{
	public class InputHook : IDisposable
	{
		public static bool isRecord;
		public static bool isImmitate;
		public static bool isPause;

		public bool isKeyPress;
		public static int lastKey;

		private static HookSettings? hookSettings;
		private static LowLevelKeyboardHook LowLevelKeyboard;
		private static LowLevelMouseHook LowLevelMouse;

		public static event Action OnHookStart;
		public static event Action OnHookStop;
		public static event Action OnHookPause;
		public static event Action OnHookContinue;

		public static void Start(HookSettings? settings = default(HookSettings))
		{
			isRecord = true;
			isPause = false;
			isImmitate = false;
			hookSettings = settings;
			InputsCompiler.InputHandler = settings?.InputEventArgs;

			OnHookStart?.Invoke();
		}
		public static  void StartImmitate(HookSettings? settings = default(HookSettings))
		{
			isRecord = true;
			isPause = false;
			isImmitate = true;
			hookSettings = settings;
			InputsCompiler.InputHandler = settings?.InputEventArgs;

			OnHookStart?.Invoke();
		}
		public static void Stop()
		{
			isRecord = false;
			hookSettings?.OnHookStop?.Invoke();

			OnHookStop?.Invoke();
		}
		private static bool HookConditions()
		{
			if (!isRecord) return false;
			if (isPause) return false;
			if (isImmitate) return false;

			return true;
		}

		public void HookActivate()
		{
			LowLevelKeyboard = new LowLevelKeyboardHook();
			LowLevelMouse = new LowLevelMouseHook();

			LowLevelKeyboard.KeyDown += KeyboardHook_KeyDown;
			LowLevelKeyboard.KeyUp += KeyboardHook_KeyUp;
			LowLevelMouse.MouseDown += MouseHook_MouseDown;
			LowLevelMouse.MouseUp += MouseHook_MouseUp;
			LowLevelMouse.MouseMove += MouseHook_MouseMove;
		}

		public static void KeyboardHook_KeyDown(object sender, KeyEventArgs e)
		{
			hookSettings?.OnKeyDown?.Invoke(e);

			if (e.KeyValue == 165)
			{
				if(isPause) OnHookContinue?.Invoke();
				else OnHookPause?.Invoke();

				isPause = !isPause;
				return;
			}
			if (!HookConditions()) return;

			if (lastKey == (int)e.KeyCode) return;
			else lastKey = (int)e.KeyCode;

			InputsCompiler.AddInput(new KeyInput(InputType.KeyDown, new KeyData(e)));
		}
		public static void KeyboardHook_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 165) return;
			if (!HookConditions()) return;

			if (lastKey == (int)e.KeyCode) lastKey = 0;

			InputsCompiler.AddInput(new KeyInput(InputType.KeyUp, new KeyData(e)));
		}

		public static void MouseHook_MouseDown(object sender, MouseEventArgs e)
		{
			if (!HookConditions()) return;

			InputsCompiler.AddInput(new MouseInput(InputType.MouseDown, new MouseData(e)));
		}
		public static void MouseHook_MouseUp(object sender, MouseEventArgs e)
		{
			if (!HookConditions()) return;

			InputsCompiler.AddInput(new MouseInput(InputType.MouseUp, new MouseData(e)));
		}
		public static void MouseHook_MouseMove(object sender, MouseEventArgs e)
		{
			if (!HookConditions()) return;

			InputsCompiler.AddInput(new MouseInput(InputType.MouseWeel, new MouseData(e)));
		}

		public void Dispose()
		{
			if(LowLevelKeyboard is not null) LowLevelKeyboard.Dispose();
			if(LowLevelMouse is not null) LowLevelMouse.Dispose();

			InputsCompiler.InputHandler = null;
		}
	}
}
