namespace Domain.Entities;

public class Employee : BaseEntity
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string IdenNumber { get; set; }
        public DateTime DateContract { get; set; }

        public ICollection<Sale> Sales { get; set;}
    }
