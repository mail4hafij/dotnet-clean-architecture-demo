namespace Core.DB.Logic
{
    /// <summary>
    /// SIMPLE EXAMPLE: Logic class that only depends on repositories
    /// Demonstrates business logic without depending on other logic classes
    /// </summary>
    public class CarLogic : LogicBase, ICarLogic
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public CarLogic(IRepositoryFactory repositoryFactory,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repositoryFactory = repositoryFactory;
        }

        public void AddCarWithValidation(long userId, string nameplate)
        {
            var userRepo = _repositoryFactory.CreateUserRepository(_unitOfWork);
            var carRepo = _repositoryFactory.CreateCarRepository(_unitOfWork);

            // Business Rule 1: User must exist
            var user = userRepo.GetUser(userId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {userId} does not exist");
            }

            // Business Rule 2: Nameplate must be at least 3 characters
            if (string.IsNullOrWhiteSpace(nameplate) || nameplate.Length < 3)
            {
                throw new ArgumentException("Car nameplate must be at least 3 characters long");
            }

            // Business Rule 3: User cannot have duplicate car nameplates
            var existingCars = carRepo.GetAllCarByUser(userId);
            if (existingCars.Any(c => c.Nameplate.Equals(nameplate, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"User already has a car with nameplate '{nameplate}'");
            }

            // All validations passed, add the car
            carRepo.AddCar(userId, nameplate);
        }

        public void TransferCarOwnership(long carId, long fromUserId, long toUserId)
        {
            var userRepo = _repositoryFactory.CreateUserRepository(_unitOfWork);
            var carRepo = _repositoryFactory.CreateCarRepository(_unitOfWork);

            // Business Rule 1: Both users must exist
            var fromUser = userRepo.GetUser(fromUserId);
            if (fromUser == null)
            {
                throw new ArgumentException($"Source user with ID {fromUserId} does not exist");
            }

            var toUser = userRepo.GetUser(toUserId);
            if (toUser == null)
            {
                throw new ArgumentException($"Target user with ID {toUserId} does not exist");
            }

            // Business Rule 2: Car must belong to source user
            var car = carRepo.GetCar(carId);
            if (car == null)
            {
                throw new ArgumentException($"Car with ID {carId} does not exist");
            }

            if (car.UserId != fromUserId)
            {
                throw new InvalidOperationException($"Car does not belong to user {fromUserId}");
            }

            // Transfer ownership
            car.UserId = toUserId;
        }

        public int GetUserCarCount(long userId)
        {
            var carRepo = _repositoryFactory.CreateCarRepository(_unitOfWork);
            var cars = carRepo.GetAllCarByUser(userId);
            return cars.Count();
        }
    }
}
