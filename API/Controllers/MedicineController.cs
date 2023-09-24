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
    public async Task<ActionResult> RegisterAsync(MedicineAllDto model)
    {
        var medicine = _mapper.Map<Medicine>(model);
        var result = await _unitOfWork.Medicines.RegisterAsync(medicine);
        return Ok(result);
    }
    [HttpGet("under50")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineAllDto>>> GetUnder50()
    {
        var medicines = await _unitOfWork.Medicines.GetUnder50();
        return _mapper.Map<List<MedicineAllDto>>(medicines);
    }
    [HttpGet("ExpiresUnder2024")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineAllDto>>> GetExpireUnder2024()
    {
        var medicines = await _unitOfWork.Medicines.GetExpireUnder2024();
        return _mapper.Map<List<MedicineAllDto>>(medicines);
    }
    [HttpGet("ExpiresUntil2024")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineAllDto>>> GetExpireIn2024()
    {
        var medicines = await _unitOfWork.Medicines.GetExpireIn2024();
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
    [HttpGet("getRangePriceStock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineAllDto>>> GetRangePriceStockPredeterminated()
    {
        var medicines = await _unitOfWork.Medicines.GetRangePriceStockPredeterminated();
        return _mapper.Map<List<MedicineAllDto>>(medicines);
    }
    [HttpGet("GetProvidersInfoWithMedicines")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProviderDto>>> GetProvidersInfoWithMedicines()
    {
        var medicines = await _unitOfWork.Medicines.GetProvidersInfoWithMedicines();
        return _mapper.Map<List<ProviderDto>>(medicines);
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
