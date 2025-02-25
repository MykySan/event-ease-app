using System.ComponentModel.DataAnnotations;

namespace EventEaseApp.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string UserEmail { get; set; } = string.Empty;
        [Required]
        public int EventId { get; set; }
    }
}