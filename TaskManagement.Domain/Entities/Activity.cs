using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Domain.Entities
{
    public class Activity : Entity<int>
    {
        [ForeignKey("Task")]
        public int TaskId { get; set; }
        public DateTime? ActivityDate { get; set; }

        [Required]
        public string DoneBy { get; set; }

        [Required]
        public string ActivityDescription { get; set; }
        public virtual Task? Task { get; set; }
    }
}
