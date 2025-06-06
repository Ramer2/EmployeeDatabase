﻿namespace EmployeeManager.Services.dtos.devices;

public class ViewDeviceDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DeviceType { get; set; }
    public bool IsEnabled { get; set; }
    public object AdditionalProperties { get; set; }
}