using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace ProBotTelegramClient.Inputs.BaseInputs
{
	public abstract class Input
    {
        public Input(BaseInputType baseInput, InputType type)
        {
            BaseType = baseInput;
            Type = type;
        }
        public BaseInputType BaseType { get; set; }
        public InputType Type { get; set; }
        public Action? Simulation { get; set; }

        public Input? preveous { get; set; }
        public Input? next { get; set; }

        public Input Last()
        {
            if (next is null) return this;

            return next.Last();
        }
		public void SimulateInput()
        {
            if(Simulation is not null) Simulation.Invoke();
        }


		public abstract void SetAction();
        public abstract string Info();
        public abstract bool InputConnection(Input input);
    }
}



