using Common.Contract;
using Core.DB.Model;

namespace Core.DB.Repo
{
    public class CarRepository : RepositoryBase, ICarRepository
    {
        public CarRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public void AddCar(long userId, string nameplate)
        {
            if(string.IsNullOrEmpty(nameplate))
            {
                throw new Exception("car nameplate can not be emtpy");
            }

            var org = new Car(userId, nameplate);
            _unitOfWork.Context.Cars.Add(org);
            _unitOfWork.Context.SaveChanges();
        }

        public List<Car> LoadAll(long userId, QueryParameters qp)
        {
            return (from c in _unitOfWork.Context.Cars
                    where c.UserId == userId
                    select c).ApplyQueryParameters(qp).ToList();
        }

        public Car? GetCar(long carId)
        {
            return _unitOfWork.Context.Cars
                .FirstOrDefault(c => c.CarId == carId && !c.Deleted);
        }

        public IEnumerable<Car> GetAllCarByUser(long userId)
        {
            return _unitOfWork.Context.Cars
                .Where(c => c.UserId == userId && !c.Deleted)
                .ToList();
        }
    }
}
