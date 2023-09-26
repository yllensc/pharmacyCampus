using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;
    public interface IEmployee : IGenericRepository<Employee>
    {
        Task<string> UpdateAsync(Employee model);
        Task<IEnumerable<Employee>> EmployeesMoreThanxSales(int numSales);
        Task<IEnumerable<Employee>> EmployeesLessThanxSalesInxYear(int numSales, int year);
        Task<IEnumerable<Employee>> EmployeesWithNoSalesInxYear(int year);
        Task<IEnumerable<Employee>> EmployeesWithNoSalesInMonth(int year, int month);
        Task<Employee> EmployeeWithMostDistinctMedicinesSoldInxYear(int year);
        Task<int> SalesTypeMedicineByEmployee(int id);
        Task<int> SalesByEmployee(int id);
    }