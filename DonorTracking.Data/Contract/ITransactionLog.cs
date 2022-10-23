using System;
using System.Collections.Generic;

namespace DonorTracking.Data {
    public interface ITransactionLog
    {
        void Add(
            string type,
            string user,
            string itemType,
            int? itemId,
            Dictionary<string, string> details,
            DateTime? date = null);

        void Add(Transaction transaction, Dictionary<string, string> details);
    }
}