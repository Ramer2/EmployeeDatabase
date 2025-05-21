using System.ComponentModel.DataAnnotations;
using EmployeeManager.API;

namespace EmployeeManager.Services.dtos;

public class CreateDeviceDto
{
    public string Name { get; set; }
    public string? DeviceType { get; set; }
    public bool IsEnabled { get; set; }
    public string AdditionalProperties { get; set; }
}