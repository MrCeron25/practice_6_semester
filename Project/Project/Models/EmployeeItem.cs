namespace Project.ViewModels
{
    public class EmployeeItem
    {
        public long employe_id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string patronymic { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public long role { get; set; }
        public string role_name { get; set; }
        public string phone { get; set; }
        public string sex { get; set; }
        public byte[] photo { get; set; }
    }
}
