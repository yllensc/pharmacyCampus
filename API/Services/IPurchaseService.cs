using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Domain.Entities;

namespace API.Services;

public interface IPurchaseService
{
    Task<string> RegisterAsync(PurchaseMedicineDto model);
}
