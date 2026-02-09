/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Core.DB.Logic;

namespace Core.DB
{
    /// <summary>
    /// LogicFactory creates Logic instances by passing dependencies as parameters
    /// NO state, NO injection - just a factory that creates instances
    /// </summary>
    public class LogicFactory : ILogicFactory
    {
        public IUserLogic CreateUserLogic(IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork)
        {
            return new UserLogic(repositoryFactory, unitOfWork);
        }

        // SIMPLE LOGIC EXAMPLE: Takes repositoryFactory from Handler
        public ICarLogic CreateCarLogic(IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork)
        {
            return new CarLogic(repositoryFactory, unitOfWork);
        }

        // COMPLEX LOGIC EXAMPLE: Takes repositoryFactory and carLogic from Handler
        // Handler creates CarLogic first, then passes it to OrderLogic
        public IOrderLogic CreateOrderLogic(IRepositoryFactory repositoryFactory, ICarLogic carLogic, IUnitOfWork unitOfWork)
        {
            return new OrderLogic(repositoryFactory, carLogic, unitOfWork);
        }
    }
}
