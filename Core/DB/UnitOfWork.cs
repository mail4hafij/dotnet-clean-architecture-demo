/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Core.DB
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _context;
        private bool _disposed;
        private bool _commitOrRollbackPerformed;

        public DataContext Context
        {
            get
            {
                if (_context == null)
                {
                    throw new Exception("UnitOfWork DataContext is null");
                }
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        public UnitOfWork(IConfiguration configuration)
        {
            _context = new DataContext(configuration);
            _disposed = false;
            _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (_commitOrRollbackPerformed)
            {
                return;
            }

            _context.SaveChanges();
            _context.Database.CommitTransaction();
            _commitOrRollbackPerformed = true;
        }

        public void Rollback()
        {
            if (_commitOrRollbackPerformed)
            {
                return;
            }

            _context.Database.RollbackTransaction();
            _commitOrRollbackPerformed = true;
        }

        public void Flush()
        {
            // Saves changes within the transaction without committing
            // Useful when you need to get generated IDs but keep transaction open
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (!_commitOrRollbackPerformed)
                {
                    Rollback();
                }
                _context.Dispose();
            }
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
