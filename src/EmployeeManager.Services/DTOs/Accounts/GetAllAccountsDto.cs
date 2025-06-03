namespace EmployeeManager.Services.dtos.accounts;

public class GetAllAccountsDto
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual string EmployeeFullName { get; set; } = null!;
    
    public virtual string RoleName { get; set; } = null!;
}