using DataAccessLayer.DbModel;
using DataAccessLayer.IServices;
using DataAccessLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using RestFul_API_With_Images.Model;

namespace RestFul_API_With_Images.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IUsers _Users;

        public UsersController(IUsers Users)
        {
            _Users = Users;
        }

        #region InsertUsers
        [HttpPost]
        [Route("InsertUsers")]
        public async Task<ActionResult> InsertUsers(Users users)
        {
            string message = string.Empty;
            try
            {
                message = await _Users.InsertUsers(users);
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return Ok(message);
        }
        #endregion


        #region UserLogin
        [HttpPost]
        [Route("UserLogin")]
        public async Task<ActionResult> UserLogin(Users users)
        {
            UserLogin userLogin = new();
            try
            {
                userLogin = await _Users.UserLogin(users);
            }
            catch (Exception)
            {
                userLogin = null;
            }
            return Ok(userLogin);
        }
        #endregion    
    }
}