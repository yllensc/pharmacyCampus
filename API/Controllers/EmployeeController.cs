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

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(LoginDto model)
    {
        var result = await _userService.GetTokenAsync(model);
        SetRefreshTokenInCookie(result.RefreshToken);
        return Ok(result);
    }

    [HttpPost("addrole")]
    public async Task<IActionResult> AddRoleAsync(AddRoleDto model)
    {
        var result = await _userService.AddRoleAsync(model);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _userService.RefreshTokenAsync(refreshToken);
        if (!string.IsNullOrEmpty(response.RefreshToken))
            SetRefreshTokenInCookie(response.RefreshToken);
        return Ok(response);
    }


    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddMinutes(2),
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
    
}
