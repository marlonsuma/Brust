using System.Collections.Generic;

namespace DonorTracking.Data
{
    public interface IPrinterRepository
    {
        List<Printer> Get();
    }
}