namespace Api.Endpoints.Customer.Auth;

public record UserAuthDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string Avatar { get; init; }
    public string Phone { get; init; }
    public string Username { get; init; }
    public string Role { get; init; }
};