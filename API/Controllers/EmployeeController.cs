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
    private readonly IUserService _userService;

    public EmployeeController(IUserService userService)
    {
        _userService = userService;
    }
<<<<<<< HEAD


=======
        [HttpPost("register")]
    public async Task<ActionResult> RegisterEmployeeAsync(RegisterEmployeeDto model)
    {
        var result = await _userService.RegisterEmployeeAsync(model);
        return Ok(result);
    }
>>>>>>> aff8a9d (interfaces yllen)

    
    
}
