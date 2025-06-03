using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Services.dtos;

public class CreateAccountDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Password { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string RoleName { get; set; } = null!;
}