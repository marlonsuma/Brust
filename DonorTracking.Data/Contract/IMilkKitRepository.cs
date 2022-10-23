using System.Collections.Generic;

namespace DonorTracking.Data
{
    public interface IMilkKitRepository
    {
        MilkKit Add(MilkKit milkKit);
        List<MilkKit> Get();
        MilkKit Get(string barcode);
        List<MilkKit> GetByDonor(string donorId);
        List<MilkKit> GetByLot(int lotId);
        List<MilkKit> GetWithLot();
        MilkKit GetWithLot(string barcode);
        bool Remove(MilkKit milkKit);
        MilkKit Update(MilkKit milkKit);
    }
}