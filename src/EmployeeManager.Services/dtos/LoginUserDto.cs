using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Services.dtos;

public class LoginUserDto
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}