using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DonorTracking.Data
{
    public class CaseRepository : ICaseRepository
    {
        private readonly IDbConnection _db;

        public CaseRepository(IRepositoryConfigurationProvider config)
        {
            _db = new SqlConnection(config.ConnectionString);
        }

        public Case Get(string barcode)
        {
            string sql = "SELECT C.*, LO.[Name] AS [Location], L.[Barcode] AS[LotBarcode] " +
                         "FROM [tblCases] C " +
                         "LEFT JOIN [tblLots] L ON C.[LotID]=L.[ID] " +
                         "LEFT JOIN [tblLocations] LO ON C.[LocationID]=LO.[ID]" +
                         "WHERE C.[Barcode]=@barcode";

            return _db.Query<Case>(sql, new { barcode }).SingleOrDefault();
        }

        public List<Case> GetByLot(int lotId)
        {
            string sql = "SELECT C.*, LO.[Name] AS [Location], L.[Barcode] AS[LotBarcode] " +
                         "FROM [tblCases] C " +
                         "LEFT JOIN [tblLots] L ON C.[LotID]=L.[ID] " +
                         "LEFT JOIN [tblLocations] LO ON C.[LocationID]=LO.[ID]" +
                         "WHERE C.[LotID] = @lotId";

            return _db.Query<Case>(sql, new { lotId }).ToList();
        }
    }
}