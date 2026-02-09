/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Core.DB.Repo;

namespace Core.DB
{
    public interface IRepositoryFactory
    {
        ICarRepository CreateCarRepository(IUnitOfWork unitOfWork);
        IUserRepository CreateUserRepository(IUnitOfWork unitOfWork);
        IOrderRepository CreateOrderRepository(IUnitOfWork unitOfWork);
        IOrderItemRepository CreateOrderItemRepository(IUnitOfWork unitOfWork);
    }
}
