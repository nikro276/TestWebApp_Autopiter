namespace TestWebApp_Autopiter.Models
{
    public class PrintJob
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int PrintingDeviceNumber { get; set; }
        public int PagesCount { get; set; }
        public int StatusCode { get; set; }
        public int? FilialId { get; set; }
        public Filial? Filial { get; set; }
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}