using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DonorTracking.Data
{
    public interface IDinGenerator {
        string Generate();
    }
    public class DinGenerator : IDinGenerator
    {
        private readonly IDbConnection _db;

        public DinGenerator(IRepositoryConfigurationProvider config)
        {
            _db = new SqlConnection(config.ConnectionString);
        }

        public DinGenerator(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public string Generate()
        {
            StringBuilder dinBuilder = new StringBuilder();
            dinBuilder.Append("=W4237");
            dinBuilder.Append(DateTime.Now.ToString("yy"));

            SequenceRecord currentSequenceRecord =
                _db.Query<SequenceRecord>("SELECT * FROM tblSequences WHERE Year = @year",
                    new { Year = DateTime.Now.ToString("yyyy") }).FirstOrDefault() ?? AddNewSequence();
            
            if(currentSequenceRecord.Sequence > 999999) throw new Exception("Sequence for the current year has exceeded 6 digits.  Unable to create new kits.");
            dinBuilder.Append(currentSequenceRecord.Sequence.ToString().PadLeft(6, '0'));
            dinBuilder.Append("00");
            IncrementSequence(currentSequenceRecord);
            return dinBuilder.ToString();
        }

        private void IncrementSequence(SequenceRecord currentSequenceRecord)
        {
            currentSequenceRecord.Sequence++;
            _db.Update(currentSequenceRecord);
        }

        private SequenceRecord AddNewSequence()
        {
            SequenceRecord newSequenceRecord = new SequenceRecord
                {
                    Year = DateTime.Now.ToString("yyyy"),
                    Sequence = 0
                };
            var id = _db.Insert(newSequenceRecord);
            newSequenceRecord.Id = (int) id;

            return newSequenceRecord;
        }
    }
}