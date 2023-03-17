using DataAccessLayer.DbModel;
using DataAccessLayer.ViewModel;

namespace DataAccessLayer.IServices
{
    public interface IOrders
    {
        Task<List<Orders>> GetOrders();
        Task<IQueryable<OrdersModel>> GetOrders_With_Employees();
    }
}
