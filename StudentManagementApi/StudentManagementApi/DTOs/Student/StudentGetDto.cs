namespace StudentManagementApi.DTOs.Student
{
    public class StudentGetDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string GroupName { get; set; }
    }
}
