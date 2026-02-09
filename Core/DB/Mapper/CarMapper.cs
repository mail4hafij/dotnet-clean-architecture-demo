using Core.DB.Model;

namespace Core.DB.Mapper
{
    public class CarMapper : ICarMapper
    {
        public Common.Contract.Model.CarContract Map(Car car)
        {
            return new Common.Contract.Model.CarContract
            {
                CarId = car.CarId,
                UserId = car.UserId,
                Nameplate = car.Nameplate
            };
        }
    }
}
