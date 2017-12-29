namespace DebtSettlement.BusinessLayer.Models
{
    public class AgreementActionParameter
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public int ParameterType { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }
}