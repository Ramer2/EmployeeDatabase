using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Services.dtos;

public class CreateDeviceDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;
    
    [Required]
    [StringLength(100)]
    public string DeviceType { get; set; } = null!;
    
    [Required]
    public bool IsEnabled { get; set; }
    
    public object? AdditionalProperties { get; set; }
}