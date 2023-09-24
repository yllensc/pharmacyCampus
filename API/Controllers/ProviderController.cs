using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using API.Dtos;
using API.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProviderController : ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProviderController(IUnitOfWork uniOfWork, IMapper mapper)
    {
        _unitOfWork = uniOfWork;
        _mapper = mapper;
    }

    [HttpGet("getProviders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProviderDto>>> GetProviders()
    {
        var providers = await _unitOfWork.Providers.GetAllAsync();
        return _mapper.Map<List<ProviderDto>>(providers);
    }
    [HttpGet("getProvidersWithMedicines")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProviderWithListMedicinesDto>>> GetProvidersWithMedicines()
    {
        var providers = await _unitOfWork.Providers.GetProvidersWithMedicines();
        return _mapper.Map<List<ProviderWithListMedicinesDto>>(providers);
    }
    [HttpGet("getProvidersWithCantMedicines")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProviderWithTotalQuantityDto>>> GetProvidersWithTotalMedicines()
    {
        var providers = await _unitOfWork.Providers.GetCantMedicineByProvider();
        var result = new List<ProviderWithTotalQuantityDto>();

        foreach (var provider in providers)
        {
            int totalMedicine = await _unitOfWork.Medicines.CalculateTotalQuantity(provider);
            var providerWithCant = new ProviderWithTotalQuantityDto
            {
                Name = provider.Name,
                MedicinesList = provider.Medicines.Select(medicine => new MedicineNameDto
                {
                    Name = medicine.Name
                }).ToList(),
                TotalQuantity = totalMedicine
            };

            result.Add(providerWithCant);
        }

        return result;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProviderxPurchaseDto>>> Get()
    {
        var providersAll = await _unitOfWork.Providers.GetAllAsync();
        return _mapper.Map<List<ProviderxPurchaseDto>>(providersAll);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromBody] ProviderDto providerDto)
    {

        var provider = _mapper.Map<Provider>(providerDto);

        var result = await _unitOfWork.Providers.RegisterAsync(provider);

        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateAsync([FromBody] ProviderPutDto providerPutDto)
    {
        if (providerPutDto == null) { return NotFound(); }
        var provider = _mapper.Map<Provider>(providerPutDto);

        var result = await _unitOfWork.Providers.UpdateAsync(provider);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> Delete(int id)
    {
        var provider = await _unitOfWork.Providers.GetByIdAsync(id);

        if (provider == null) { return NotFound(); }

        this._unitOfWork.Providers.Remove(provider);
        await this._unitOfWork.SaveAsync();
        return NoContent();
    }

}
