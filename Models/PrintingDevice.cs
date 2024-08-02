namespace TestWebApp_Autopiter.Models
{
    public class PrintingDevice
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? MAC { get; set; }
        public int? ConnectionTypeId { get; set; }
        public ConnectionType? ConnectionType { get; set; }
    }
}