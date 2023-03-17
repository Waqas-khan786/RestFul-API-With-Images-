using DataAccessLayer.DbModel;

namespace DataAccessLayer.IServices
{
    public interface IEmployee
    {
        Task<List<Employees>> GetEmployees();
        Task<Employees> GetEmployeesById(int? EmployeeId);
        Task<string> InsertEmployeeBySqlQuery(Employees employee);
        Task<string> InsertEmployee(Employees employee);
        Task<string> DeleteEmployee(int? employee_Id);
        Task<string> UpdateEmployee(Employees employee);
    }
}
