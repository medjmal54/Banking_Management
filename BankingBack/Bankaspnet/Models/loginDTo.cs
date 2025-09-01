using System.ComponentModel.DataAnnotations;

namespace BankingManagement.Models
{
    public class loginDTo
    {
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string Rib { get; set; } = null!;
    }
}
