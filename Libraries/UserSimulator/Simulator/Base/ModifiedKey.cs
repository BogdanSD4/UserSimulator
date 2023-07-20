using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace ProBotTelegramClient.Simulator.Base
{
	public struct ModifiedKey
	{
        public ModifiedKey(IEnumerable<Keys> modifiedKeys, IEnumerable<Keys> simpleKeys)
        {
            modifiedKey = modifiedKeys.Select(x => (VirtualKeyCode)x);
			simpleKey = simpleKeys.Select(x => (VirtualKeyCode)x);
        }
        public IEnumerable<VirtualKeyCode> modifiedKey;
		public IEnumerable<VirtualKeyCode> simpleKey;
	}
}
