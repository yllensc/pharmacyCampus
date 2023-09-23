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

    public SaleController(IUnitOfWork unitOfWork, IMapper mapper)
    {
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

    
// [HttpPost]
// [ProducesResponseType(StatusCodes.Status201Created)]
// [ProducesResponseType(StatusCodes.Status400BadRequest)]
// public async Task<ActionResult<Sale>> Post([FromBody] SaleDto saleDto)
// {
//     if (saleDto == null)
// {
//     // Manejo de error o respuesta 400 Bad Request
//     return BadRequest("El objeto SaleDto es nulo.");
// }
    
//     var sale = _mapper.Map<Sale>(saleDto);
//     var saleMedicine = _mapper.Map<SaleMedicine>(saleDto);


//     // Agrega el SaleMedicine a la colección de SaleMedicines en la entidad Sale
//     _unitOfWork.SaleMedicines.Add(saleMedicine);
//     _unitOfWork.Sales.Add(sale);
    
//     try
//     {
//         await _unitOfWork.SaveAsync();
//         return Ok("Venta creada con éxito");
//     }
//     catch (Exception ex)
//     {
//         // Loguea la excepción interna para obtener más información
//         Console.WriteLine(ex.InnerException);
//         throw; // Re-lanza la excepción para que sea manejada globalmente o muestre más detalles en la respuesta HTTP
//     }
// }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromBody] SaleDto saleDto){

        var sale = _mapper.Map<Sale>(saleDto);
        var saleMedicine = _mapper.Map<SaleMedicine>(saleDto);

        var result = await _unitOfWork.Sales.RegisterAsync(sale,saleMedicine);

        return Ok(result);

    }
    [HttpPost("range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RegisterManyMedicinesAsync([FromBody] SaleManyPostDto saleManyDto){

        var sale = _mapper.Map<Sale>(saleManyDto);
        var list = _mapper.Map<List<SaleMedicine>>(saleManyDto.MedicinesList);
        var result = await _unitOfWork.Sales.RegisterManyMedicinesAsync(sale,list);

        return Ok(result);

    }
}


    

    // [HttpPut("{id}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult<SaleDto>> Put(int id, [FromBody] SaleDto saleDto)
    // {
    //     if (saleDto == null) return NotFound();
    //     var sales = _mapper.Map<Sale>(saleDto);
    //     _unitOfWork.Sales.Update(sales);
    //     await _unitOfWork.SaveAsync();
    //     return saleDto;
    // }

    // [HttpDelete("{id}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> Delete(int id)
    // {
    //     var sale = await _unitOfWork.Sales.GetByIdAsync(id);

    //     if(sale == null) {return NotFound();}

    //     this._unitOfWork.Sales.Remove(sale);
    //     await this._unitOfWork.SaveAsync();
    //     return NoContent();
    // }
// }