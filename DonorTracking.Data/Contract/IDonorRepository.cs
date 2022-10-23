using System.Collections.Generic;

namespace DonorTracking.Data
{
    public interface IDonorRepository
    {
        List<Donor> FindByName(string searchValue);
        List<Donor> Get();
        Donor Get(string donorId);
        Donor GetWithAddresses(string donorId);
        Donor GetWithKits(string donorId);
        Donor Add(Donor donor);
        Donor Update(Donor donor);
        bool Remove(Donor donor);
        bool Remove(string donorId);
        List<Donor> GetDonorWithStatus(bool Status);
    }
}