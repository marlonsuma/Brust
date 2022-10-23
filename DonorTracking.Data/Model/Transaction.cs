using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    [Table("tblTransactions")]
    public class Transaction
    {
        public Transaction() { }

        public Transaction(
            string type,
            string user,
            string itemType,
            int? itemId,
            DateTime? date = null)
        {
            TransactionType = type;
            TransactionUser = user;
            ItemType = itemType;
            ItemId = itemId;
            TransactionDate = date ?? DateTime.Now;
        }
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionUser { get; set; }
        public string ItemType { get; set; }
        public int? ItemId { get; set; }

        //public List<TransactionDetail> TransactionDetails { get; set; }
    }
}