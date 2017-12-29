using System.Data.Entity.ModelConfiguration;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.AgreementLoader.DataAccess
{
    internal class TransitDelinquencyConfiguration : EntityTypeConfiguration<TransitDelinquency>
    {
        public TransitDelinquencyConfiguration()
        {
            ToTable("TRANSIT_DELINQUENCY", "COLLECTSM");
            Property(t => t.AgreemId).HasColumnName("AGREEMID");
            Property(t => t.DPD).HasColumnName("DPD");

            HasKey(t => t.AgreemId);
        }
    }
}
