using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Models.models;

public class Account
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Password { get; set; } = null!;

    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;
    
    public virtual Employee Employee { get; set; } = null!;
}