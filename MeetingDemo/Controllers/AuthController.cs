using Microsoft.AspNetCore.Mvc;
using MeetingDemo.Data;
using MeetingDemo.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly ApplicationDbContext _context;

	public AuthController(ApplicationDbContext context)
	{
		_context = context;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] User user)
	{
		if (_context.Users.Any(u => u.Email == user.Email))
		{
			return BadRequest("Email already exists.");
		}

		using (var hmac = new HMACSHA512())
		{
			user.PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(user.PasswordHash)));
		}

		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		return Ok("User registered successfully.");
	}
	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] User loginUser)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Email);

		if (user == null)
		{
			return Unauthorized("Invalid email.");
		}

		using (var hmac = new HMACSHA512())
		{
			var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(loginUser.PasswordHash)));

			if (computedHash != user.PasswordHash)
			{
				return Unauthorized("Invalid password.");
			}
		}

		// Kullanıcı doğrulandı
		return Ok("Login successful.");
	}
}
