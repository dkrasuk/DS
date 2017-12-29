namespace DebtSettlement.Model.DTO
{
    public class UserDTO
    {
        public string FullName { get; set; }

        public string Login { get; set; }

        public int IsBlock { get; set; }

        public string Category { get; set; }

        public int DepartmentId { get; set; }

        public string Email { get; set; }
    }
}