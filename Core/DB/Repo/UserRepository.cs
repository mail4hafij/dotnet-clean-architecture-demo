using Common.Contract;
using Core.DB.Model;
using Microsoft.EntityFrameworkCore;

namespace Core.DB.Repo
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task AddUser(string email, string password)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new Exception("email or password can not be emtpy");
            }

            if(Exists(email))
            {
                throw new Exception("email already exists");
            }

            var user = new User(email, password);
            await _unitOfWork.Context.Users.AddAsync(user);
            _unitOfWork.Context.SaveChanges();
        }

        public User GetUser(long userId)
        {
            return (from user in _unitOfWork.Context.Users.
                    Include(o => o.Cars)
                    where user.UserId == userId
                    select user).Single();
        }

        public User GetUser(string email)
        {
            return (from user in _unitOfWork.Context.Users.
                    Include(o => o.Cars)
                    where user.Email == email
                    select user).Single();
        }

        public bool Exists(string email)
        {
            return _unitOfWork.Context.Users.Any(u => u.Email == email);
        }

        public List<User> LoadAll(QueryParameters qp)
        {
            return (from user in _unitOfWork.Context.Users select user).ApplyQueryParameters(qp).ToList();
        }
    }
}
