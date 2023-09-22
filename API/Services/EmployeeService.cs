using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Domain.Interfaces;

namespace API.Services;
public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    public EmployeeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<string> UpdateAsync(EmployeeDto model)
    {
        var existingEmployee = await _unitOfWork.Employees.GetByIdAsync(model.Id);
        if (existingEmployee == null)
        {
            return "Empleado no encontrado";
        }
        string strDateTimeNowExact = model.DateContract.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ");
        if (DateTime.TryParseExact(strDateTimeNowExact, "yyyy-MM-ddTHH:mm:ss.ffffffZ", null, DateTimeStyles.None, out DateTime parseDate))
        {
            var existPosition = await _unitOfWork.Positions.GetByIdAsync(model.PositionId);
            if (existPosition != null)
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(model.Id);
                employee.Name = model.Name;
                employee.DateContract = parseDate;
                employee.PositionId = model.PositionId;
                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.SaveAsync();

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
}