using System.ComponentModel.DataAnnotations;
namespace MVCProject2.Models
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Code { get; set; }
        public StatusCodes Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime? ModDate { get; set; }
    }

    public enum StatusCodes
    {
        Pending,
        Processing,
        Completed
    }
}
