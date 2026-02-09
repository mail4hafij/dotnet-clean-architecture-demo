using AutoMapper;
using Common.Contract.Messaging;
using Common.Contract.Model;
using Core.DB;
using Core.LIB;

namespace Core.Handler.User
{
    public class GetAllUserHandler : RequestHandler<GetAllUserReq, GetAllUserResp>
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public GetAllUserHandler(IMapper mapper, IRepositoryFactory repositoryFactory,
            IUnitOfWorkFactory unitOfWorkFactory, IResponseFactory responseFactory) : base(unitOfWorkFactory, responseFactory)
        {
            _mapper = mapper;
            _repositoryFactory = repositoryFactory;
        }

        public override GetAllUserResp Process(GetAllUserReq req)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
                {
                    var userRepo = _repositoryFactory.CreateUserRepository(unitOfWork);

                    var users = userRepo.LoadAll(req.QueryParameters);
                    
                    // Let's use automapper
                    List<UserContract> userContracts = _mapper.Map<List<UserContract>>(users);

                    return new GetAllUserResp()
                    {
                        userContracts = userContracts
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
