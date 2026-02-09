using AutoMapper;
using Common.Contract.Messaging;
using Common.Contract.Model;
using Core.DB;
using Core.LIB;

namespace Core.Handler.Car
{
    public class GetAllCarHandler : RequestHandler<GetAllCarReq, GetAllCarResp>
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapperFactory _mapperFactory;
        private readonly IMapper _mapper;

        public GetAllCarHandler(IMapper mapper, IMapperFactory mapperFactory, IRepositoryFactory repositoryFactory,
            IUnitOfWorkFactory unitOfWorkFactory, IResponseFactory responseFactory) : base(unitOfWorkFactory, responseFactory)
        {
            _mapper = mapper;
            _mapperFactory = mapperFactory;
            _repositoryFactory = repositoryFactory;
        }

        public override GetAllCarResp Process(GetAllCarReq req)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
                {
                    var userRepo = _repositoryFactory.CreateUserRepository(unitOfWork);
                    var carRepo = _repositoryFactory.CreateCarRepository(unitOfWork);

                    var user = userRepo.GetUser(req.Email);
                    var cars = carRepo.LoadAll(user.UserId, req.QueryParameters);

                    /*
                    var carMapper = _mapperFactory.CreateCarMapper();
                    List<CarContract> carContracts = new List<CarContract>();
                    foreach (var car in cars)
                    {
                        carContracts.Add(carMapper.Map(car));
                    }
                    */

                    // Let's use automapper
                    List<CarContract> carContracts = _mapper.Map<List<CarContract>>(cars);

                    return new GetAllCarResp()
                    {
                        carContracts = carContracts    
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
