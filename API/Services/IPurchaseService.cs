using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Domain.Entities;

namespace API.Services;

public interface IPurchaseService
{
<<<<<<< HEAD
    Task<string> RegisterAsync(PurchasePostDto model);
=======
    Task<string> RegisterAsync(PurchaseMedicineDto model);
>>>>>>> eca8963 (feat: :sparkles: Add Purchase, wuuu)
}
