namespace Api.Endpoints.Customer.Auth;

public record UserAuthDto
    (int Id, string Name, string Email, string Avatar, string Phone, string Username, string Role);