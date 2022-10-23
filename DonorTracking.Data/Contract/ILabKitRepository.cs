using System.Collections.Generic;

namespace DonorTracking.Data {
    public interface ILabKitRepository
    {
        LabKit Get(string barcode);
        List<LabKit> GetByDonor(string donorId);
        LabKit GetByMilkKit(int milkKitId);
        LabKit Add(LabKit labKit);
        LabKit Update(LabKit labKit);
    }
}