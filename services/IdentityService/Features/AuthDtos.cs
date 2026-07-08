namespace IdentityService.Features;
public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Name, string Email, string Password);
public record AuthUserDto(Guid Id, string Name, string Email, string RoleName);
public record AuthResponse(string Token, AuthUserDto User);
