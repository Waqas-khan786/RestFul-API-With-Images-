namespace RestFul_API_With_Images.Model
{
    public class Student : Status
    {
        public Student()
        {
            StudentsList = new List<Student>();
        }
        public int Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Student> StudentsList { get; set; }
    }
}
