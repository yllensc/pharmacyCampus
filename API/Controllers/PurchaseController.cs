using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using AutoMapper;

namespace API.Controllers;

public class PurchaseController : ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPurchaseService _purchaseService;
    private readonly IMapper _mapper;
    public PurchaseController(IUnitOfWork unitOfWork,  IPurchaseService purchaseService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _purchaseService = purchaseService;
        _mapper = mapper;
    }

    /*[HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PurchaseMedicineDto>>> Get()
    {
        var purchases = await _unitOfWork.PurchasedMedicines.GetAllAsync();
        return _mapper.Map<List<PurchaseMedicineDto>>(purchases);
    }*/
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PurchaseDto>>> Get()
    {
        var purchases = await _unitOfWork.Purchases.GetAllAsync();
        return _mapper.Map<List<PurchaseDto>>(purchases);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RegisterAsync([FromBody] PurchasePostDto purchasePostDto){

        var result = await _purchaseService.RegisterAsync(purchasePostDto);

        return Ok(result);

    }
    [HttpPost("range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RegisterManyMedicinesAsync([FromBody] PurchaseManyPostDto purchasePostDto){

        var result = await _purchaseService.RegisterManyMedicinesAsync(purchasePostDto);

        return Ok(result);

    }

}
