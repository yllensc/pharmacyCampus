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
using Microsoft.AspNetCore.Authorization;
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
    [HttpGet("providersWithMedicines")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProviderWithListMedicinesDto>>> GetProvidersWithMedicines()
    {
        var providers = await _unitOfWork.Providers.GetProvidersWithMedicines();
        return _mapper.Map<List<ProviderWithListMedicinesDto>>(providers);
    }
    [HttpGet("providersWithMedicinesUnder{cant}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProviderWithMedicineUnder50>>> GetProvidersWithMedicinesUnderx(int cant)
    {
        var providers = await _unitOfWork.Providers.GetProvidersWithMedicinesUnderx(cant);
        return _mapper.Map<List<ProviderWithMedicineUnder50>>(providers);
    }
    [HttpGet("getProvidersWithCantMedicines")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<ActionResult<IEnumerable<ProviderWithTotalQuantityDto>>> GetProvidersWithTotalPurchasedMedicines()
{
    var providers = await _unitOfWork.Providers.GetCantPurchasedMedicineByProvider();
    var result = new List<ProviderWithTotalQuantityDto>();

    foreach (var provider in providers)
    {
        int totalMedicine = await _unitOfWork.Medicines.CalculateTotalPurchaseQuantity(provider);

        var medicineQuantities = new List<MedicineWithQuantityDto>();

        foreach (var medicine in provider.Medicines)
        {
            int individualCantPurchased = await _unitOfWork.PurchasedMedicines.CalculateMedicineQuantityPurchased(medicine.Id);

            var medicineDto = new MedicineWithQuantityDto
            {
                Name = medicine.Name,
                PurchaseCant = individualCantPurchased
            };

            medicineQuantities.Add(medicineDto);
        }

        var providerWithCant = new ProviderWithTotalQuantityDto
        {
            Name = provider.Name,
            MedicinesList = medicineQuantities,
            TotalPurchaseCant = totalMedicine
        };

        result.Add(providerWithCant);
    }

    return result;
}
[HttpGet("getProvidersWithCantTotalMedicines")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<ActionResult<IEnumerable<ProviderWithTotalQuantityStockDto>>> GetProvidersWithTotalMedicines()
{
    var providers = await _unitOfWork.Providers.GetCantPurchasedMedicineByProvider();
    var result = new List<ProviderWithTotalQuantityStockDto>();

    foreach (var provider in providers)
    {
        int totalMedicine = await _unitOfWork.Medicines.CalculateTotalStockQuantity(provider);

        var medicineQuantities = new List<MedicineWithStockDto>();

        foreach (var medicine in provider.Medicines)
        {
            var medicineDto = new MedicineWithStockDto
            {
                Name = medicine.Name,
                Stock = medicine.Stock
            };

            medicineQuantities.Add(medicineDto);
        }

        var providerWithCant = new ProviderWithTotalQuantityStockDto
        {
            Name = provider.Name,
            MedicinesList = medicineQuantities,
            TotalStockCant = totalMedicine
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

    [HttpGet("onlyProviders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProviderDto>>> Get2()
    {
        var providersAll = await _unitOfWork.Providers.GetAllAsync();
        return _mapper.Map<List<ProviderDto>>(providersAll);
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

    /*[HttpGet("gains")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetGains()
    {     
        var providers = await _unitOfWork.Providers.GetGainsByProviders();
        return Ok(providers);
    }*/

     [HttpGet("moreQuantityMedicines")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetQuantity()
    {     
        var provider = await _unitOfWork.Providers.GetProviderWithMoreMedicines();
        return Ok(provider);
    }

    [HttpGet("totalProviders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTotalProviders2023()
    {     
        var provider = await _unitOfWork.Providers.GetTotalProviders2023();
        return Ok(provider);
    }

    [HttpGet("aleast5medicines")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetProvidersWithDiferentMedicines()
    {     
        var providers = await _unitOfWork.Providers.GetProvidersWithDiferentMedicines();
        return Ok(_mapper.Map<List<ProviderDto>>(providers));
    }

    [HttpGet("gain2023")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetGain()
    {     
        var result = await _unitOfWork.Providers.GetGainByProvider();
        return Ok(result);
    }
    
}
