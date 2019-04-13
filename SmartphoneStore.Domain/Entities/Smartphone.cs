namespace SmartphoneStore.Domain.Entities
{
    public class Smartphone
    {
        public int SmartphoneId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
    }
}