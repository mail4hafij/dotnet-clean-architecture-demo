/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Core.DB.Logic;

namespace Core.DB
{
    public interface ILogicFactory
    {
        IUserLogic CreateUserLogic(IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork);

        // SIMPLE LOGIC EXAMPLE: Logic class with minimal dependencies
        // Takes repositoryFactory as parameter (passed from Handler)
        ICarLogic CreateCarLogic(IRepositoryFactory repositoryFactory, IUnitOfWork unitOfWork);

        // COMPLEX LOGIC EXAMPLE: Logic that depends on other logic classes
        // Takes repositoryFactory and carLogic as parameters (passed from Handler)
        IOrderLogic CreateOrderLogic(IRepositoryFactory repositoryFactory, ICarLogic carLogic, IUnitOfWork unitOfWork);
    }
}
