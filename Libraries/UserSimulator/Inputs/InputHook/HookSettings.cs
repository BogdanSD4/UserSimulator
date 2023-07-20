using ProBotTelegramClient.Inputs.BaseInputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSimulator.Inputs.InputHook
{
	public class HookSettings
	{
		public Action? OnHookStop { get; set; }
		public Action<KeyEventArgs>? OnKeyDown { get; set; }
		public Action<BaseInputType, Input>? InputEventArgs {get; set;}
	}
}
