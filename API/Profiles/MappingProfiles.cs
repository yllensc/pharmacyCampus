using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;

namespace API.Profiles;

public class MappingProfiles : Profile
{

    public MappingProfiles()
    {
        CreateMap<Provider, ProviderDto>()
            .ReverseMap()
            .ForMember(o => o.Purchases, d => d.Ignore())
            .ForMember(o => o.Medicines, d => d.Ignore());
        CreateMap<Provider, ProviderPutDto>()
            .ReverseMap();
        CreateMap<Employee, EmployeeDto>()
            .ReverseMap();
        CreateMap<Employee, EmployeeGetDto>()
            .ReverseMap()
            .ForMember(o => o.PositionId, d => d.Ignore());
        CreateMap<Position, PositionDto>()
            .ReverseMap()
            .ForMember(o => o.Id, d => d.Ignore());
        CreateMap<Medicine, MedicineAllDto>()
            .ForMember(dest => dest.ProviderName, origen => origen.MapFrom(o => o.Provider.Name))
            .ReverseMap()
            .ForMember(o => o.PurchasedMedicines, d => d.Ignore())
            .ForMember(o => o.SaleMedicines, d => d.Ignore());
        CreateMap<Medicine, MedicinePutDto>()
            .ReverseMap();
        CreateMap<Patient, PatientDto>()
            .ReverseMap()
            .ForMember(o => o.Sales, d => d.Ignore());
        CreateMap<Patient, PatientPutDto>()
            .ReverseMap();
        CreateMap<Sale, SaleDto>()
            .ReverseMap()
            .ForMember(o => o.SaleMedicines, d => d.Ignore());
        
         CreateMap<SaleMedicine,SaleDto>()
            .ReverseMap();        

        CreateMap<Sale,SaleAverageDto>()
        .ReverseMap(); 
        
        CreateMap<Sale, SaleManyPostDto>()
            .ForMember(dest => dest.MedicinesList, origen => origen.MapFrom(o => o.SaleMedicines))
            .ReverseMap()
            .ForMember(o => o.SaleMedicines, d => d.Ignore());
        
        CreateMap<SaleMedicine,SaleMedicinePostDto>()
            .ReverseMap();

        CreateMap<Provider,ProviderxPurchaseDto>()
                .ForMember(dest=> dest.purchases, origen => origen.MapFrom(o => o.Purchases))
                .ReverseMap();
        CreateMap<Provider, ProviderWithListMedicinesDto>()
            .ForMember(dest => dest.Medicines, opt => opt.MapFrom(src => src.Medicines.Select(medicine => new MedicineBaseDto
            {
                Name = medicine.Name,
                Price = medicine.Price
            })))
            .ReverseMap();
        CreateMap<Purchase, PurchaseDto>()
            .ForMember(dest => dest.purchaseMedicines, origen => origen.MapFrom(o => o.PurchasedMedicines))
            .ReverseMap();
        CreateMap<Purchase, PurchasePostDto>()
            .ReverseMap();
        CreateMap<PurchasedMedicine, PurchasePostDto>()
            .ReverseMap();
        CreateMap<PurchasedMedicine, PurchaseMedicineDto>()
            .ForMember(dest => dest.MedicineName, origen => origen.MapFrom(o => o.Medicine.Name))
            .ReverseMap();
        CreateMap<Purchase, PurchaseManyPostDto>()
            .ForMember(dest=> dest.MedicinesList, origen => origen.MapFrom(o => o.PurchasedMedicines))
            .ReverseMap();
        CreateMap<PurchasedMedicine, PurchaseManyPostDto>()
            .ForMember(dest => dest.MedicinesList, origen => origen.MapFrom(o => o.Medicine.Name))
            .ReverseMap();
        CreateMap<PurchasedMedicine, PurchaseMedicinePostDto>()
            .ReverseMap();
    }

}
