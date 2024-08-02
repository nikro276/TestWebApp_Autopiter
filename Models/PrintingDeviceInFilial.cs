namespace TestWebApp_Autopiter.Models
{
    public class PrintingDeviceInFilial
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int NumberInFilial { get; set; }
        public bool IsDefault { get; set; }
        public int? PrintingDeviceId { get; set; }
        public PrintingDevice? PrintingDevice { get; set; }
        public int? FilialId { get; set; }
        public Filial? Filial { get; set; }
    }
}