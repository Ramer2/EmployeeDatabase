using EmployeeManager.Services.dtos.employees;

namespace EmployeeManager.Services.dtos.devices;

public class GetDeviceByIdDto
{
    public string Name { get; set; }
    public string DeviceType { get; set; }
    public bool IsEnabled { get; set; }
    public object AdditionalProperties { get; set; }
    public object? CurrentEmployee { get; set; }

    public GetDeviceByIdDto()
    {
    }

    public GetDeviceByIdDto(string name, string deviceType, bool isEnabled, object additionalProperties, GetAllEmployeeDto employee)
    {
        Name = name;
        DeviceType = deviceType;
        IsEnabled = isEnabled;
        AdditionalProperties = additionalProperties;
        CurrentEmployee = employee;
    }
}