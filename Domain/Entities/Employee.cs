namespace Domain.Entities;

public class Employee : BaseEntity
    {
        public int UserId { get; set; } 
        public User User { get; set; }
        public string Name { get; set; }
        public int PositionId {get; set;}
        public Position Position { get; set; }
        public DateTime DateContract { get; set; }  = DateTime.UtcNow;
        public ICollection<Sale> Sales { get; set;}
    }
