using Common.Contract;
using Core.DB.Model;

namespace Core.DB.Repo
{
    public interface ICarRepository
    {
        void AddCar(long userId, string name);
        List<Car> LoadAll(long userId, QueryParameters qp);
        Car? GetCar(long carId);
        IEnumerable<Car> GetAllCarByUser(long userId);
    }
}
