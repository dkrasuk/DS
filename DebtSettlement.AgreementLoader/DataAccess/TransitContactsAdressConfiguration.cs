using System.Data.Entity.ModelConfiguration;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.AgreementLoader.DataAccess
{
    internal class TransitContactsAdressConfiguration : EntityTypeConfiguration<TransitContactsAdress>
    {
        public TransitContactsAdressConfiguration()
        {
            ToTable("TRANSIT_CONTACTS_ADRESS", "COLLECTSM");

            Property(t => t.AdressId).HasColumnName("ADRESSID");
            Property(t => t.PersonId).HasColumnName("PERSONID");
            Property(t => t.RegionId).HasColumnName("REGIONID");
            Property(t => t.Settlement).HasColumnName("SETTLEMENT");
            Property(t => t.AdressFull).HasColumnName("ADRESSFULL");
            Property(t => t.Work).HasColumnName("WORK");

            HasKey(t => t.AdressId);

            HasOptional(t => t.Person)
                .WithMany(t => t.ContactsAdress)
                .HasForeignKey(t => t.PersonId);

            HasOptional(t => t.Region)
                .WithMany(t => t.ContactsAdress)
                .HasForeignKey(t => t.RegionId);

        }
    }
}
