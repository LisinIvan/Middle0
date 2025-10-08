using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Middle0.Domain.Common.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Duende.IdentityServer.Services;
using Middle0.Application.Service.Interfaces;

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
			if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
				return Unauthorized();

			var token = _tokenService.GenerateJwtToken(user); // метод, который создаёт JWT-токен

			return Ok(new LoginResponseDto
			{
				Token = token,
				UserName = user.UserName,
				Email = user.Email
			});

			//var user = await _userManager.FindByEmailAsync(dto.Email);
			//if (user == null)
			//	return Unauthorized("User not found");

			//var result = await _signInManager.PasswordSignInAsync(user.UserName, dto.Password, false, false);
			//if (!result.Succeeded)
			//	return Unauthorized("Invalid credentials");

			//// генерируем токен
			//var claims = new List<Claim>
			//{
			//	new Claim(ClaimTypes.Name, user.UserName),
			//	new Claim(ClaimTypes.NameIdentifier, user.Id),
			//	new Claim(ClaimTypes.Email, user.Email)
			//};

			//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			//var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			//var token = new JwtSecurityToken(
			//	issuer: _config["Jwt:Issuer"],
			//	audience: _config["Jwt:Audience"],
			//	claims: claims,
			//	expires: DateTime.UtcNow.AddHours(1),
			//	signingCredentials: creds
			//);

			//var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			//return Ok(new { token = tokenString });
		}
	}

}
