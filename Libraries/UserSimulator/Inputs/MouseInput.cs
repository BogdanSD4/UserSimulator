using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json;
using ProBotTelegramClient.Inputs.BaseInputs;
using ProBotTelegramClient.Inputs.InputsData;
using ProBotTelegramClient.Simulator.Base;
using System.Diagnostics;

namespace ProBotTelegramWinForm.Inputs
{
    public class MouseInput : Input
	{
        public MouseInput(InputType type, MouseData mouseData) : base(BaseInputType.Mouse, type)
        {
			if (type is InputType.MouseDown)
			{
				if (ListOfPressedButton.ContainsKey(mouseData.Button)) return;
				ListOfPressedButton.Add(mouseData.Button, mouseData);
			}

			MouseData = mouseData;
		}

		public MouseData MouseData { get; set; }
		private static IDictionary<MouseButtons, MouseData> ListOfPressedButton { get; set; } = new Dictionary<MouseButtons, MouseData>();
		private static MouseInput? LastAction;

		public override string Info()
		{
			return string.Join(';', BaseType, Type, JsonConvert.SerializeObject(MouseData));
		}
		public override bool InputConnection(Input input)
		{
			if(input is MouseInput mouseInput)
			{
				var buttType = mouseInput.MouseData.Button;

				if(mouseInput.Type is InputType.MouseUp)
				{
					var butt = ListOfPressedButton[buttType];
					ListOfPressedButton.Remove(mouseInput.MouseData.Button);

					if (butt == mouseInput.MouseData)
					{
						if (LastAction is not null && LastAction.MouseData.IsTimeCorrect(mouseInput.MouseData))
						{
							Type = buttType switch
							{
								MouseButtons.Left => InputType.LeftMouseDouble,
								MouseButtons.Right => InputType.RightMouseDouble,
								MouseButtons.Middle => InputType.MiddleMouseDouble,
								_ => throw new Exception("Type not registered")
							};

							LastAction.Type = InputType.MouseDown;
							LastAction = null;
							return true;
						}
						else
						{
							Type = buttType switch
							{
								MouseButtons.Left => InputType.LeftMouseClick,
								MouseButtons.Right => InputType.RightMouseClick,
								MouseButtons.Middle => InputType.MiddleMouseClick,
								_ => throw new Exception("Type not registered")
							};

							LastAction = mouseInput;
							return true;
						}
					}
					else
					{
						Type = buttType switch
						{
							MouseButtons.Left => InputType.LeftMouseSelect,
							MouseButtons.Right => InputType.RightMouseSelect,
							MouseButtons.Middle => InputType.MiddleMouseSelect,
							_ => throw new Exception("Type not registered")
						};
						MouseData = new MouseData(mouseInput.MouseData, butt.x1, butt.y1);
						return true;
					}
				}
				else if(mouseInput.Type is InputType.MouseWeel)
				{
					if(Type is InputType.MouseWeel)
					{
						var md = mouseInput.MouseData;
						MouseData = new MouseData(MouseData, md.delta, md.x1, md.y1);
						return true;
					}
				}
				return false;
			}
			else
			{
				return false;
			}
		}
		public override void SetAction()
		{
			Simulation = Type switch
			{
				InputType.LeftMouseClick or InputType.RightMouseClick or InputType.MiddleMouseClick => () => 
				{
					InputSimulatorBuilder.SimulateMouseClick(MouseData.Button, MouseData.x1, MouseData.y1); 
				},
				InputType.LeftMouseDouble or InputType.RightMouseDouble or InputType.MiddleMouseDouble => () =>
				{
					InputSimulatorBuilder.SimulateMouseDouble(MouseData.Button, MouseData.x1, MouseData.y1);
				},
				InputType.LeftMouseSelect or InputType.RightMouseSelect or InputType.MiddleMouseSelect => () =>
				{
					InputSimulatorBuilder.SimulateMouseDown(MouseData.Button, MouseData.x1, MouseData.y1);
					InputSimulatorBuilder.SimulateMouseUp(MouseData.Button, MouseData.x2, MouseData.y2);
				},
				InputType.MouseWeel => () =>
				{
					InputSimulatorBuilder.SimulateMouseWeel(MouseData.x1, MouseData.y1, MouseData.delta);
				}
			};
		}
	}
}
