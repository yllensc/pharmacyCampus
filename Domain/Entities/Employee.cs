namespace Domain.Entities;

public class Employee : BaseEntity
    {
        public int UserId { get; set; } 
        public User User { get; set; }
        public string Name { get; set; }
<<<<<<< HEAD
        public int PositionId {get; set;}
        public Position Position { get; set; }
=======
        public string Position { get; set; }
>>>>>>> e85a095 (feat: :alembic: cambios y experimentos con roles y usuarios jeje)
        public DateTime DateContract { get; set; }
        public ICollection<Sale> Sales { get; set;}
    }
