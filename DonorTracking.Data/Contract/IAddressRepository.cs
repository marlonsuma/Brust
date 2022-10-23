using System.Collections.Generic;

namespace DonorTracking.Data
{
    public interface IAddressRepository
    {
        List<Address> Get(string donorId);
        Address Get(string donorId, AddressType addressType);
        Address Add(Address address);
        Address Update(Address address);
        bool Remove(string donorId, AddressType type);
    }
}