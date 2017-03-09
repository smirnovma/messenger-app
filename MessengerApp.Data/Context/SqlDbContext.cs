using MessengerApp.Entities.Chat;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp.Data.Context
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext()
           : base("name = SqlConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SqlDbContext, Migration.Configuration>());
        }

        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<SessionKeyEntity> SessionKey { get; set; }

        public static SqlDbContext Create()
        {
            return new SqlDbContext();
        }
    }
}
