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



    
    
}
