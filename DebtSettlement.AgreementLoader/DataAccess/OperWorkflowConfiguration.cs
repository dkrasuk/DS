using System.Data.Entity.ModelConfiguration;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.AgreementLoader.DataAccess
{
    internal class OperWorkflowConfiguration : EntityTypeConfiguration<OperWorkflow>
    {
        public OperWorkflowConfiguration()
        {
            ToTable("OPER_WORKFLOW", "COLLECTSM");

            Property(t => t.Id).HasColumnName("SYS_TABLEKEY");
            Property(t => t.AgreementId).HasColumnName("SYS_RECORDKEY");
            Property(t => t.AssignedCollector).HasColumnName("T_685_ASSIGNEDCOLLECTOR");
            Property(t => t.AssignedFieldUser).HasColumnName("T_683_ASSIGNEDFIELDUSER");
            Property(t => t.AssignedLegalUser).HasColumnName("T_684_ASSIGNEDLEGALUSER");

            HasKey(t => t.Id);
        }
    }
}
