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

namespace API.Controllers;

public class EmployeeController : ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeService _employees;
    private readonly IMapper _mapper;

   public EmployeeController(IUnitOfWork uniOfWork, IEmployeeService employees,IMapper mapper)
    {
        _unitOfWork = uniOfWork;
        _employees = employees;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<EmployeeGetDto>>> Get()
    {
        var employees = await _unitOfWork.Employees.GetAllAsync();
        return _mapper.Map<List<EmployeeGetDto>>(employees);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateAsync([FromBody] EmployeeDto employeeDto)
    {
        if(employeeDto == null){return NotFound();}

        var result = await _employees.UpdateAsync(employeeDto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);

        if(employee == null) {return NotFound();}

        this._unitOfWork.Employees.Remove(employee);
        await this._unitOfWork.SaveAsync();
        return NoContent();
    }

}