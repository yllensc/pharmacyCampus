using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;

public class MedicineRepository : GenericRepository<Medicine>, IMedicineRepository
{
    private readonly PharmacyDbContext _context;

    public MedicineRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;

    }

    public async Task<string> RegisterAsync(Medicine model)
        {
            var existingID = _context.Providers
                        .Where(u => u.Id == model.ProviderId)
                        .FirstOrDefault();

            if (existingID != null) {
                var medicine = new Medicine
            {
                Name = model.Name,
                ProviderId = model.ProviderId,
                Stock = model.Stock,
                Price = model.Price
            };

            var existMedicine = _context.Medicines
                                       .Where(u => u.Name.ToLower() == model.Name.ToLower() && u.ProviderId == model.ProviderId)
                                       .FirstOrDefault();
            if (existMedicine == null)
            {
                try
                {
                    _context.Medicines.Add(medicine);
                    await _context.SaveChangesAsync();

                    return $"Medicine  {model.Name} has been registered successfully";
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return $"Error: {message}";
                }
            }
            else
            {
                return $"Medicine {model.Name} alredy register with this provider";
            }
             }
             else{
                return "El proveedor no existe en nuestro sistema, sorry";
             }
        }
    public async Task<string> UpdateAsync(Medicine model)
        {
            var medicine =  _context.Medicines.Where(u=> u.Id == model.Id).FirstOrDefault();
            if(medicine != null){
                medicine.Stock = model.Stock;
                medicine.Price = model.Price;
                _context.Medicines.Update(medicine);
                await _context.SaveChangesAsync();
                return "medicine update successfully";

            }
            else{
                return "El registro que quieres actualizar, no existe, sorry";
            }  
        }
    public async Task<IEnumerable<Medicine>> GetUnder50()
    {
        return await _context.Medicines.Where(m => m.Stock < 50).ToListAsync();
    }

    public async Task<IEnumerable<Medicine>> GetExpireUnder2024()
    {
        DateTime init2024 = new(2024,1,1);
        var listPurchases = await _context.PurchasedMedicines.Where(m => m.ExpirationDate < init2024).ToListAsync();
        List<Medicine> listMedicines = new();
        foreach(var m in listPurchases){
            var medicine = _context.Medicines.Where(me=>me.Id == m.MedicineId).FirstOrDefault();
            if(medicine != null){
                listMedicines.Add(medicine);
            }
        }
        return listMedicines;
    }
    public async Task<IEnumerable<Medicine>> GetExpireUntil2024()
    {
        DateTime init2024 = new(2024,1,1);
        var listPurchases = await _context.PurchasedMedicines.Where(m => m.ExpirationDate >= init2024).ToListAsync();
        List<Medicine> listMedicines = new();
        foreach(var m in listPurchases){
            var medicine = _context.Medicines.Where(me=>me.Id == m.MedicineId).FirstOrDefault();
            if(medicine != null){
                listMedicines.Add(medicine);
            }
        }
        await _context.SaveChangesAsync();
        return listMedicines;
    }
    public async Task<IEnumerable<Medicine>> GetMoreExpensive()
    {
        double priceMoreExpensive = 0;
        List<Medicine> listMedicines = new();
        foreach (var m in _context.Medicines){
            var medicinePrice = m.Price;
            if (medicinePrice >= priceMoreExpensive){
                priceMoreExpensive = medicinePrice;
            }
        }
        return  await _context.Medicines.Where(m=>m.Price == priceMoreExpensive).ToListAsync(); 
    }
    public async Task<IEnumerable<Medicine>> GetRangePriceStockPredeterminated()
    {
        List<Medicine> listMedicines = new();
        foreach(var m in _context.Medicines){
            if (m.Price >= 50000 && m.Stock > 100){
                listMedicines.Add(m);
            }
        }
        await _context.SaveChangesAsync();
        return listMedicines;
        
    }

    public async Task<IEnumerable<Provider>> GetProvidersInfoWithMedicines()
    {
          return await this._context.Providers
            .Include(d => d.Medicines)
            .ToListAsync();
    }
    public override async Task<Medicine> GetByIdAsync(int id)
    {
        return await _context.Medicines
                    .Include(p => p.PurchasedMedicines)
                    .FirstOrDefaultAsync(p=>p.Id == id);
    }
}

