using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using AutoMapper;
using Domain.Entities;

namespace API.Controllers;

public class PurchaseController : ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public PurchaseController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
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

        var purchase = _mapper.Map<Purchase>(purchasePostDto);
        var purchasedMedicine = _mapper.Map<PurchasedMedicine>(purchasePostDto);

        var result = await _unitOfWork.Purchases.RegisterAsync(purchase,purchasedMedicine);

        return Ok(result);

    }
    [HttpPost("range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RegisterManyMedicinesAsync([FromBody] PurchaseManyPostDto purchasePostDto){

        var purchase = _mapper.Map<Purchase>(purchasePostDto);
        var list = _mapper.Map<List<PurchasedMedicine>>(purchasePostDto.MedicinesList);
        var result = await _unitOfWork.Purchases.RegisterManyMedicinesAsync(purchase,list);

        return Ok(result);

    }
}
