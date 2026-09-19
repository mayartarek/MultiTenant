namespace MultiTenant.Entities
{
    public class Product :IMustHaveTenant
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Rated { get; set; }
        public string TenantId { get; set; } = null!;
    }
}
