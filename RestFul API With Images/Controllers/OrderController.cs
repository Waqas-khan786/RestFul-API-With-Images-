using DataAccessLayer.DbModel;
using DataAccessLayer.IServices;
using DataAccessLayer.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestFul_API_With_Images.Model;

namespace RestFul_API_With_Images.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly IOrders _Orders;

        public OrderController(IOrders Orders)
        {
            _Orders = Orders;
        }

        #region GetOrders
        [HttpGet]
        [Route("GetOrders")]
        public async Task<ActionResult> GetOrders()
        {
            Status status = new();
            try
            {
                List<Orders> getOrders = await _Orders.GetOrders();
                if (getOrders is not null)
                {
                    return Ok(getOrders);
                }
                status.Message = "No Records Found";
                status.Success = false;
                return Ok(status);
            }
            catch (Exception ex)
            {
                status.Message = ex.Message.ToString();
                status.Success = false;
                return Ok(status.Message);
            }
        }
        #endregion

        #region GetOrders_With_Employees
        [HttpGet]
        [Route("GetOrders_With_Employees")]
        public async Task<ActionResult> GetOrders_With_Employees()
        {
            Status status = new();
            try
            {
                IQueryable<OrdersModel> getOrdersWithEmployees = await _Orders.GetOrders_With_Employees();
                if (getOrdersWithEmployees is not null)
                {
                    return Ok(getOrdersWithEmployees);
                }
                status.Message = "No Records Found";
                status.Success = false;
                return Ok(status);
            }
            catch (Exception ex)
            {
                status.Message = ex.Message.ToString();
                status.Success = false;
                return Ok(status.Message);
            }
        }
        #endregion
    }
}