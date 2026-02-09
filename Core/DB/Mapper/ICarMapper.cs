using Core.DB.Model;

namespace Core.DB.Mapper
{
    public interface ICarMapper
    {
        Common.Contract.Model.CarContract Map(Car organization);
    }
}
