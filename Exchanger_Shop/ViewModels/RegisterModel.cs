using System.ComponentModel.DataAnnotations;

namespace Exchanger_Shop.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email не указан")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль  не указан")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
