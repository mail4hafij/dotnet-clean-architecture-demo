using Common.Contract;
using Core.DB.Model;

namespace Core.DB.Repo
{
    public interface IUserRepository
    {
        User GetUser(long userId);
        User GetUser(string email);
        Task AddUser(string email, string password);
        bool Exists(string email);
        List<User> LoadAll(QueryParameters qp);
    }
}
