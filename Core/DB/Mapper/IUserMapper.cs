using Core.DB.Model;

namespace Core.DB.Mapper
{
    public interface IUserMapper
    {
        Common.Contract.Model.UserContract Map(User user);
    }
}
