using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoMailing.Data
{
	public class User
	{
        public User(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string LastMessage { get; set; }
		public bool MenuEnabel { get; set; }
	}
}
