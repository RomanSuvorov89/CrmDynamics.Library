namespace CrmDynamics.Library.Models.Crm
{
    public class Money
    {
        public decimal Value { get; set; }

        public Money(decimal value)
        {
            Value = value;
        }
    }
}
