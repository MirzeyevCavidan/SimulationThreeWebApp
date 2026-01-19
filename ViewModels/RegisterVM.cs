using System.ComponentModel.DataAnnotations;

namespace SimulationThreeWebApp.ViewModels.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "The name must be entered")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
        [MaxLength(25, ErrorMessage = "The name can be up to 25 characters long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last name must be entered.")]
        [MinLength(3, ErrorMessage = "Last name must be at least 3 characters long.")]
        [MaxLength(25, ErrorMessage = "Last name can be up to 25 characters long")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email must be entered")]
        [EmailAddress(ErrorMessage = "Please enter the correct email format.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password must be entered.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}