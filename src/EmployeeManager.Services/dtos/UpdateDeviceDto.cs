using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Services.dtos;

public class UpdateDeviceDto
{
    [Required]
    public string Name { get; set; }
    public string? DeviceType { get; set; }
    public bool IsEnabled { get; set; }
    public string AdditionalProperties { get; set; }
}