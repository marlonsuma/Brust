using System;

namespace DonorTracking.Data {
    public interface IRepositoryConfigurationProvider : IDisposable
    {
        string ConnectionString { get; }
    }
    public class RepositoryConfigurationProvider : IRepositoryConfigurationProvider
    {
        private bool _disposed;

        public RepositoryConfigurationProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string ConnectionString { get; }

        public void Dispose()
        {
            ////Dispose(true);
            ////GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            //if (_disposed) return;

            //if (disposing)
            //{
            //}

            //_disposed = true;
        }
    }
}