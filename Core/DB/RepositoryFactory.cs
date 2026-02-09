/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Core.DB.Repo;

namespace Core.DB
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public ICarRepository CreateCarRepository(IUnitOfWork unitOfWork)
        {
            return new CarRepository(unitOfWork);
        }

        public IUserRepository CreateUserRepository(IUnitOfWork unitOfWork)
        {
            return new UserRepository(unitOfWork);
        }

        public IOrderRepository CreateOrderRepository(IUnitOfWork unitOfWork)
        {
            return new OrderRepository(unitOfWork);
        }

        public IOrderItemRepository CreateOrderItemRepository(IUnitOfWork unitOfWork)
        {
            return new OrderItemRepository(unitOfWork);
        }
    }
}
