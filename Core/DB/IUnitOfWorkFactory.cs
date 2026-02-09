/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

namespace Core.DB
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUnitOfWork(bool useChangeTracking = true);
    }
}
