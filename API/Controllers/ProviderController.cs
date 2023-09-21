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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProviderDto>>> Get()
    {
        var providers = await _unitOfWork.Providers.GetAllAsync();
        return _mapper.Map<List<ProviderDto>>(providers);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult> RegisterAsync([FromBody] ProviderDto providerDto){

        var result = await _providerService.RegisterAsync(providerDto);

        return Ok(result);

    }

}
