using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    public class LabKitRepository : ILabKitRepository
    {
        private readonly IDbConnection _db;

        public LabKitRepository(IRepositoryConfigurationProvider config)
        {
            _db = new SqlConnection(config.ConnectionString);
        }

        public LabKit Add(LabKit labKit)
        {
            long id = _db.Insert(labKit);
            labKit.Id = (int) id;
            labKit.Barcode = "DK" + ((int) id).ToString().PadLeft(7, '0');
            _db.Update(labKit);

            return labKit;
        }

        public LabKit Get(string barcode)
        {
            return _db.Query<LabKit>("SELECT * FROM tblDNAKits WHERE Barcode = @barcode", new { barcode })
                      .SingleOrDefault();
        }

        public List<LabKit> GetByDonor(string donorId)
        {
            return _db.Query<LabKit>("SELECT * FROM tblDNAKits WHERE DonorID = @donorId", new { donorId }).ToList();
        }

        public LabKit GetByMilkKit(int milkKitId)
        {
            return _db.QueryFirstOrDefault<LabKit>("SELECT * FROM tblDNAKits WHERE MilkKitID = @milkKitId",
                new { milkKitId });
        }

        public LabKit Update(LabKit labKit)
        {
            _db.Update(labKit);

            return labKit;
        }
    }
}