using API.Dtos;
using API.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class SaleController : ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISaleService _saleService;

    public SaleController(IUnitOfWork unitOfWork, ISaleService saleService, IMapper mapper)
    {
        _saleService = saleService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<SaleDto>>> Get()
    {
        var sales = await _unitOfWork.Sales.GetAllAsync();
        return _mapper.Map<List<SaleDto>>(sales);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SaleDto>> Get(int id)
    {
        var sale = await _unitOfWork.Sales.GetByIdAsync(id);
        return _mapper.Map<SaleDto>(sale);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Sale>> Post(SaleDto saleDto)
    {
        var sale = _mapper.Map<Sale>(saleDto);
        _unitOfWork.Sales.Add(sale);
        await _unitOfWork.SaveAsync();
        if (sale == null)
        {
            return BadRequest();
        }
        saleDto.Id = sale.Id;
        return CreatedAtAction(nameof(Post),new {id = saleDto.Id}, saleDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SaleDto>> Put(int id, [FromBody] SaleDto saleDto)
    {
        if (saleDto == null) return NotFound();
        var sales = _mapper.Map<Sale>(saleDto);
        _unitOfWork.Sales.Update(sales);
        await _unitOfWork.SaveAsync();
        return saleDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var sale = await _unitOfWork.Sales.GetByIdAsync(id);

        if(sale == null) {return NotFound();}

        this._unitOfWork.Sales.Remove(sale);
        await this._unitOfWork.SaveAsync();
        return NoContent();
    }
}