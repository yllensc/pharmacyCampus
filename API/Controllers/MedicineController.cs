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
    private readonly IProviderService _providerService;
    private readonly IMapper _mapper;

    public MedicineController(IUnitOfWork uniOfWork,IProviderService providerService, IMapper mapper)
    {
        _unitOfWork = uniOfWork;
        _providerService = providerService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<MedicineDto>>> Get()
    {
        var medicines = await _unitOfWork.Medicines.GetAllAsync();
        return _mapper.Map<List<MedicineDto>>(medicines);
    }
    }
