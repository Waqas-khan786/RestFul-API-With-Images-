using DataAccessLayer.DbModel;
using DataAccessLayer.ViewModel;

namespace DataAccessLayer.IServices
{
    public interface IUsers
    {
        Task<string> InsertUsers(Users users);
        Task<UserLogin> UserLogin(Users users);
    }
}
