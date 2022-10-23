using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    public class DonorRepository : IDonorRepository
    {
        private readonly IDbConnection _db;

        public DonorRepository(IRepositoryConfigurationProvider config)
        {
            _db = new SqlConnection(config.ConnectionString);
        }

        public Donor Add(Donor donor)
        {
            long id = _db.Insert(donor);
            donor.Id = (int) id;

            return donor;
        }

        public List<Donor> FindByName(string searchValue)
        {
            string sql = "SELECT * FROM tblDonors D " +
                         "LEFT Join tblAddresses A on A.DonorID = D.DonorID " +
                         "WHERE D.FirstName LIKE @searchValue OR D.LastName like @searchValue";

            var donorDictionary = new Dictionary<int, Donor>();

            List<Donor> data = _db.Query<Donor, Address, Donor>(sql, (d, a) =>
                    {
                        if (!donorDictionary.TryGetValue(d.Id, out Donor donor))
                        {
                            donor = d;
                            donorDictionary.Add(donor.Id, donor);
                        }

                        if (a == null) return donor;

                        if (a.AddressType == AddressType.Mailing)
                            donor.MailingAddress = a;
                        else
                            donor.ShippingAddress = a;

                        return donor;
                    },
                new { SearchValue = $"%{searchValue}%" }).Distinct().ToList();

            return data;
        }

        public List<Donor> Get()
        {
            return _db.GetAll<Donor>().ToList();
        }

        public Donor Get(string donorId)
        {
            var sql = "SELECT * FROM tblDonors " +
                      "WHERE DonorID = @donorId;";

            return _db.QuerySingleOrDefault<Donor>(sql, new { donorId });
        }

        public Donor GetWithAddresses(string donorId)
        {
            string sql = "SELECT * FROM tblDonors D " +
                         "LEFT Join tblAddresses A on A.DonorID = D.DonorID " +
                         "WHERE D.DonorID = @donorId;";
            var donorDictionary = new Dictionary<int, Donor>();

            Donor data = _db.Query<Donor, Address, Donor>(sql, (d, a) =>
                    {
                        if (!donorDictionary.TryGetValue(d.Id, out Donor donor))
                        {
                            donor = d;
                            donorDictionary.Add(donor.Id, donor);
                        }

                        if (a == null) return donor;

                        if (a.AddressType == AddressType.Mailing)
                            donor.MailingAddress = a;
                        else
                            donor.ShippingAddress = a;

                        return donor;
                    },
                new { donorId }).Distinct().SingleOrDefault();

            return data;
        }

        public Donor GetWithKits(string donorId)
        {
            var sql = "SELECT * FROM tblDonors " +
                      "WHERE DonorID = @donorId; " +
                      "SELECT * FROM tblBloodKits " +
                      "WHERE DonorID = @donorId; " +
                      "SELECT M.*, L.Barcode as 'LotBarcode' FROM tblMilkKits M LEFT JOIN tblLots L on L.ID = M.LotID " +
                      "WHERE DonorID = @donorId; " +
                      "SELECT * FROM tblDNAKits " +
                      "WHERE DonorID = @donorId; " +
                      "SELECT * FROM tblAddresses " +
                      "WHERE DonorID = @donorId;";

            using (var multipleResults = _db.QueryMultiple(sql, new { donorId }))
            {
                var donor = multipleResults.Read<Donor>().SingleOrDefault();

                if (donor == null) return null;

                var bloodKits = multipleResults.Read<BloodKit>().ToList();
                var milkKits = multipleResults.Read<MilkKit>().ToList();
                var labKits = multipleResults.Read<LabKit>().ToList();
                var addresses = multipleResults.Read<Address>().ToList();

                if (bloodKits.Any()) donor.BloodKits.AddRange(bloodKits);

                if (milkKits.Any()) donor.MilkKits.AddRange(milkKits);

                if (labKits.Any()) donor.LabKits.AddRange(labKits);

                if (addresses.Any(a => a.AddressType == AddressType.Mailing))
                    donor.MailingAddress = addresses.Single(a => a.AddressType == AddressType.Mailing);

                if (addresses.Any(a => a.AddressType == AddressType.Shipping))
                    donor.ShippingAddress = addresses.Single(a => a.AddressType == AddressType.Shipping);

                return donor;
            }
        }
        public List<Donor> GetDonorWithStatus(bool status)
        {
            
            return _db.GetAll<Donor>().Where(x=>x.Active == status).ToList();

        }

        public bool Remove(Donor donor)
        {
            return Remove(donor.Id);
        }

        public bool Remove(string donorId)
        {
            return _db.Delete(new Donor { DonorId = donorId });
        }

        public Donor Update(Donor donor)
        {
            _db.Update(donor);

            return donor;
        }

        private bool Remove(int id)
        {
            return _db.Delete(new Donor { Id = id });
        }
    }
}