using System.Data.Entity.ModelConfiguration;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.AgreementLoader.DataAccess
{
    internal class ProcessConfiguration : EntityTypeConfiguration<Process>
    {
        public ProcessConfiguration()
        {
            ToTable("PROCESS", "PROCESSMANAGER");

            Property(t => t.Id).HasColumnName("ID");
            Property(t => t.AgreementId).HasColumnName("AGREEMID");
            Property(t => t.ProcessType).HasColumnName("PROCESSTYPE");
            Property(t => t.State).HasColumnName("STATE");
            Property(t => t.Stage).HasColumnName("STAGE");
            Property(t => t.Status).HasColumnName("STATUS");
            Property(t => t.DateOpen).HasColumnName("DATEOPEN");
            Property(t => t.DateClose).HasColumnName("DATECLOSE");
            Property(t => t.Identifier).HasColumnName("IDENTIFIER");
            Property(t => t.Urisdiction).HasColumnName("URISTDICTION");
            Property(t => t.Initiator).HasColumnName("INITIATOR");
            Property(t => t.Responsible).HasColumnName("RESPONSIBLE");
            Property(t => t.Name).HasColumnName("NAME");
            Property(t => t.ControlDate).HasColumnName("CONTROLDATE");
            Property(t => t.NumberDeal).HasColumnName("NUMBERDEAL");
            Property(t => t.ControlDateTypeId).HasColumnName("CONTROLDATETYPEID");

            HasKey(t => t.Id);
        }
    }
}
