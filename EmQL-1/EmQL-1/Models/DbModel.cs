using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmQL_1.Models
{
    public enum Gender { Male=1, Female}
    public class Employee
    {
        public int EmployeeId { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; } = default!;
        [Required, EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
        [Required, StringLength(50)]
        public string Address { get; set; } = default!;
        [Required, Column(TypeName ="date")]
        public DateTime? JoiningDate { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal? Salary { get; set; }
        public bool IsaCurrentEmployee { get; set; }
        [Required, StringLength(50)]
        public string Picture { get; set; } = default!;
        public virtual ICollection<Qualification> Qualifications { get; set; }= new List<Qualification>();
    }
    public class Qualification 
    { 
      public int QualificationId { get; set; }
        [Required]
        public int? PassingYear { get; set; }
        [Required, StringLength(50)]
        public string Degree { get; set; } = default!;
        [Required, ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }= default!;

    }
    public class EmployeeDbCOntext(DbContextOptions<EmployeeDbCOntext> options) : DbContext(options) 
    {
       public DbSet<Employee> Employees { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
    
    }


}
