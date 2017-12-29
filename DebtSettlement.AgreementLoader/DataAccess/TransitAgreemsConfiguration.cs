using System.Data.Entity.ModelConfiguration;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.AgreementLoader.DataAccess
{
    internal class TransitAgreemsConfiguration : EntityTypeConfiguration<TransitAgreems>
    {
        public TransitAgreemsConfiguration()
        {
            ToTable("TRANSIT_AGREEMS", "COLLECTSM");

            Property(t => t.AgreementId).HasColumnName("AGREEMID");
            Property(t => t.PersonId).HasColumnName("CLIENTID");
            Property(t => t.AgreementNumber).HasColumnName("AGREEMNUMBER");
            Property(t => t.OpenDate).HasColumnName("OPENEDDATE");
            Property(t => t.PlanedCloseDate).HasColumnName("PLANEDCLOSEDATE");
            Property(t => t.ProductCodeId).HasColumnName("PRODUCTCODEID");
            Property(t => t.Outstanding).HasColumnName("OUTSTANDING");

            HasKey(t => t.AgreementId);

            HasOptional(t => t.Person)
                .WithMany(t => t.Agreements)
                .HasForeignKey(t => t.PersonId);

            HasOptional(t => t.ProductCode)
                .WithMany(t => t.Agreements)
                .HasForeignKey(t => t.ProductCodeId);

            HasOptional(t => t.OperWorkflow)
                .WithRequired(t => t.TransitAgreem);

            HasOptional(t => t.Delinquency)
                .WithRequired(t => t.TransitAgreem);
        }
    }
}
