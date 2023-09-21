using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;

namespace API.Profiles;

public class MappingProfiles: Profile
{
   
    public MappingProfiles()
    {
        CreateMap<Provider,ProviderDto>()
            .ReverseMap()
            .ForMember(o=> o.Purchases, d => d.Ignore())
            .ForMember(o=> o.Medicines, d => d.Ignore());

    }

}
