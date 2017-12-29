using System.Data.Entity.ModelConfiguration;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.AgreementLoader.DataAccess
{
    internal class TransitLinkedPersonsConfiguration : EntityTypeConfiguration<TransitLinkedPersons>
    {
        public TransitLinkedPersonsConfiguration()
        {

            ToTable("TRANSIT_LINKEDPERSONS", "COLLECTSM");

            Property(t => t.LinkId).HasColumnName("LINKID");
            Property(t => t.LincedPersonId).HasColumnName("LINCEDPERSONID");
            Property(t => t.PersonId).HasColumnName("PERSONID");
            Property(t => t.LinkTypeId).HasColumnName("LINKTYPEID");
            Property(t => t.AgreemId).HasColumnName("AGREEMID");
            Property(t => t.LinkedAgreemId).HasColumnName("LINKEDAGREEMID");

            HasKey(t => t.PersonId);

            HasOptional(t => t.Persons)
                .WithMany(t => t.LinkedPersons)
                .HasForeignKey(t => t.LincedPersonId);


        }

    }
}
