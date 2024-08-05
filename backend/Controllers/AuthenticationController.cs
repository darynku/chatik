using DarynChat.API.DatabaseContext;
using DarynChat.API.JWT;
using Microsoft.AspNetCore.DataProtection.Internal;
using Microsoft.AspNetCore.Mvc;

namespace DarynChat.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly ChatDbContext _context;
    public AuthenticationController(IJwtService jwtService, ChatDbContext context)
    {
        _jwtService = jwtService;
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthRequest request, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.Password) && string.IsNullOrEmpty(request.UserName))
            throw new ArgumentNullException(nameof(request.Password));
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        var user = UserEntity.Create(Guid.NewGuid(),  request.UserName, passwordHash);
        
        await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);
        
        var token = _jwtService.Generate(user);

        var response = new AuthRespones(token);
        return Ok(response);
    }
}