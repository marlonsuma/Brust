using System.Collections.Generic;

namespace DonorTracking.Data {
    public interface IBloodKitRepository {
        List<BloodKit> Get();
        BloodKit Get(string din);
        List<BloodKit> GetByDonor(string donorId);
        BloodKit Add(BloodKit bloodKit);
        BloodKit Update(BloodKit bloodKit);
        bool Remove(BloodKit bloodKit);
    }
}