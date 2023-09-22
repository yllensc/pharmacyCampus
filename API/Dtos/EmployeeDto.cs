using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;
    public class EmployeeDto
    {
        public int Id { get; set;}
        [Required]
        public string Name { get; set; }
        [Required]
        public int PositionId { get; set; }
        public PositionDto Position { get; set; }
        [Required]
        public DateTime DateContract { get; set; }
        
    }
