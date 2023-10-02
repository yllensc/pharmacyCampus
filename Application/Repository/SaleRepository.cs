using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;

public class SaleRepository : GenericRepository<Sale>, ISale
{
    private readonly PharmacyDbContext _context;

    public SaleRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<string> RegisterAsync(Sale modelSale, SaleMedicine modelSaleMedicine)
    {
        var newSale = new Sale
        {
            DateSale = DateTime.UtcNow,
            PatientId = modelSale.PatientId,
            EmployeeId = modelSale.EmployeeId,
            Prescription = modelSale.Prescription
        };
        try
        {
            _context.Sales.Add(newSale);
            await _context.SaveChangesAsync();
            var saleCreated = await _context.Sales
                                    .Where(u => u.Id == newSale.Id)
                                    .FirstOrDefaultAsync();
            var medicine = await _context.Medicines
                                        .Where(u => u.Id == modelSaleMedicine.MedicineId)
                                        .FirstOrDefaultAsync();
            Console.WriteLine(medicine);
            var newSaleMedicine = new SaleMedicine
            {
                SaleId = saleCreated.Id,
                MedicineId = modelSaleMedicine.MedicineId,
                SaleQuantity = modelSaleMedicine.SaleQuantity,
                Price = medicine.Price * modelSaleMedicine.SaleQuantity
            };
            try
            {
                if (medicine.Stock >= newSaleMedicine.SaleQuantity)
                {
                    int quantity = modelSaleMedicine.SaleQuantity;
                    while (quantity != 0)
                    {
                        var nearestExpirationDate = await _context.PurchasedMedicines
                                                    .Where(u => u.MedicineId == modelSaleMedicine.MedicineId && u.Stock > 0)
                                                    .OrderBy(o => o.ExpirationDate).FirstAsync();
                        if (nearestExpirationDate == null)
                        {
                            quantity = 0;
                        }
                        else
                        {
                            var purchasedMedicine = await _context.PurchasedMedicines.Where(u => u.Id == nearestExpirationDate.Id).FirstOrDefaultAsync();
                            if (nearestExpirationDate.Stock >= quantity)
                            {
                                //Stock Lote por fecha de vencimiento
                                purchasedMedicine.Stock -= quantity;
                                _context.PurchasedMedicines.Update(purchasedMedicine);
                                await _context.SaveChangesAsync();
                                quantity = 0;
                            }
                            else
                            {
                                quantity -= purchasedMedicine.Stock;
                                purchasedMedicine.Stock = 0;
                                _context.PurchasedMedicines.Update(purchasedMedicine);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    medicine.Stock -= modelSaleMedicine.SaleQuantity;
                    _context.SaleMedicines.Add(newSaleMedicine);
                    _context.Medicines.Update(medicine);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Sales.Remove(newSale);
                    _context.Sales.Remove(saleCreated);
                    await _context.SaveChangesAsync();
                    return "No hay tantos medicamentos";
                }
            }
            catch (Exception ex)
            {
                return $"{ex.Message}. Details: {ex.Data}";
            }
        }
        catch (Exception ex)
        {
            return $"{ex.Message}. Details: {ex.Data}";
        }
        return "Sale made successfully!!";
    }
    public async Task<string> RegisterManyMedicinesAsync(Sale modelSale, List<SaleMedicine> list)
    {
        var newSale = new Sale
        {
            DateSale = DateTime.UtcNow,
            PatientId = modelSale.PatientId,
            EmployeeId = modelSale.EmployeeId,
            Prescription = modelSale.Prescription
        };
        try
        {
            _context.Sales.Add(newSale);
            await _context.SaveChangesAsync();
            var saleCreated = await _context.Sales
                                        .Where(u => u.Id == newSale.Id)
                                        .FirstOrDefaultAsync();
            List<SaleMedicine> newSaleMedicines = new();
            foreach (var saleMedicine in list)
            {
                var medicine = await _context.Medicines
                                    .Where(u => u.Id == saleMedicine.MedicineId)
                                    .FirstOrDefaultAsync();
                newSaleMedicines.Add(new SaleMedicine
                {
                    SaleId = saleCreated.Id,
                    MedicineId = saleMedicine.MedicineId,
                    SaleQuantity = saleMedicine.SaleQuantity,
                    Price = medicine.Price * saleMedicine.SaleQuantity,
                });
                if (medicine.Stock >= saleMedicine.SaleQuantity)
                {
                    int quantity = saleMedicine.SaleQuantity;
                    while (quantity != 0)
                    {
                        var nearestExpirationDate = await _context.PurchasedMedicines
                                                    .Where(u => u.MedicineId == saleMedicine.MedicineId && u.Stock > 0)
                                                    .OrderBy(o => o.ExpirationDate).FirstAsync();
                        if (nearestExpirationDate == null)
                        {
                            quantity = 0;
                        }
                        else
                        {
                            var purchasedMedicine = await _context.PurchasedMedicines.Where(u => u.Id == nearestExpirationDate.Id).FirstOrDefaultAsync();
                            if (nearestExpirationDate.Stock >= quantity)
                            {
                                //Stock Lote por fecha de vencimiento
                                purchasedMedicine.Stock -= quantity;
                                _context.PurchasedMedicines.Update(purchasedMedicine);
                                await _context.SaveChangesAsync();
                                quantity = 0;
                            }
                            else
                            {
                                quantity -= purchasedMedicine.Stock;
                                purchasedMedicine.Stock = 0;
                                _context.PurchasedMedicines.Update(purchasedMedicine);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    medicine.Stock -= saleMedicine.SaleQuantity;
                    _context.Medicines.Update(medicine);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Sales.Remove(newSale);
                    _context.Sales.Remove(saleCreated);
                    await _context.SaveChangesAsync();
                    return "No hay tantos medicamentos";
                }
            }
            try
            {
                _context.SaleMedicines.AddRange(newSaleMedicines);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return "Sale made successfully!!";
    }
    public async Task<object> GetAllRecipesAsync()
    {
        DateTime fechaDeseada = new DateTime(2023, 1, 1); // Reemplaza esta fecha por la fecha deseada
        var result = from sale in _context.Sales
                     join saleMedicine in _context.SaleMedicines on sale.Id equals saleMedicine.SaleId
                     join medicine in _context.Medicines on saleMedicine.MedicineId equals medicine.Id
                     where sale.Prescription && sale.DatePrescription >= fechaDeseada
                     select new
                     {
                         SaleId = sale.Id,
                         MedicineName = medicine.Name,
                         Quantity = saleMedicine.SaleQuantity
                     };
        return await result.ToListAsync();
    }
    public async Task<IEnumerable<Sale>> GetSaleMonthly(int parameter)
    {
        DateTime date = new(2023, parameter, 1);
        var query = _context.Sales as IQueryable<Sale>;
        var registros = await query
                                .Where(p => p.DateSale.Year == date.Year && p.DateSale.Month == date.Month)
                                .ToListAsync();
        return registros;
    }
    public async Task<object> GetTotalSalesOneMedicine(int id)
    {
        var existMedicine = await _context.Medicines
                                    .Where(u => u.Id == id)
                                    .FirstOrDefaultAsync();
        if (existMedicine == null)
        {
            return null;
        }
        var salesMedicine = await _context.SaleMedicines
                                    .Where(u => u.MedicineId == existMedicine.Id)
                                    .ToListAsync();
        object totalSales = new
        {
            TotalSales = salesMedicine.Select(u => u.SaleQuantity).Sum()
        };
        return totalSales;
    }
    public async Task<object> GetGainSales()
    {
        var totalGain = await _context.SaleMedicines.SumAsync(u => u.Price);
        object totalSales = new
        {
            TotalSales = totalGain
        };
        return totalSales;
    }
    public async Task<IEnumerable<Medicine>> GetUnsoldMedicine()
    {
        var sales = await _context.Sales.ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();
        var saleMedicines = await _context.SaleMedicines.ToListAsync();
        var salesMed = (from sale in sales
                        join saleMedicine in saleMedicines on sale.Id equals saleMedicine.SaleId
                        select saleMedicine.MedicineId)
                        .Distinct()
                        .ToList();
        var unsoldMed = medicines.Where(u => !salesMed.Any(s => s == u.Id));
        return unsoldMed;
    }
    public async Task<IEnumerable<Medicine>> GetUnsoldMedicines2023()
    {
        DateTime init2023 = new(2023, 1, 1);
        DateTime init2024 = new(2024, 1, 1);
        var sales = await _context.Sales
                                    .Where(u => u.DateSale >= init2023 && u.DateSale < init2024).ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();
        var saleMedicines = await _context.SaleMedicines.ToListAsync();
        var salesMed = (from sale in sales
                        join saleMedicine in saleMedicines on sale.Id equals saleMedicine.SaleId
                        select saleMedicine)
                        .Distinct()
                        .ToList();
        var unsoldMed = medicines.Where(u => !salesMed.Any(s => s.MedicineId == u.Id));
        return unsoldMed;
    }
    public async Task<IEnumerable<Patient>> GetPatients(int id)
    {
        var existMedicine = await _context.Medicines
                                    .Where(u => u.Id == id)
                                    .FirstOrDefaultAsync();
        if (existMedicine == null)
        {
            return null;
        }
        var patients = await _context.Patients.ToListAsync();
        var sales = await _context.Sales.ToListAsync();
        var salesMedicines = await _context.SaleMedicines.ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();
        var patientsPurchasedMedicine = (from patient in patients
                                         join sale in sales on patient.Id equals sale.PatientId
                                         join saleMedicine in salesMedicines on sale.Id equals saleMedicine.SaleId
                                         join medicine in medicines on saleMedicine.MedicineId equals medicine.Id
                                         where medicine.Id == id
                                         select patient).Distinct();
        return patientsPurchasedMedicine;
    }
    public async Task<IEnumerable<Patient>> GetPatients2023(int id)
    {
        var existMedicine = await _context.Medicines
                                     .Where(u => u.Id == id)
                                     .FirstOrDefaultAsync();
        if (existMedicine == null)
        {
            return null;
        }
        DateTime init2023 = new(2023, 1, 1);
        DateTime init2024 = new(2024, 1, 1);
        var sales = await _context.Sales
                                    .Where(u => u.DateSale >= init2023 && u.DateSale < init2024).ToListAsync();
        var patients = await _context.Patients.ToListAsync();
        var salesMedicines = await _context.SaleMedicines.ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();
        var patientsPurchasedMedicine = (from patient in patients
                                         join sale in sales on patient.Id equals sale.PatientId
                                         join saleMedicine in salesMedicines on sale.Id equals saleMedicine.SaleId
                                         join medicine in medicines on saleMedicine.MedicineId equals medicine.Id
                                         where medicine.Id == id
                                         select patient).Distinct();
        return patientsPurchasedMedicine;
    }
    public async Task<IEnumerable<object>> GetlessSoldMedicine()
    {
        DateTime init2023 = new(2023, 1, 1);
        DateTime init2024 = new(2024, 1, 1);
        var sales = await _context.Sales
                                    .Where(u => u.DateSale >= init2023 && u.DateSale < init2024).ToListAsync();
        var patients = await _context.Patients.ToListAsync();
        var salesMedicines = await _context.SaleMedicines.ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();
        var soldMedicine = (from medicine in medicines
                            join saleMedicine in salesMedicines on medicine.Id equals saleMedicine.MedicineId
                            join sale in sales on saleMedicine.SaleId equals sale.Id
                            select saleMedicine)
                                .Select(s => new
                                {
                                    NameMedicine = s.Medicine.Name,
                                    Quantity = s.SaleQuantity
                                })
                                .GroupBy(u => u.NameMedicine)
                                .Select(u => new
                                {
                                    idMedicine = u.Key,
                                    TotalQuantity = u.Sum(s => s.Quantity)
                                }).OrderBy(o => o.TotalQuantity).ToList();
        int min = soldMedicine.Min(a => a.TotalQuantity);
        var lessSoldMedicine = soldMedicine.Where(a => a.TotalQuantity == min).ToList();
        return lessSoldMedicine;
    }
    public async Task<IEnumerable<object>> GetPatientTotalSpent()
    {
        DateTime init2023 = new(2023, 1, 1);
        DateTime init2024 = new(2024, 1, 1);
        var sales = await _context.Sales
                                    .Where(u => u.DateSale >= init2023 && u.DateSale < init2024).ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();
        var patients = await _context.Patients.ToListAsync();
        var salesMedicines = await _context.SaleMedicines.ToListAsync();
        var patientsSales = (from patient in patients
                             join sale in sales on patient.Id equals sale.PatientId
                             join saleMedicine in salesMedicines on sale.Id equals saleMedicine.SaleId
                             select sale).Distinct()
                            .Select(s => new
                            {
                                IdenNumber = s.Patient.IdenNumber,
                                s.Patient.Name,
                                subSpent = s.SaleMedicines.Select(u => u.Price).Sum(),
                            }).GroupBy(g => g.Name)
                            .Select(u => new
                            {
                                IdenNumber = u.Select(a => a.IdenNumber).FirstOrDefault(),
                                Name = u.Key,
                                TotalSpent = u.Sum(a => a.subSpent)
                            });
        var patientWithoutSales = patients.Where(u => !sales.Any(s => s.PatientId == u.Id))
                                    .Select(u => new
                                    {
                                        IdenNumber = u.IdenNumber,
                                        u.Name,
                                        TotalSpent = 0.0
                                    });
        return patientsSales.Concat(patientWithoutSales);
    }
    public async Task<object> GetAverage()
    {
        var sales = await _context.Sales.ToListAsync();
        var result = from sale in sales
                     join saleMedicine in _context.SaleMedicines on sale.Id equals saleMedicine.SaleId
                     join medicine in _context.Medicines on saleMedicine.MedicineId equals medicine.Id
                     select new
                     {
                         SaleId = sale.Id,
                         MedicineName = medicine.Name,
                         Quantity = saleMedicine.SaleQuantity
                     };
        var averageQuantities = result.GroupBy(r => r.SaleId)
            .Select(group => new
            {
                SaleId = group.Key,
                AverageQuantity = group.Average(r => r.Quantity)
            })
            .ToList();
        return averageQuantities;
    }
    public async Task<object> GetSaleQuantityAsync()
    {
        var employees = await _context.Employees.ToListAsync();
        var quantity = employees.Select(u => new { u.Name, u.Sales.Count });
        return quantity;
    }
     public async Task<object> GetTotalMedicinesQuarter(int quarterM)
    {
        if (quarterM <= 0 || quarterM >= 5)
        {
            return null;
        }
        int init = 1;
        switch (quarterM)
        {
            case 1: init = 1; break;
            case 2: init = 4; break;
            case 3: init = 7; break;
            case 4: init = 10; break;
            default:
                break;
        }
        DateTime dateStart = new(2023, init, 1);
        DateTime dateEnd = dateStart.AddMonths(3);
        var sales = await _context.Sales
                            .Where(u => u.DateSale >= dateStart && u.DateSale <= dateEnd)
                            .ToListAsync();
        var saleMedicines = await _context.SaleMedicines.ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();
        var totalSold = (from sale in sales
                         join saleMedicine in saleMedicines on sale.Id equals saleMedicine.SaleId
                         join medicine in medicines on saleMedicine.MedicineId equals medicine.Id
                         select saleMedicine)
                             .Select(s => new
                             {
                                 Name = s.Medicine.Name,
                                 subQuantity = s.SaleQuantity
                             }).GroupBy(g => g.Name)
                             .Select(u => new
                             {
                                 NameMedicine = u.Key,
                                 TotalQuantity = u.Sum(a => a.subQuantity)
                             });
        var total = totalSold.Sum(s => s.TotalQuantity);
        object objecResult = new
        {
            Total = total,
            listMedicines = totalSold
        };
        return objecResult;
    }
    public async Task<IEnumerable<object>> GetPatientMoreSpent()
    {
        DateTime init2023 = new(2023, 1, 1);
        DateTime init2024 = new(2024, 1, 1);
        var sales = await _context.Sales
                                    .Where(u => u.DateSale >= init2023 && u.DateSale < init2024).ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();
        var patients = await _context.Patients.ToListAsync();
        var salesMedicines = await _context.SaleMedicines.ToListAsync();
        var groupSalesMedicine = (
                        from sale in sales
                        join saleMedicine in salesMedicines on sale.Id equals saleMedicine.SaleId
                        join medicine in medicines on saleMedicine.Id equals medicine.Id
                        select saleMedicine).GroupBy(u => u.SaleId);
        Dictionary<int, double> spentPatient = new();
        foreach (var group in groupSalesMedicine)
        {
            double spentSale = 0;
            foreach (var saleMedicine in group)
            {
                spentSale += saleMedicine.Price;
            }
            int idPatient = sales.Where(u => u.Id == group.Key).FirstOrDefault().PatientId;
            if (spentPatient.ContainsKey(idPatient))
            {
                spentPatient[idPatient] += spentSale;
            }
            else
            {
                spentPatient.Add(idPatient, spentSale);
            }
        }
        foreach (var patient in patients)
        {
            if (!spentPatient.ContainsKey(patient.Id))
            {
                spentPatient.Add(patient.Id, 0);
            }
        }
        double maxSpent = spentPatient.Max(patient => patient.Value);
        List<object> totalSpent = new();
        foreach (var dic in spentPatient)
        {
            if (dic.Value == maxSpent)
            {
                var patient = patients.Where(u => u.Id == dic.Key).FirstOrDefault();
                object objecResult = new
                {
                    patient.Id,
                    patient.Name,
                    TotalSpent = dic.Value
                };
                totalSpent.Add(objecResult);
            }
        }
        return totalSpent;
    }
    public async Task<IEnumerable<object>> GetBatchOfMedicines()
    {
        var medicines = await _context.Medicines.ToListAsync();
        var purchasedmeds = await _context.PurchasedMedicines.ToListAsync();
        var prueba = (from medicine in medicines
                      join purchasedmed in purchasedmeds on medicine.Id equals purchasedmed.MedicineId
                      select purchasedmed)
                        .Select(s => new
                        {
                            Id = s.Medicine.Id,
                            IdPurchase = s.PurchasedId,
                            StockLote = s.Stock,
                            ExpirationDate = s.ExpirationDate
                        }).GroupBy(g=> g.Id)
                        .Select(a=> new{
                            Id = a.Key,
                            NameMedicine = medicines.Where(u=> u.Id == a.Key).Select(u=> u.Name).FirstOrDefault(),
                            ListBatch = a.Select(u=> new{
                                ExpirationDate = u.ExpirationDate,
                                StockLote = u.StockLote
                            }).OrderBy
                            (o => o.ExpirationDate)
                        });
        return prueba;
    }
    public async Task<object> GetAllSales()
    {
        var sales = await _context.Sales.ToListAsync();
        var patients = await _context.Patients.ToListAsync();
        var employees = await _context.Employees.ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();
        var saleMedicines = await _context.SaleMedicines.ToListAsync();
        var result = from sale in sales
                     join employee in employees on sale.EmployeeId equals employee.Id
                     join patient in patients on sale.PatientId equals patient.Id
                     join saleMedicine in saleMedicines on sale.Id equals saleMedicine.SaleId
                     join medicine in medicines on saleMedicine.MedicineId equals medicine.Id
                     select new
                     {
                         SaleId = sale.Id,
                         EmployeeName = employee.Name,
                         PatientName = patient.Name,
                         MedicineName = medicine.Name,
                         Quantity = saleMedicine.SaleQuantity,
                         Prescription = sale.Prescription,
                         DatePrescription = sale.DatePrescription
                     };
        return result;
    }
}