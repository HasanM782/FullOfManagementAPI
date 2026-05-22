using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.DTOs.Group
{
    public class UpdateGroupDto
    {
        [Required(ErrorMessage = "Ad boş ola bilməz!")]
        public string Name { get; set; }
    }
}
