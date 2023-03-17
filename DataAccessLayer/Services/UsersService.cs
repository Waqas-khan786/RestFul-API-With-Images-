using DataAccessLayer.DatabaseContext;
using DataAccessLayer.DbModel;
using DataAccessLayer.Helpers;
using DataAccessLayer.IServices;
using DataAccessLayer.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Services
{
    public class UsersService : IUsers
    {
        private readonly AppDbContext _dbContext;

        public UsersService(AppDbContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<string> InsertUsers(Users users)
        {
            string message = string.Empty;
            try
            {
                int result = 0;
                Users Users = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == users.UserName);
                if (Users == null)
                {
                    Users = new()
                    {
                        UserName = users.UserName,
                        FirstName = users.FirstName,
                        LastName = users.LastName,
                        CreateDate = DateTime.Now,
                        Password = Helper.EncryptString(users.Password)
                    };
                    _dbContext.Users.Add(Users);
                    result = await _dbContext.SaveChangesAsync();
                    message = result > 0 ? "User inserted successfully" : "Error while inserting user's data";
                }
                else
                {
                    message = "User name is already exisit try another";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return await Task.FromResult(message);
        }

        public async Task<UserLogin> UserLogin(Users users)
        {
            UserLogin userLogin = new();
            try
            {
                Users UsersData = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == users.UserName);
                if (UsersData is null)
                {
                    return userLogin;
                }
                string DecryptedPassword = Helper.DecryptString(UsersData.Password);
                if (DecryptedPassword != users.Password)
                {
                    return userLogin;
                }
                userLogin.AccessToken = AuthenticationHelper.GenerateToken(UsersData.UserName);
                userLogin.userName = UsersData.UserName;
            }
            catch (Exception ex)
            {
                throw;
            }
            return await Task.FromResult(userLogin);
        }
    }
}
