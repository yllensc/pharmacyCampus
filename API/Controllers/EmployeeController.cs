using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Services;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class EmployeeController : ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeService _employees;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeService _employees;
    private readonly IMapper _mapper;

   public EmployeeController(IUnitOfWork uniOfWork, IEmployeeService employees,IMapper mapper)
   public EmployeeController(IUnitOfWork uniOfWork, IEmployeeService employees,IMapper mapper)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<EmployeeGetDto>>> Get()
    {
        var employees = await _unitOfWork.Employees.GetAllAsync();
        return _mapper.Map<List<EmployeeGetDto>>(employees);
    }


    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(LoginDto model)
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
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _userService.RefreshTokenAsync(refreshToken);
        if (!string.IsNullOrEmpty(response.RefreshToken))
            SetRefreshTokenInCookie(response.RefreshToken);
        return Ok(response);
=======
>>>>>>> 81e6bcc (Employee CRUD check y avance de Medicine jaja)
    }


    private void SetRefreshTokenInCookie(string refreshToken)
    {
        _unitOfWork.UserRoles.Remove(userEmployeeRole);
    }
        this._unitOfWork.Employees.Remove(employee);
        await this._unitOfWork.SaveAsync();
        return NoContent();
    }

}
=======
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
        var userEmployeeRole =  _unitOfWork.UserRoles
                                .Find(u => u.UserId == employee.UserId && u.Rol.Name == "employee")
                                .FirstOrDefault();
    if (userEmployeeRole != null)
    {
        _unitOfWork.UserRoles.Remove(userEmployeeRole);
    }
        this._unitOfWork.Employees.Remove(employee);
        await this._unitOfWork.SaveAsync();
        return NoContent();
    }

}
>>>>>>> 81e6bcc (Employee CRUD check y avance de Medicine jaja)
