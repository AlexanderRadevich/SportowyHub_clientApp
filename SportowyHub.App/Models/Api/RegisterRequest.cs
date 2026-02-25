namespace SportowyHub.Models.Api;

public record RegisterRequest(string Email, string Password, string PasswordConfirm, string? Phone = null);
