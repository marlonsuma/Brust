using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    public class PrinterRepository : IPrinterRepository
    {
        private readonly IDbConnection _db;

        public PrinterRepository(IRepositoryConfigurationProvider config)
        {
            _db = new SqlConnection(config.ConnectionString);
        }

        public List<Printer> Get()
        {
            return _db.GetAll<Printer>().ToList();
        }
    }
}