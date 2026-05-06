using System.ComponentModel.DataAnnotations;

namespace todo_api_week11.DTOs
{
    public class CreateTodoDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        [RegularExpression("Low|Medium|High")]
        public string Priority { get; set; } = "Medium";
    }

}
