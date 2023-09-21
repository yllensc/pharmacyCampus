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
        if (DateTime.TryParseExact(model.DateContract.ToString(), "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime parsedDate))
        {

            var existPosition = await _unitOfWork.Positions.GetByIdAsync(model.PositionId);
            if (existPosition != null){
                var employee = await _unitOfWork.Employees.GetByIdAsync(model.Id);
                employee.Name = model.Name;
                employee.DateContract = model.DateContract;
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