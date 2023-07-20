using InfoMailing.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegarmBot_Jmenka.Database
{
	public class DBcontroller : DbContext
	{
        public DBcontroller(string connectionString) : base(connectionString)
        {

        }

		public DbSet<UserInfo> Users { get; set; }
	}
}
