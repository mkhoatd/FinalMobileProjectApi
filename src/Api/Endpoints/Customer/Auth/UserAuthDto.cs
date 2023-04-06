namespace Api.Endpoints.Customer.Auth;

public record UserAuthDto
    (Guid Id, string Name, string Email, string Avatar, string Phone, string Username, string Role);