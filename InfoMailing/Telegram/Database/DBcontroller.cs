using InfoMailing.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotSettings.Database
{
	public class DBcontroller : DbContext
	{
        public DBcontroller(string connectionString) : base(connectionString)
        {

        }

		public DbSet<ChatInfo> Users { get; set; }
	}
}
