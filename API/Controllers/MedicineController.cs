using API.Dtos;
using API.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
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
    public async Task<ActionResult> RegisterAsync(MedicineDto model)
    {
        var medicine = _mapper.Map<Medicine>(model);
        var result = await _unitOfWork.Medicines.RegisterAsync(medicine);
        return Ok(result);
    }
    [HttpGet("under50")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineDto>>> GetUnder50()
    {
        var medicines = await _unitOfWork.Medicines.GetUnder50();
        return _mapper.Map<List<MedicineDto>>(medicines);
    }
    [HttpGet("ExpiresUnder2024")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineDto>>> GetExpireUnder2024()
    {
        var medicines = await _unitOfWork.Medicines.GetExpireUnder2024();
        return _mapper.Map<List<MedicineDto>>(medicines);
    }
    [HttpGet("ExpiresUntil2024")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineDto>>> GetExpireUntil2024()
    {
        var medicines = await _unitOfWork.Medicines.GetExpireUntil2024();
        return _mapper.Map<List<MedicineDto>>(medicines);
    }
    [HttpGet("moreExpensive")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineDto>>> GetMoreExpensive()
    {
        var medicines = await _unitOfWork.Medicines.GetMoreExpensive();
        return _mapper.Map<List<MedicineDto>>(medicines);
    }
    [HttpGet("getRangePriceStock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineDto>>> GetRangePriceStockPredeterminated()
    {
        var medicines = await _unitOfWork.Medicines.GetRangePriceStockPredeterminated();
        return _mapper.Map<List<MedicineDto>>(medicines);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineDto>>> Get()
    {
        var medicines = await _unitOfWork.Medicines.GetAllAsync();
        return _mapper.Map<List<MedicineDto>>(medicines);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateAsync([FromBody] MedicinePutDto medicineDto)
    {
        if(medicineDto == null){return NotFound();}
        var medicine = _mapper.Map<Medicine>(medicineDto);
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
