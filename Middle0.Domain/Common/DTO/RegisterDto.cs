

using System.ComponentModel.DataAnnotations;

namespace Middle0.Domain.Common.DTO
{
    public class RegisterDto
    {
		[Required(ErrorMessage = "Имя пользователя обязательно")]
		[StringLength(20, MinimumLength = 3, ErrorMessage = "Имя пользователя должно быть от 3 до 20 символов")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Email обязателен")]
		[EmailAddress(ErrorMessage = "Введите корректный email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Пароль обязателен")]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
		public string Password { get; set; }
	}
}
