using EmployeeManager.API;

namespace EmployeeManager.Services.dtos;

public class GetEmployeeById
{
    public Person Person { get; set; }
    public decimal Salary { get; set; }
    public object Position { get; set; }
    public DateTime HireDate { get; set; }
}