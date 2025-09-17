using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Middle0.Configuration;

namespace Middle0.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			var user = new IdentityUser { UserName = dto.Username, Email = dto.Email };
			var result = await _userManager.CreateAsync(user, dto.Password);

			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return Ok("User created");
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			var result = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);
			if (!result.Succeeded)
				return Unauthorized();

			return Ok("Login successful");
		}
	}

	public class RegisterDto
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class LoginDto
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
