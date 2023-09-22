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
        string prueba = model.DateContract.Date.ToString();
        string prueba2 = model.DateContract.Date.ToLocalTime().ToString();
        if (DateTime.TryParseExact(prueba, "yyyy-MM-dd HH:mm:ss.ffffff", null, System.Globalization.DateTimeStyles.None, out DateTime result))
        {
            var existPosition = await _unitOfWork.Positions.GetByIdAsync(model.PositionId);
            if (existPosition != null){
                var employee = await _unitOfWork.Employees.GetByIdAsync(model.Id);
                employee.Name = model.Name;
                employee.DateContract = result;
                employee.PositionId = model.PositionId;
                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.SaveAsync();

                return "employee update successfully";
            }
            else{
                return "sorry, this position isn't part in our company";
            }
            
        }
        else
        {
            return $"La fecha proporcionada no es válida.";
        }





    }
}