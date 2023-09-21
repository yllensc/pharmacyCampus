using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.Services;

namespace API.Controllers;

public class PurchaseController : ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPurchaseService _purchaseService;

    public PurchaseController(IUnitOfWork unitOfWork,  IPurchaseService purchaseService)
    {
        _unitOfWork = unitOfWork;
        _purchaseService = purchaseService;
 
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult> RegisterAsync([FromBody] PurchaseMedicineDto purchaseDto){

        var result = await _purchaseService.RegisterAsync(purchaseDto);

        return Ok(result);

    }
}
