using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PatientController : ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public PatientController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, Employee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PatientDto>>> Get()
    {
        var patients = await _unitOfWork.Patients.GetAllAsync();
        return _mapper.Map<List<PatientDto>>(patients);
    }

    [HttpGet("patientsWithNoSalesIn{year}")]
    [Authorize(Roles = "Administrator, Employee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatientWithNoSalesInxYear(int year)
    {
        var patients = await _unitOfWork.Patients.GetPatientWithNoSalesInxYear(year);
        return _mapper.Map<List<PatientDto>>(patients);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator, Employee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult> Post([FromBody] PatientDto patientDto){
        var patient =  _mapper.Map<Patient>(patientDto);
        var result = await _unitOfWork.Patients.RegisterAsync(patient);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = "Administrator, Employee")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult> Put([FromBody] PatientPutDto patientPutDto)
    {
        if(patientPutDto == null){return NotFound();}
        var patient = _mapper.Map<Patient>(patientPutDto);
        var result = await _unitOfWork.Patients.UpdateAsync(patient);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator, Employee")]
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
