using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    [Table("tblSequences")]
    public class SequenceRecord
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public int Sequence { get; set; }
    }
}