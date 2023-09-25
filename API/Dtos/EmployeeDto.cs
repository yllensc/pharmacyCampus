using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;
    public class EmployeeGetIdNameDto
    {
        public int Id { get; set;}
        [Required]
        public string Name { get; set; }
    }
    public class EmployeeGetDto: EmployeeGetIdNameDto
    {
        public string PositionName { get; set; }
        [Required]
        public DateTime DateContract { get; set; }
    }
    public class EmployeeAllDto
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
