using System.ComponentModel.DataAnnotations;

namespace TeacherOs.DTO
{
    public class UserEditDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string? Firstname { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string? Lastname { get; set; }
    }
}