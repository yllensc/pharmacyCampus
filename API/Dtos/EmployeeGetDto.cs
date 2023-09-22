using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class EmployeeGetDto
    {
        public int Id { get; set;}
        [Required]
        public string Name { get; set; }
        public string PositionName { get; set; }
        [Required]
        public DateTime DateContract { get; set; }
    }
}