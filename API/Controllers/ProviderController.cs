using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    private readonly IProviderService _providerService;
    private readonly IMapper _mapper;

    public ProviderController(IUnitOfWork uniOfWork,IProviderService providerService, IMapper mapper)
    {
        _unitOfWork = uniOfWork;
        _providerService = providerService;
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
    public async Task<ActionResult> RegisterAsync([FromBody] ProviderDto providerDto){

        var result = await _providerService.RegisterAsync(providerDto);

        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateAsync([FromBody] ProviderPutDto providerPutDto)
    {
        if(providerPutDto == null){return NotFound();}

        var result = await _providerService.UpdateAsync(providerPutDto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> Delete(int id)
    {
        var provider = await _unitOfWork.Providers.GetByIdAsync(id);

        if(provider == null) {return NotFound();}

        this._unitOfWork.Providers.Remove(provider);
        await this._unitOfWork.SaveAsync();
        return NoContent();
    }

}
