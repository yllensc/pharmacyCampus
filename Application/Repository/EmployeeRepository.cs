using System.Globalization;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;


namespace Application.Repository;
public class EmployeeRepository : GenericRepository<Employee>, IEmployee
{
    private readonly PharmacyDbContext _context;

    public EmployeeRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;
    }
    public override async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await this._context.Employees
            .Include(d => d.Position)
            .ToListAsync();
    }
    public async Task<string> UpdateAsync(Employee model)
    {
        var existingEmployee = await _context.Employees.Where(e => e.Id == model.Id).FirstOrDefaultAsync();
        if (existingEmployee == null)
        {
            return "Empleado no encontrado, sorry";
        }
        string strDateTimeNowExact = model.DateContract.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ");
        if (DateTime.TryParseExact(strDateTimeNowExact, "yyyy-MM-ddTHH:mm:ss.ffffffZ", null, DateTimeStyles.None, out DateTime parseDate))
        {
            var existPosition = await _context.Positions.Where(e => e.Id == model.PositionId).FirstOrDefaultAsync();
            if (existPosition != null)
            {
                existingEmployee.Name = model.Name;
                existingEmployee.DateContract = parseDate;
                existingEmployee.PositionId = model.PositionId;
                _context.Employees.Update(existingEmployee);
                await _context.SaveChangesAsync();

                return "employee update successfully";
            }
            else
            {
                return "sorry, this position isn't part in our company";
            }
        }
        else
        {
            return $"La fecha proporcionada no es válida.";
        }
    }
    public async Task<IEnumerable<Employee>> EmployeesMoreThanxSales(int numSales)
    {
        var Employees = await _context.Employees
            .Where(e => e.Sales
                .Where(s => s.SaleMedicines
                    .Any(sm => sm.SaleQuantity > 0))
                .Count() > numSales)
            .ToListAsync();

        return Employees;
    }
    public async Task<IEnumerable<Employee>> EmployeesLessThanxSalesInxYear(int numSales, int year)
    {
        var Employees = await _context.Employees
            .Where(e => e.Sales
                .Where(s => s.DateSale.Year == year)
                .Count() < 5)
            .ToListAsync();

        return Employees;
    }
    public async Task<IEnumerable<Employee>> EmployeesWithNoSalesInxYear(int year)
    {
        var employees = await _context.Employees
            .Where(e => !e.Sales.Any(s => s.DateSale.Year == year))
            .ToListAsync();

        return employees;
    }
public async Task<Employee> EmployeeWithMostDistinctMedicinesSoldInxYear(int year)
{
    var result = await _context.Employees
        .Where(e => e.Sales
            .Where(s => s.DateSale.Year == year)
            .SelectMany(s => s.SaleMedicines)
            .Select(sm => sm.MedicineId)
            .Distinct()
            .Count() > 0)
        .OrderByDescending(e => e.Sales
            .Where(s => s.DateSale.Year == year)
            .SelectMany(s => s.SaleMedicines)
            .Select(sm => sm.MedicineId)
            .Distinct()
            .Count())
        .FirstOrDefaultAsync();

    return result;
}
public async Task<int> SalesTypeMedicineByEmployee(int id)
{
    int numberOfMedicinesSold = await _context.SaleMedicines
            .Where(sm => sm.Sale.EmployeeId == id)
            .Select(sm => sm.MedicineId)
            .Distinct()
            .CountAsync();
    return numberOfMedicinesSold;
}
public async Task<int> SalesByEmployee(int id)
{
    int numberOfMedicinesSold = await _context.SaleMedicines
            .Where(sm => sm.Sale.EmployeeId == id)
            .Select(sm => sm.MedicineId)
            .CountAsync();
    return numberOfMedicinesSold;
}
public async Task<IEnumerable<Employee>> EmployeesWithNoSalesInMonth(int month, int year)
{
    return await _context.Employees
        .Where(e => !e.Sales.Any(s => s.DateSale.Year == year && s.DateSale.Month == month))
        .ToListAsync();
}



}
