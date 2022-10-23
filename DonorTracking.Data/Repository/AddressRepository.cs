using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IDbConnection _db;

        public AddressRepository(IRepositoryConfigurationProvider config)
        {
            _db = new SqlConnection(config.ConnectionString);
        }
        public List<Address> Get(string donorId)
        {
            string sql = "SELECT * FROM tblAddresses " +
                         "WHERE DonorID = @donorId";

            return _db.Query<Address>(sql, new { donorId }).ToList();
        }

        public Address Get(string donorId, AddressType addressType)
        {
            string sql = "SELECT * FROM tblAddresses " +
                         "WHERE DonorID = @DonorId " +
                         "AND AddressType = @AddressType";

            return _db.QuerySingleOrDefault<Address>(sql, new { DonorId = donorId, AddressType = (int)addressType });
        }

        public Address Add(Address address)
        {
            long id = _db.Insert(address);
            address.Id = (int) id;

            return address;
        }

        public Address Update(Address address)
        {
            _db.Update(address);

            return address;
        }

        public bool Remove(string donorId, AddressType addressType)
        {
            string sql = "DELETE FROM tblAddresses " +
                         "WHERE DonorID = @DonorId " +
                         "AND AddressType = @AddressType";
            int rows = _db.Execute(sql, new { DonorId = donorId, AddressType = (int) addressType });

            return rows > 0;
        }
    }
}