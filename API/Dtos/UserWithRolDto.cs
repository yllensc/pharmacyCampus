using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class UserWithRolDto
    {
        public int Id {get; set;}
        public string UserName { get; set; }
        public string RolName { get; set; }
    }
}