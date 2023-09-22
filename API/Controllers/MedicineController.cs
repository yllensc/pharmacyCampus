using API.Dtos;
using API.Services;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
    public class MedicineController: ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMedicineService _medicines;
    private readonly IMapper _mapper;

    public MedicineController(IUnitOfWork uniOfWork,IMedicineService medicines, IMapper mapper)
    {
        _unitOfWork = uniOfWork;
        _medicines = medicines;
        _mapper = mapper;
    }
    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(MedicineDto model)
    {
        var result = await _medicines.RegisterAsync(model);
        return Ok(result);
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

        var result = await _medicines.UpdateAsync(medicineDto);
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
        return NoContent();
    }
    }
