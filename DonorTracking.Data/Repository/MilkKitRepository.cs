using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    public class MilkKitRepository : IMilkKitRepository
    {
        private readonly IDbConnection _db;

        public MilkKitRepository(IRepositoryConfigurationProvider config)
        {
            _db = new SqlConnection(config.ConnectionString);
        }

        public MilkKitRepository(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public MilkKit Add(MilkKit milkKit)
        {
            long id = _db.Insert(milkKit);
            milkKit.Id = (int) id;

            return milkKit;
        }

        public List<MilkKit> Get()
        {
            return _db.GetAll<MilkKit>().ToList();
        }

        public MilkKit Get(string barcode)
        {
            return _db.QuerySingleOrDefault<MilkKit>("SELECT * FROM tblMilkKits WHERE Barcode = @barcode",
                new { barcode });
        }

        public List<MilkKit> GetByDonor(string donorId)
        {
            var sql = "SELECT * FROM tblMilkKits M " +
                      "LEFT JOIN tblLots L on L.ID = M.LotID " +
                      "WHERE M.DonorID = @donorId";
            List<MilkKit> data = _db.Query<MilkKit, Lot, MilkKit>(sql, (milkKit, lot) =>
                {
                    milkKit.Lot = lot;

                    return milkKit;
                }, new { donorId }).ToList();

            return data;
        }

        public List<MilkKit> GetByLot(int lotId)
        {
            string sql = "SELECT * FROM tblMilkKits WHERE LotID = @lotId";

            return _db.Query<MilkKit>(sql, new { lotId }).ToList();
        }

        public List<MilkKit> GetWithLot()
        {
            var sql = "SELECT * FROM tblMilkKits M " +
                      "LEFT JOIN tblLots L on L.ID = M.LotID";
            List<MilkKit> data = _db.Query<MilkKit, Lot, MilkKit>(sql, (milkKit, lot) =>
                {
                    milkKit.Lot = lot;

                    return milkKit;
                }).ToList();

            return data;
        }

        public MilkKit GetWithLot(string barcode)
        {
            var sql = "SELECT * FROM tblMilkKits M " +
                      "LEFT JOIN tblLots L on L.ID = M.LotID " +
                      "WHERE M.Barcode = @Barcode";
            MilkKit data = _db.Query<MilkKit, Lot, MilkKit>(sql, (milkKit, lot) =>
                {
                    milkKit.Lot = lot;

                    return milkKit;
                }, new { barcode }).FirstOrDefault();

            return data;
        }

        public bool Remove(MilkKit milkKit)
        {
            return Remove(milkKit.Id);
        }

        public MilkKit Update(MilkKit milkKit)
        {
            _db.Update(milkKit);

            return milkKit;
        }

        private bool Remove(int milkKitId)
        {
            return _db.Delete(new MilkKit { Id = milkKitId });
        }
    }
}