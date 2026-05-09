namespace HRMS.Domain.Entities
{
    public class PaymentStatus:BaseEntity
    {
        public string StatusName { get; set; }
        public string StatusDisplayName { get; set; }
        public ICollection<Payroll> Payrolls { get; set; }=new List<Payroll>();
    }
}
