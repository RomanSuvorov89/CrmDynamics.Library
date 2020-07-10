namespace CrmDynamics.Library.Models.Crm
{
    public class OptionSetValue
    {
        public int Value { get; set; }
        public string FormattedValue { get; }

        public OptionSetValue(int value, string formattedValue = null)
        {
            Value = value;
            FormattedValue = formattedValue;
        }
    }
}
