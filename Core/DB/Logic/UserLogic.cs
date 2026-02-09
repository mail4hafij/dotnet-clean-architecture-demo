
namespace Core.DB.Logic
{
    public class UserLogic : LogicBase, IUserLogic
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public UserLogic(IRepositoryFactory repositoryFactory,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repositoryFactory = repositoryFactory;
        }
    }
}
