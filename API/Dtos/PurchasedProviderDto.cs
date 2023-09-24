using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace API.Dtos;

public class PurchasedProviderDto
{
    public string Name { get; set; }

    public List<MedicinesProviderDto> medicines { get; set; }
}

public class MedicinesProviderDto
{
    public string Name { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }
}




