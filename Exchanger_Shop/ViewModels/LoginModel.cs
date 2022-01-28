using System.ComponentModel.DataAnnotations;

namespace Exchanger_Shop.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль не указан")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
