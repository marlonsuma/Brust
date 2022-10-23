using System.Collections.Generic;

namespace DonorTracking.Data {
    public interface ICaseRepository {
        Case Get(string barcode);
        List<Case> GetByLot(int lotId);
    }
}