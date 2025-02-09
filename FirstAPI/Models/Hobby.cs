using System.ComponentModel.DataAnnotations;

namespace FirstAPI.Models
{
    public class Hobby
    {
        public int Id { get; set; }
        [RegularExpression(@"^[^ ].*[^ ]$", ErrorMessage = "Content must not have leading or trailing spaces.")]
        public string Title { get; set; } = "";

        [RegularExpression(@"^[^ ].*[^ ]$", ErrorMessage = "Content must not have leading or trailing spaces.")]
        public string Content { get; set; } = "";
        public string Image { get; set; } = "";

        // Foreign key
        //Navigation property
    }
}
