namespace EmployeeManager.Services.dtos.accounts;

public class UpdatePersonalDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string PassportNumber { get; set; }
    public string PhoneNumber { get; set; }
}