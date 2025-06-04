namespace EmployeeManager.Services.dtos.accounts;

public class ViewAccountDto
{
    public string Username { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public string FullName { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;
    
    public string HireDate { get; set; } = null!;
    
    public string PassportNumber { get; set; } = null!;
}