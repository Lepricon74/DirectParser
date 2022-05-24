using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Direct.Parser.Database.Interfaces;

namespace Direct.Parser.Database.Providers
{
    public class NpgsqlConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string connectionString;
        public NpgsqlConnectionStringProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public string GetConnectionString()
        {
            return connectionString;
        }
    }
}
