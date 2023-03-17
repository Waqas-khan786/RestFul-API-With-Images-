using DataAccessLayer.DatabaseContext;
using DataAccessLayer.DbModel;
using DataAccessLayer.IServices;
using DataAccessLayer.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace DataAccessLayer.Services
{
    public class OrdersService : IOrders
    {
        private readonly AppDbContext _dbContext;

        public OrdersService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Orders>> GetOrders()
        {
            Task<List<Orders>> getOrderList = null;
            DataTable table = new DataTable();
            //IEnumerable<Orders> getOrderList = null;
            Stopwatch StopWatch = new();            
            string stopwatchResult = string.Empty;
            try
            {
                StopWatch.Start();
                getOrderList = (from orders in _dbContext.Orders
                                select new Orders
                                {
                                    ShipName = orders.ShipName,
                                    ShipAddress = orders.ShipAddress,
                                    ShipCity = orders.ShipCity,
                                    ShipRegion = orders.ShipRegion,
                                    ShipPostalCode = orders.ShipPostalCode,
                                }).ToListAsync();
                StopWatch.Stop();
                stopwatchResult = StopWatch.Elapsed.Seconds.ToString();             
            }
            catch (Exception ex)
            {
                getOrderList = null;
            }
            return await getOrderList;
        }

        //Fetching records of those Employees whose orders were delayed
        public async Task<IQueryable<OrdersModel>> GetOrders_With_Employees()
        {
            //List<OrdersModel> getOrders = null;
            IQueryable<OrdersModel> getOrders = null;
            Stopwatch StopWatch = new();
            string stopwatchResult = string.Empty;
            try
            {
                StopWatch.Start();
                getOrders = (from orders in _dbContext.Orders
                             join employee in _dbContext.Employees on orders.EmployeeID equals employee.EmployeeID
                             where orders.ShippedDate > orders.RequiredDate
                             select new OrdersModel
                             {
                                 EmployeeName = $"{employee.FirstName} {employee.LastName}",
                                 ShipName = orders.ShipName,
                                 ShipAddress = orders.ShipAddress,
                                 ShipCountry = orders.ShipCountry,
                                 OrderDate = orders.OrderDate,
                                 RequiredDate = orders.RequiredDate,
                                 ShippedDate = orders.ShippedDate
                             }).Distinct().AsQueryable();
                StopWatch.Stop();
                stopwatchResult = StopWatch.Elapsed.Seconds.ToString();
            }
            catch (Exception)
            {
                getOrders = null;
            }
            if (getOrders is not null)
            {
                return await Task.FromResult(getOrders);
            }
            return getOrders;
        }
    }
}
