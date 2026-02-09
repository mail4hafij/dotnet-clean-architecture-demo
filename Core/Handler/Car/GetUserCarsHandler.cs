using Common.Contract.Messaging;
using Common.Contract.Model;
using Core.DB;
using Core.LIB;

namespace Core.Handler.Car
{
    /// <summary>
    /// Get all cars for a specific user
    /// </summary>
    public class GetUserCarsHandler : RequestHandler<GetUserCarsReq, GetUserCarsResp>
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public GetUserCarsHandler(
            IRepositoryFactory repositoryFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IResponseFactory responseFactory)
            : base(unitOfWorkFactory, responseFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public override GetUserCarsResp Process(GetUserCarsReq req)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
                {
                    var carRepo = _repositoryFactory.CreateCarRepository(unitOfWork);
                    var cars = carRepo.GetAllCarByUser(req.UserId);

                    var carContracts = cars.Select(c => new CarContract
                    {
                        CarId = c.CarId,
                        UserId = c.UserId,
                        Nameplate = c.Nameplate
                    }).ToList();

                    unitOfWork.Commit();

                    return new GetUserCarsResp
                    {
                        Cars = carContracts,
                        TotalCount = carContracts.Count
                    };
                }
            }
            catch (Exception ex)
            {
                return GetErrorResp(ex, "Failed to get user cars");
            }
        }
    }
}
