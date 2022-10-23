using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DonorTracking.Data
{
    public class LotRepository : ILotRepository
    {
        private readonly IDbConnection _db;

        public LotRepository(IRepositoryConfigurationProvider config)
        {
            _db = new SqlConnection(config.ConnectionString);
        }

        public Lot Get(string barcode)
        {
            string sql = "Select * FROM tblLots WHERE Barcode = @barcode";

            return _db.Query<Lot>(sql, new { barcode }).SingleOrDefault();
        }
    }
}