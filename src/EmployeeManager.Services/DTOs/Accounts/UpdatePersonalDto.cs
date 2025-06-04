using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Services.dtos.accounts;

public class UpdatePersonalDto
{
    [RegularExpression(@"^[^\d][\w\d_]{2,}$")]
    public string Username { get; set; }
    
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    public string Email { get; set; }
    
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{12,}$")]
    public string Password { get; set; }
    
    public string FirstName { get; set; }
    
    public string? MiddleName { get; set; }
    
    public string LastName { get; set; }
    
    public string PassportNumber { get; set; }
    
    [RegularExpression(@"^\+?[0-9\s\-]{7,15}$")]
    public string PhoneNumber { get; set; }
}