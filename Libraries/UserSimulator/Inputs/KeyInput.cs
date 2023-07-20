using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json;
using ProBotTelegramClient.Inputs.BaseInputs;
using ProBotTelegramClient.Inputs.InputsData;
using ProBotTelegramClient.Simulator.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProBotTelegramWinForm.Inputs
{
    public class KeyInput : Input
	{
        public KeyInput(InputType type, KeyData key) : base(BaseInputType.Key, type)
		{
			if (key.isModified)
			{
				if (type is InputType.KeyDown)
				{
					currentModified.Add(key.keyCode);
				}
				else
				{
					currentModified.Remove(key.keyCode);
				}
			}
			else
			{
				if (type is InputType.KeyDown)
				{
					if (currentModified.Count == 0)
					{
						Type = InputType.KeyPress;
					}
				}
			}

			Keys = key;
		}

        public KeyData Keys { get; set; }
		private static List<Keys> currentModified = new List<Keys>();
		private static bool IsLastModified;

		public override string Info()
		{
			return string.Join(';', BaseType, Type, JsonConvert.SerializeObject(Keys));
		}
		public override bool InputConnection(Input input)
		{
			if(input is KeyInput keyInput)
			{
				if (keyInput.Type is InputType.KeyUp)
				{
					if (keyInput.Keys.isModified && !IsLastModified)
					{
						if (currentModified.Count > 0)
						{
							IsLastModified = true;
							keyInput.Type = InputType.KeyModified;
							keyInput.Keys.modifiedKeys.AddRange(currentModified);
							return false;
						}
						else
						{
							keyInput.Type = InputType.KeyPress;
							return false;
						}
					}
					return true;
				}
				if(keyInput.Type is InputType.KeyDown)
				{
					if (IsLastModified) IsLastModified = false;
					if (!keyInput.Keys.isModified && currentModified.Count > 0 && !IsLastModified)
					{
						IsLastModified = true;
						keyInput.Type = InputType.KeyModified;
						keyInput.Keys.modifiedKeys.AddRange(currentModified);
						return false;
					}
				}
			}

			return false;
		}
		public override void SetAction()
		{
			Simulation = Type switch
			{
				InputType.KeyPress => () => { InputSimulatorBuilder.SimulateKeyPress(Keys.keyCode); },
				InputType.KeyModified => () => { InputSimulatorBuilder.SimulateModifiedKey(new ModifiedKey(Keys.modifiedKeys, new Keys[] { Keys.keyCode })); },
			};
		}
	}
}
