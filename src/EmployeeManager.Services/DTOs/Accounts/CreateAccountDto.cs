using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Services.dtos.accounts;

public class CreateAccountDto
{
    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[^\d][\w\d_]{2,}$")]
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(100)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{12,}$")]
    public string Password { get; set; } = null!;

    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string RoleName { get; set; } = null!;
}