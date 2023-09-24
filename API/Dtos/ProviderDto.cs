using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ProviderDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string IdenNumber { get; set;}
        public string Email { get; set;}
        public string Address { get; set;}
    }

    public class ProviderMoreQuantityMedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MoreQuantity { get; set; }
    }
}