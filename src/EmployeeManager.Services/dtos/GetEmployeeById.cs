﻿using EmployeeManager.API;

namespace EmployeeManager.Services.dtos;

public class GetEmployeeById
{
    public PersonDto PersonDto { get; set; }
    public decimal Salary { get; set; }
    public object PositionDto { get; set; }
    public DateTime HireDate { get; set; }
}