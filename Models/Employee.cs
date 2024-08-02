namespace TestWebApp_Autopiter.Models
{
    public class Employee
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? FilialId { get; set; }
        public Filial? Filial { get; set; }
    }
}
