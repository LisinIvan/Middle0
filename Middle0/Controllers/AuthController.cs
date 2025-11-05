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
		private readonly IConfiguration _config;
		private readonly Application.Service.Interfaces.ITokenService _tokenService;

		public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config,
			Application.Service.Interfaces.ITokenService tokenService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_config = config;
			_tokenService = tokenService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			var existingUser = await _userManager.FindByEmailAsync(dto.Email);
			if (existingUser != null)
				return BadRequest(new { message = "A user with this email is already registered." });

			var user = new IdentityUser { UserName = dto.Username, Email = dto.Email };
			var result = await _userManager.CreateAsync(user, dto.Password);

			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return Ok();
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			var user = await _userManager.FindByEmailAsync(dto.Email);
			if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
				return Unauthorized();

			var token = _tokenService.GenerateJwtToken(user); // метод, который создаёт JWT-токен

			return Ok(new LoginResponseDto
			{
				Token = token,
				UserName = user.UserName,
				Email = user.Email
			});
		}
	}

}
