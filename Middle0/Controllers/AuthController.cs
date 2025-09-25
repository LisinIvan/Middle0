using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Middle0.Domain.Common.DTO;

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
			var existingUser = await _userManager.FindByEmailAsync(dto.Email);
			if (existingUser != null)
				return BadRequest(new { message = "A user with this email is already registered." });

			var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
			var result = await _userManager.CreateAsync(user, dto.Password);

			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return Ok();
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			var user = await _userManager.FindByEmailAsync(dto.Email);
			if (user == null)
				return Unauthorized("User not found");

			var result = await _signInManager.PasswordSignInAsync(user.UserName, dto.Password, false, false);
			if (!result.Succeeded)
				return Unauthorized();

			return Ok();
		}
	}

}
