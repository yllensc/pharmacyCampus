using API.Dtos;
using API.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
    public class MedicineController: ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MedicineController(IUnitOfWork uniOfWork, IMapper mapper)
    {
        _unitOfWork = uniOfWork;
        _mapper = mapper;
    }
    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(MedicineAllDto model)
    {
        var medicine = _mapper.Map<Medicine>(model);
        var result = await _unitOfWork.Medicines.RegisterAsync(medicine);
        return Ok(result);
    }
    [HttpGet("underStock{cant}")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineAllDto>>> GetUnderCant(int cant)
    {
        var medicines = await _unitOfWork.Medicines.GetUnderCant(cant);
        return _mapper.Map<List<MedicineAllDto>>(medicines);
    }
    [HttpGet("ExpiresUnder{year}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineAllDto>>> GetExpireUnderxYear(int year)
    {
        var medicines = await _unitOfWork.Medicines.GetExpireUnderxYear(year);
        return _mapper.Map<List<MedicineAllDto>>(medicines);
    }
    [HttpGet("ExpiresIn{year}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineAllDto>>> GetExpireInxYear(int year)
    {
        var medicines = await _unitOfWork.Medicines.GetExpireInxYear(year);
        return _mapper.Map<List<MedicineAllDto>>(medicines);
    }
    [HttpGet("moreExpensive")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineAllDto>>> GetMoreExpensive()
    {
        var medicines = await _unitOfWork.Medicines.GetMoreExpensive();
        return _mapper.Map<List<MedicineAllDto>>(medicines);
    }
    [HttpGet("getRangePrice{price}Stock{stock}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineAllDto>>> GetRangePriceStockPredeterminated(double price, int stock)
    {
        var medicines = await _unitOfWork.Medicines.GetRangePriceStockPredeterminated(price,stock);
        return _mapper.Map<List<MedicineAllDto>>(medicines);
    }
    [HttpGet("GetProvidersInfoWithMedicines")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProviderWithListMedicinesDto>>> GetProvidersInfoWithMedicines()
    {
        var medicines = await _unitOfWork.Providers.GetCantMedicineByProvider();
        return _mapper.Map<List<ProviderWithListMedicinesDto>>(medicines);
    }
    [HttpGet("salesIn{year}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Dictionary<string, List<object>>>> GetMedicineSoldonYear(int year)
    {
        var medicines = await _unitOfWork.Medicines.GetMedicineSoldonYear(year);
        return medicines;
    }
    

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineAllDto>>> Get()
    {
        var medicines = await _unitOfWork.Medicines.GetAllAsync();
        return _mapper.Map<List<MedicineAllDto>>(medicines);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateAsync([FromBody] MedicinePutDto MedicineAllDto)
    {
        if(MedicineAllDto == null){return NotFound();}
        var medicine = _mapper.Map<Medicine>(MedicineAllDto);
        var result = await _unitOfWork.Medicines.UpdateAsync(medicine);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> Delete(int id)
    {
        var medicine = await _unitOfWork.Medicines.GetByIdAsync(id);

        if(medicine == null) {return NotFound();}
        this._unitOfWork.Medicines.Remove(medicine);
        await this._unitOfWork.SaveAsync();
        return Ok($"El medicamento {medicine.Name} ha sido eliminado correctamente");
    }
    }
