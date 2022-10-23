using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    public class BloodKitRepository : IBloodKitRepository
    {
        private readonly IDbConnection _db;
        public BloodKitRepository(IRepositoryConfigurationProvider config)
        {
            _db = new SqlConnection(config.ConnectionString);
        }

        public BloodKitRepository(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }
        public List<BloodKit> Get()
        {
            return _db.GetAll<BloodKit>().ToList();
        }

        public BloodKit Get(string din)
        {
            return _db.Query<BloodKit>("SELECT * FROM tblBloodKits WHERE DIN = @din", new { din }).SingleOrDefault();
        }

        public List<BloodKit> GetByDonor(string donorId)
        {
            return _db.Query<BloodKit>("SELECT * FROM tblBloodKits WHERE DonorID = @DonorId", new { donorId }).ToList();
        }

        public BloodKit Add(BloodKit bloodKit)
        {
            long id = _db.Insert(bloodKit);
            bloodKit.Id = (int)id;

            return bloodKit;
        }

        public BloodKit Update(BloodKit bloodKit)
        {
            _db.Update(bloodKit);
            return bloodKit;
        }

        public bool Remove(BloodKit bloodKit)
        {
            return Remove(bloodKit.Id);
        }

        private bool Remove(int id)
        {
            return _db.Delete(new BloodKit { Id = id });
        }
    }
}