using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PatientController : ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public PatientController(IUnitOfWork unitOfWork, IUserService userService)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterPatientAsync(RegisterPatientDto model)
    {
        var result = await _userService.RegisterPatientAsync(model);
        return Ok(result);
    }
}
