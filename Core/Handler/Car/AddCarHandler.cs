using Common.Contract.Messaging;
using Core.DB;
using Core.LIB;

namespace Core.Handler.Car
{
    /// <summary>
    /// SIMPLE LOGIC EXAMPLE: Handler using CarLogic
    /// Handler injects factories and passes repositoryFactory to CarLogic
    /// Shows how to use logic for business rules instead of putting them in handlers
    /// </summary>
    public class AddCarHandler : RequestHandler<AddCarReq, AddCarResp>
    {
        private readonly ILogicFactory _logicFactory;
        private readonly IRepositoryFactory _repositoryFactory;

        public AddCarHandler(
            ILogicFactory logicFactory,
            IRepositoryFactory repositoryFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IResponseFactory responseFactory)
            : base(unitOfWorkFactory, responseFactory)
        {
            _logicFactory = logicFactory;
            _repositoryFactory = repositoryFactory;
        }

        public override AddCarResp Process(AddCarReq req)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
                {
                    // Create CarLogic, passing repositoryFactory
                    var carLogic = _logicFactory.CreateCarLogic(_repositoryFactory, unitOfWork);

                    // Logic handles all business rules (validation, duplicate check, etc.)
                    carLogic.AddCarWithValidation(req.UserId, req.Nameplate);

                    unitOfWork.Commit();

                    return new AddCarResp();
                }
            }
            catch (Exception ex)
            {
                return GetErrorResp(ex, "Failed to add car");
            }
        }
    }
}
