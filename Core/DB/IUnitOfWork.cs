/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Microsoft.EntityFrameworkCore.Storage;

namespace Core.DB
{
    public interface IUnitOfWork : IDisposable
    {
        DataContext Context { get; set; }
        void Commit();
        void Rollback();
        void Flush(); // Saves changes without committing transaction
    }
}
