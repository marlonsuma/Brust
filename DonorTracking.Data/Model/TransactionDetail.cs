using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    [Table("tblTransactionDetails")]
    public class TransactionDetail
    {
        public TransactionDetail() { }
        public TransactionDetail(int transactionId, KeyValuePair<string, string> detail)
        {
            TransactionId = transactionId;
            Field = detail.Key;
            Value = detail.Value;
        }

        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
    }
}