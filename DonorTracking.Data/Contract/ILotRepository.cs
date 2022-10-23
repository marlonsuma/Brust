namespace DonorTracking.Data {
    public interface ILotRepository {
        Lot Get(string barcode);
    }
}