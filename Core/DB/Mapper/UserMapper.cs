using Common.Contract.Model;
using Core.DB.Model;

namespace Core.DB.Mapper
{
    public class UserMapper : IUserMapper
    {
        private readonly ICarMapper _carMapper;

        public UserMapper(ICarMapper carMapper)
        {
            _carMapper = carMapper;
        }

        public UserContract Map(User user)
        {
            return new UserContract
            {
                UserId = user.UserId,
                Email = user.Email,
                CarContracts = MapCars(user.Cars),
            };
        }

        private List<CarContract> MapCars(List<Car> cars)
        {
            List<CarContract> result = new List<CarContract>();

            foreach (Car car in cars)
            {
                result.Add(_carMapper.Map(car));
            }

            return result;
        }
    }
}
