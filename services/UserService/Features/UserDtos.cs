namespace UserService.Features;
public record UserDto(Guid Id, string Name, string Role, string Email, string Status, string Team, int Score);
public record CreateUserRequest(string Name, string Role, string Email, string Team, string Status, int Score);
public record UpdateUserRequest(string Name, string Role, string Email, string Team, string Status, int Score);
