namespace MultiTenant.Constracts
{
    public interface IMustHaveTenant
    {
        public string TenantId { get; set; }
    }
}
