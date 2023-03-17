using System.ComponentModel.DataAnnotations;
namespace DataAccessLayer.DbModel
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
