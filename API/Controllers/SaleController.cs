using System.Collections;
using API.Dtos;
using API.Helpers;
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
    public async Task<ActionResult<object>> Get()
    {
        var sales = await _unitOfWork.Sales.GetAllSales();
        return sales;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromBody] SaleDto saleDto)
    {

        var sale = _mapper.Map<Sale>(saleDto);
        var saleMedicine = _mapper.Map<SaleMedicine>(saleDto);

        var result = await _unitOfWork.Sales.RegisterAsync(sale, saleMedicine);

        return Ok(result);
    }

    [HttpPost("range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RegisterManyMedicinesAsync([FromBody] SaleManyPostDto saleManyDto)
    {

        var sale = _mapper.Map<Sale>(saleManyDto);
        var list = _mapper.Map<List<SaleMedicine>>(saleManyDto.MedicinesList);
        var result = await _unitOfWork.Sales.RegisterManyMedicinesAsync(sale, list);

        return Ok(result);
    }

    [HttpGet("recipes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> GetRecipes()
    {
        var sales = await _unitOfWork.Sales.GetAllRecipesAsync();
        return sales;
    }

    [HttpGet("month")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetSaleMonthly([FromQuery] Params parameter)
    {
        var sales = await _unitOfWork.Sales.GetSaleMonthly(parameter.Month);
        return _mapper.Map<List<SaleDto>>(sales);
    }

    [HttpGet("average")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> GetAverage()
    {
        var sales = await _unitOfWork.Sales.GetAverage();
        return Ok(sales);
    }

    [HttpGet("getSaleQuantityAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> GetSaleQuantityAsync()
    {
        var sales = await _unitOfWork.Sales.GetSaleQuantityAsync();
        return Ok(sales);
    }
    // [HttpGet("average")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult<IEnumerable<object>>> GetAverage()
    // {
    //     var sales = await _unitOfWork.Sales.GetAverage();
    //     return Ok(sales);
    // }

    [HttpGet("totalSaleOneMedicine/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTotalSalesOneMedicine(int id)
    {
        var result = await _unitOfWork.Sales.GetTotalSalesOneMedicine(id);
        return  Ok(result);
    }

    
    [HttpGet("gainSales")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> GetGainSales()
    {
        var result = await _unitOfWork.Sales.GetGainSales();
        return  Ok(result);
    }

    [HttpGet("unsoldMedicines2023")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUnsoldMedicines2023()
    {
        var result = await _unitOfWork.Sales.GetUnsoldMedicines2023();
        return  Ok(_mapper.Map<List<MedicinePDto>>(result));
    }

    [HttpGet("unsoldMedicines")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUnsoldMedicine()
    {
        var result = await _unitOfWork.Sales.GetUnsoldMedicine();
        return  Ok(_mapper.Map<List<MedicinePDto>>(result));
    }

    [HttpGet("patientsByMedicine/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPatients(int id)
    {
        var result = await _unitOfWork.Sales.GetPatients(id);
        return  Ok(_mapper.Map<List<PatientOnlyDto>>(result));
    }

    [HttpGet("patientsByMedicine2023/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPatients2023(int id)
    {
        var result = await _unitOfWork.Sales.GetPatients2023(id);
        return  Ok(_mapper.Map<List<PatientOnlyDto>>(result));
    }

    [HttpGet("lessSoldMedicine")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetlessSoldMedicine()
    {
        var result = await _unitOfWork.Sales.GetlessSoldMedicine();
        return  Ok(result);
    }

    [HttpGet("patientTotalSpent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPatientTotalSpent()
    {
        var result = await _unitOfWork.Sales.GetPatientTotalSpent();
        return  Ok(result);
    }

    [HttpGet("totalMedicinesQuarter/{quarter}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetTotalMedicinesQuarter(int quarter)
    {
        var result = await _unitOfWork.Sales.GetTotalMedicinesQuarter(quarter);
        return  Ok(result);
    }

    [HttpGet("patientMoreSpent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPatientMoreSpent()
    {
        var result = await _unitOfWork.Sales.GetPatientMoreSpent();
        return  Ok(result);
    }

    [HttpGet("batchOfMedicines")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetBatchOfMedicines()
    {
        var result = await _unitOfWork.Sales.GetBatchOfMedicines();
        return  Ok(result);
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