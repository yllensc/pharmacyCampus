using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using iText.Svg.Renderers.Path.Impl;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;

public class PatientRepository : GenericRepository<Patient>, IPatient
{
    private readonly PharmacyDbContext _context;

    public PatientRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<string> RegisterAsync(Patient patient)
    {
        var newPatient = new Patient
        {
            Name = patient.Name,
            IdenNumber = patient.IdenNumber,
            PhoneNumber = patient.PhoneNumber,
            Address = patient.Address
        };

        var existingID = _context.Patients
                                .Where(u => u.IdenNumber.ToLower() == patient.IdenNumber.ToLower())
                                .FirstOrDefault();
        
        if (existingID == null)
        {
            try
            {
                _context.Patients.Add(newPatient);
                await _context.SaveChangesAsync();

                return $"User  {patient.Name} has been registered successfully";
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return $"Error: {message}";
            }
        }else{
            return $"Patient with Identifaction Number {patient.IdenNumber} alredy register ";
        }
    
    }
    public async Task<string> UpdateAsync(Patient model){
            
        var patient =  _context.Patients.Where(u => u.Id == model.Id).FirstOrDefault();
        
        if(patient == null){
            return "Id Patient doesn't exist";
        }
        patient.Name = model.Name;
        patient.PhoneNumber = model.PhoneNumber;
        patient.Address = model.Address;

        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();

        return "Patient update successfully";

    }

    public async Task<IEnumerable<Patient>> GetPatientWithNoSalesInxYear(int year){
        return await _context.Patients
                .Where(p => !p.Sales.Any(s=> s.DateSale.Year == year)).ToListAsync();
    }


}
