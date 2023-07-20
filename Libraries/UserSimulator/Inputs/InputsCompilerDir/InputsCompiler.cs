using ProBotTelegramClient.Inputs.BaseInputs;
using ProBotTelegramWinForm;
using ProBotTelegramWinForm.Inputs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProBotTelegramClient.Inputs.InputsCompilerDir
{
	public static class InputsCompiler
    {
		private static Input? inputs;
		public static Input? Inputs
		{
			get { return inputs; }
			set { inputs = value; }
		}

		private static IDictionary<BaseInputType, Input> Last { get; set; } = new Dictionary<BaseInputType, Input>();
		internal static Action<BaseInputType, Input>? InputHandler { get; set; }  
		public static void AddInput(Input input)
		{
			if (Inputs is null)
			{
				Inputs = input;
			}
			else
			{
				if (Last.ContainsKey(input.BaseType))
				{
					if (!Last[input.BaseType].InputConnection(input))
					{
						input.preveous = Last[input.BaseType];
						Inputs.Last().next = input;
					}
					else
					{
						InputHandler?.Invoke(input.BaseType, Last[input.BaseType]);
						return;
					}
				}
			}

			#region AddLast
			if (Last.ContainsKey(input.BaseType))
			{
				Last[input.BaseType] = input;
			}
			else
			{
				Last.Add(input.BaseType, input);
			}
			#endregion
		}

		public static IDictionary<int, Input> BaseCompilation()
		{
			IDictionary<int, Input> result = new Dictionary<int, Input>();

			int index = 0;
			while (inputs is not null)
			{
				#region Precondition
				if (inputs.Type is InputType.KeyDown ||
					inputs.Type is InputType.MouseUp ||
					inputs.Type is InputType.MouseDown) { }
				#endregion
				else
				{
					inputs.SetAction();
					result.Add(index, inputs);
				}

				var current = inputs.next;
				inputs.next = null;
				inputs = current;

				index++;
			}

			return result;
		}
		public static IDictionary<int, string> Compilation()
		{
			IDictionary<int, Input> inputs = BaseCompilation();
			IDictionary<int, string> result = new Dictionary<int, string>();

			foreach (var input in inputs)
			{
				#region Precondition
				if (input.Value.Type is InputType.KeyDown ||
					input.Value.Type is InputType.MouseDown)
				{
					continue;
				}
				#endregion

				result.Add(input.Key, input.Value.Info());
			}

			return result;
		}

		private static void SendInMainThread(Action action)
		{
			new MethodInvoker(action).Invoke();
		}
	}
}
