using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    public class TransactionLog : ITransactionLog
    {
        private readonly IDbConnection _db;

        public TransactionLog(IRepositoryConfigurationProvider config)
        {
            _db = new SqlConnection(config.ConnectionString);
        }

        public void Add(
            string type,
            string user,
            string itemType,
            int? itemId,
            Dictionary<string, string> details,
            DateTime? date = null)
        {
            Transaction transaction = new Transaction
                {
                    TransactionType = type,
                    TransactionUser = user,
                    ItemType = itemType,
                    ItemId = itemId,
                    TransactionDate = date ?? DateTime.Now
                };
            Add(transaction, details);
        }

        public void Add(Transaction transaction, Dictionary<string, string> details)
        {
            var id = _db.Insert(transaction);
            List<TransactionDetail> detailList = new List<TransactionDetail>();
            foreach (KeyValuePair<string, string> detail in details)
                detailList.Add(new TransactionDetail((int) id, detail));

            _db.Insert(detailList);
        }
    }
    public static class ItemType
    {
        public static string BloodKit => "Blood Kit";
        public static string MilkKit => "Milk Kit";
        public static string LabKit => "DNA Kit";
        public static string Lot => "Lot";
        public static string Case => "Case";
        public static string Donor => "Donor";
    }

    public static class TransactionType
    {
        public static string CreateBloodKit => "Create Blood Kit";
        public static string ReceiveBloodKit => "Receive Blood Kit";
        public static string CreateMilkKit => "Create Milk Kit";
        public static string ReceiveMilkKit => "Receive Milk Kit";
        public static string QuarantineMilkBag => "Quarantine Milk Bag";
        public static string CreateLabKit => "Create DNA Kit";
        public static string LabResults => "Lab Results";
        public static string CreateLot => "Create Lot";
        public static string TransferLot => "Transfer Lot";
        public static string CreateCase => "Create Case";
        public static string ChangeDonorStatus => "Change Donor Status";
        public static string FormReceived => "Form Received";
        public static string ChangeMilkKit => "Change Milk Kit";
        public static string ChangeLabKit => "Change DNA Kit";
    }
}