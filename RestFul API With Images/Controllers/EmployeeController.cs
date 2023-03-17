using DataAccessLayer.DbModel;
using DataAccessLayer.Helpers;
using DataAccessLayer.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestFul_API_With_Images.Model;

namespace RestFul_API_With_Images.Controllers
{
    [Authorize]
    [ApiController]
    //[Route("[controller]")]
    [Route("api/Employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employee;

        public EmployeeController(IEmployee employee)
        {
            _employee = employee;
        }

        [HttpGet]
        [Route("GetEmployee")]
        public async Task<ActionResult> GetEmployee()
        {
            string Token = Request.Headers.Authorization.ToString().Split(" ")[1];
            Status status = new();
            try
            {
                var UserName = AuthenticationHelper.ValidateToken(Token);
                List<Employees> getEmployees = await _employee.GetEmployees();
                if (getEmployees is not null)
                {
                    return Ok(getEmployees);
                }
                return Ok(getEmployees);
            }
            catch (Exception ex)
            {
                status.Message = ex.Message.ToString();
                status.Success = false;
                return Ok(status.Message);
            }

        }


        [HttpGet]
        [Route("GetEmployeeById")]
        public async Task<ActionResult> GetEmployeeById(int? EmployeeId)
        {
            Status status = new();
            try
            {
                Employees getEmployee = await _employee.GetEmployeesById(EmployeeId);
                if (getEmployee is not null)
                {
                    return Ok(getEmployee);
                }
                status.Message = "No Record Found";
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

        #region InsertEmployeeBySqlQuery
        [HttpPost]
        [Route("InsertEmployeeBySqlQuery")]
        public async Task<ActionResult> InsertEmployeeBySqlQuery(Employees employee)
        {
            Status status = new();
            try
            {
                string InsertMessage = await _employee.InsertEmployeeBySqlQuery(employee);
                if (InsertMessage is not null)
                {
                    return Ok(InsertMessage);
                }
                status.Message = "No Record Found";
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

        #region InsertEmployee
        [HttpPost]
        [Route("InsertEmployee")]
        public async Task<ActionResult> InsertEmployee(Employees employee)
        {
            Status status = new();
            try
            {
                string InsertMessage = await _employee.InsertEmployee(employee);
                return Ok(InsertMessage);
            }
            catch (Exception ex)
            {
                status.Message = ex.Message.ToString();
                status.Success = false;
                return Ok(status.Message);
            }
        }
        #endregion

        #region DeleteEmployee
        [HttpDelete]
        [Route("DeleteEmployee")]
        public async Task<ActionResult> DeleteEmployee(int? employee_Id)
        {
            Status status = new();
            try
            {
                string DeleteMessage = await _employee.DeleteEmployee(employee_Id);
                return Ok(DeleteMessage);
            }
            catch (Exception ex)
            {
                status.Message = ex.Message.ToString();
                status.Success = false;
                return Ok(status.Message);
            }
        }
        #endregion

        #region UpdateEmployee
        [HttpPut]
        [Route("UpdateEmployee")]
        public async Task<ActionResult> UpdateEmployee(Employees employee)
        {
            Status status = new();
            string UpdateMessage = string.Empty;
            try
            {
                UpdateMessage = await _employee.UpdateEmployee(employee);
                return Ok(UpdateMessage);
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