using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Services;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

public class EmployeeController : ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

   public EmployeeController(IUnitOfWork uniOfWork,IMapper mapper)
    {
        _unitOfWork = uniOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<EmployeeGetDto>>> Get()
    {
        var employees = await _unitOfWork.Employees.GetAllAsync();
        return _mapper.Map<List<EmployeeGetDto>>(employees);
    }
    [HttpGet("moreThan{numSales}Sales")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<EmployeeGetIdNameDto>>> EmployeesMoreThanxSales(int numSales)
    {
        var employees = await _unitOfWork.Employees.EmployeesMoreThanxSales(numSales);
        return _mapper.Map<List<EmployeeGetIdNameDto>>(employees);
    }
    [HttpGet("lessThan{numSales}Sales{year}")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<EmployeeGetIdNameDto>>> EmployeesLessThanxSalesInxYear(int numSales, int year)
    {
        var employees = await _unitOfWork.Employees.EmployeesLessThanxSalesInxYear(numSales, year);
        return _mapper.Map<List<EmployeeGetIdNameDto>>(employees);
    }
    [HttpGet("noSalesIn{year}")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<EmployeeGetIdNameDto>>> EmployeesWithNoSalesInxYear(int year)
    {
        var employees = await _unitOfWork.Employees.EmployeesWithNoSalesInxYear(year);
        return _mapper.Map<List<EmployeeGetIdNameDto>>(employees);
    }
    [HttpGet("noSalesInMonth/{month}/InYear/{year}")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<EmployeeGetIdNameDto>>> EmployeesWithNoSalesInMonth(int month,int year)
    {
        var employees = await _unitOfWork.Employees.EmployeesWithNoSalesInMonth(month,year);
        return _mapper.Map<List<EmployeeGetIdNameDto>>(employees);
    }

    [HttpGet("mostDistinctMedicinesSoldIn{year}")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeGetSalesDto>> EmployeeWithMostDistinctMedicinesSoldInxYear(int year)
    {
        var employee = await _unitOfWork.Employees.EmployeeWithMostDistinctMedicinesSoldInxYear(year);
        if (employee != null){
            var sales = await _unitOfWork.Employees.SalesByEmployee(employee.Id);
            var typeMedicine = await _unitOfWork.Employees.SalesTypeMedicineByEmployee(employee.Id);
            var employeeSales = new EmployeeGetSalesDto{
                Id = employee.Id,
                Name = employee.Name,
                CantSales = sales,
                CantTypesMedicines = typeMedicine
            };
            return _mapper.Map<EmployeeGetSalesDto>(employeeSales);
        }
        return _mapper.Map<EmployeeGetSalesDto>(employee);
    }

    [HttpPut]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateAsync([FromBody] EmployeeAllDto EmployeeAllDto)
    {
        if(EmployeeAllDto == null){return NotFound();}
        var employee =  _mapper.Map<Employee>(EmployeeAllDto);
        var result = await _unitOfWork.Employees.UpdateAsync(employee);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);

        if(employee == null) {return NotFound();}
        var userEmployeeRole =  _unitOfWork.UserRoles
                                .Find(u => u.UserId == employee.UserId && u.Rol.Name == "employee")
                                .FirstOrDefault();
    if (userEmployeeRole != null)
    {
        _unitOfWork.UserRoles.Remove(userEmployeeRole);
    }
        this._unitOfWork.Employees.Remove(employee);
        await this._unitOfWork.SaveAsync();
        return Ok($"El empleado {employee.Name} ha sido eliminado correctamente");
    }

}
