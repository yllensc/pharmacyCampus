using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;

public interface ISale : IGenericRepository<Sale>
{
    Task<string> RegisterAsync(Sale modelSale, SaleMedicine modelSaleMedicine);
    Task<string> RegisterManyMedicinesAsync(Sale modelSale , List<SaleMedicine> list);
}
