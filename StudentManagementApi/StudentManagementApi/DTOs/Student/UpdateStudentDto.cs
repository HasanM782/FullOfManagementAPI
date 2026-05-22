using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.DTOs.Student
{
    public class UpdateStudentDto
    {
        [Required(ErrorMessage = "Ad boş ola bilməz!")]
        [MinLength(3, ErrorMessage = "Ad minimum 3 simvol olmalıdır!")]
        public string FullName { get; set; }

        [Range(16, 60, ErrorMessage = "Yaş 16 və 60 arasında olmalıdır!")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email boş ola bilməz!")]
        [EmailAddress(ErrorMessage = "Email düzgün formatda deyil!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "GroupId boş ola bilməz!")]
        public int GroupId { get; set; }
    }
}
