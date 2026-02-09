/**
 * Clean Architecture Framework by Mohammad Hafijur Rahman
 * Implements Mediator, Factory, Unit of Work, and Repository patterns
 * Free to use - please keep this credit
 * https://github.com/mail4hafij/dotnet-clean-architecture-demo
 */

using Core.DB.Mapper;

namespace Core.DB
{
    public interface IMapperFactory
    {
        public ICarMapper CreateCarMapper();
        public IUserMapper CreateUserMapper(ICarMapper carMapper);
    }
}
