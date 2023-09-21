using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Services;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PatientController : ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPatientService _patientService;
    private readonly IMapper _mapper;


    public PatientController(IUnitOfWork unitOfWork, IPatientService patientService, IMapper mapper)
    {
        _patientService = patientService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PatientDto>>> Get()
    {
        var patients = await _unitOfWork.Patients.GetAllAsync();
        return _mapper.Map<List<PatientDto>>(patients);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult> RegisterAsync([FromBody] PatientDto patientDto){

        var result = await _patientService.RegisterAsync(patientDto);

        return Ok(result);

    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult> UpdateAsync([FromBody] PatientPutDto patientPutDto)
    {
        if(patientPutDto == null){return NotFound();}

        var result = await _patientService.UpdateAsync(patientPutDto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> Delete(int id)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(id);

        if(patient == null) {return NotFound();}

        this._unitOfWork.Patients.Remove(patient);
        await this._unitOfWork.SaveAsync();
        return NoContent();
    }
}
