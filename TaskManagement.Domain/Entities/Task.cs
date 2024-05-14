using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Domain.Entities
{
    public class Task : Entity<int>
    {
        [Required]
        public string TaskName { get; set; }
        public List<string> Tags { get; set; } = new List<string>(); // Separated By Pipe | in db
        public DateTime? DueDate { get; set; }
        public string Color { get; set; }
        public int Status { get; set; } // 0 - New, 1 - Active, 2- Completed
        public string AssignedTo { get; set; }
        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

        [NotMapped]
        public string TagsString
        {
            get => string.Join(", ", Tags);
            set => Tags = value.Split(',').Select(t => t.Trim()).ToList();
        }

        [NotMapped]
        public string StatusString
        {
            get
            {
                string s = string.Empty;
                if (Status == 0)
                {
                    s = "New";
                }
                else if (Status == 1)
                {
                    s = "Pending";
                }
                else
                {
                    s = "Completed";
                }
                return s;
            }
            set
            {
                if (value.ToLower() == "new")
                {
                    Status = 0;
                }
                else if (value.ToLower() == "pending")
                {
                    Status = 1;
                }
                else if (value.ToLower() == "completed")
                {
                    Status = 2;
                }
            }
        }
    }
}
