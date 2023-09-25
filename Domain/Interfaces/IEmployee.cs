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
    }