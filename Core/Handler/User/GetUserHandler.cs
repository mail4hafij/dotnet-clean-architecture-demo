using Common.Contract.Messaging;
using Core.DB;
using Core.LIB;

namespace Core.Handler.User
{
    public class GetUserHandler : RequestHandler<GetUserReq, GetUserResp>
    {
        private readonly IMapperFactory _mapperFactory;
        private readonly IRepositoryFactory _repositoryFactory;

        public GetUserHandler(IMapperFactory mapperFactory,
            IRepositoryFactory repositoryFactory,
            IUnitOfWorkFactory unitOfWorkFactory, IResponseFactory responseFactory) : base(unitOfWorkFactory, responseFactory)
        {
            _mapperFactory = mapperFactory;
            _repositoryFactory = repositoryFactory;
        }

        public override GetUserResp Process(GetUserReq req)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
                {
                    var userRepo = _repositoryFactory.CreateUserRepository(unitOfWork);
                    var user = userRepo.GetUser(req.Email);

                    var carMapper = _mapperFactory.CreateCarMapper();
                    var userMapper = _mapperFactory.CreateUserMapper(carMapper);

                    return new GetUserResp()
                    {
                        userContract = userMapper.Map(user)
                    };
                }
            }
            catch (Exception ex)
            {
                return GetErrorResp(ex, "Something went wrong");
            }
        }
    }
}
