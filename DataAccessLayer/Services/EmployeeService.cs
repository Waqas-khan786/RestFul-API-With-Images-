using DataAccessLayer.DatabaseContext;
using DataAccessLayer.DbModel;
using DataAccessLayer.Helpers;
using DataAccessLayer.IServices;
using Microsoft.EntityFrameworkCore;
namespace DataAccessLayer.Services
{
    public class EmployeeService : IEmployee
    {
        private readonly AppDbContext _dbContext;

        public EmployeeService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Employees>> GetEmployees()
        {
            List<Employees> getEmployeesList = new();

            try
            {
                //var _getEmployeesList = await _dbContext.Employees.ToListAsync();
                getEmployeesList = (from employee in _dbContext.Employees.ToList()
                                    select new Employees
                                    {
                                        FirstName = employee.FirstName,
                                        LastName = employee.LastName,
                                        BirthDate = employee.BirthDate,
                                        HireDate = employee.HireDate,
                                        Address = employee.Address,
                                    }).ToList();
            }
            catch (Exception ex)
            {
                getEmployeesList = null;
            }
            return await Task.FromResult(getEmployeesList);
        }

        public async Task<Employees> GetEmployeesById(int? EmployeeId)
        {
            Employees employee = null;
            try
            {
                employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeID == EmployeeId);
            }
            catch (Exception ex)
            {
                employee = null;
            }
            return employee;
        }

        public async Task<string> InsertEmployee(Employees employee)
        {
            string message = string.Empty;
            int result = 0;
            try
            {
                var address = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Address.Contains(employee.Address));
                if (address is null)
                {
                    string FilePath = await Helper.UploadImage(employee.Photo,"TestImage2",".png");
                    employee.PhotoPath = FilePath;
                    await _dbContext.Employees.AddAsync(employee);
                    result = await _dbContext.SaveChangesAsync();
                    message = result > 0 ? "record inserted successfully" : "Error while inserting record";
                }
                else
                {
                    message = "Address Already Exist";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString().ToUpper();
            }
            return message;
        }

        public async Task<string> InsertEmployeeBySqlQuery(Employees employee)
        {
            string message;
            try
            {
                int result = await _dbContext.Database.ExecuteSqlAsync(
               $"Insert Into Employees (FirstName,LastName,BirthDate,HireDate) Values ({employee.FirstName},{employee.LastName},{employee.BirthDate},{employee.HireDate})");
                message = result > 0 ? "record inserted successfully" : "Error while inserting record";
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString().ToUpper();
            }
            return message;
        }

        public async Task<string> DeleteEmployee(int? employee_Id)
        {
            string message;
            int result = 0;
            try
            {
                Employees _employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeID == employee_Id);
                if (_employee != null)
                {
                    _dbContext.Employees.Remove(_employee);
                    result = await _dbContext.SaveChangesAsync();
                    message = result > 0 ? "records Deleted successfully" : "Error while Deleting records";
                }
                else
                {
                    message = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString().ToUpper();
            }
            return message;
        }

        public async Task<string> UpdateEmployee(Employees employee)
        {
            string message;
            int? result = 0;
            try
            {
                Employees _employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeID == employee.EmployeeID);
                if (_employee != null)
                {
                    _employee.FirstName = employee.FirstName is not null ? employee.FirstName : _employee.FirstName;
                    _employee.LastName = employee.LastName is not null ? employee.LastName : _employee.LastName;
                    _employee.Title = employee.Title is not null ? employee.Title : _employee.Title;
                    _employee.Address = employee.Address is not null ? employee.Address : _employee.Address;
                    _employee.City = employee.City is not null ? employee.City : _employee.City;
                    _employee.Country = employee.Country is not null ? employee.Country : _employee.Country;
                    _dbContext.Employees.Update(_employee);
                    result = await _dbContext.SaveChangesAsync();
                    message = result > 0 ? "records Updated successfully" : "Error while Updating records";
                }
                else
                {
                    message = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString().ToUpper();
            }
            return message;
        }      
    }
}
