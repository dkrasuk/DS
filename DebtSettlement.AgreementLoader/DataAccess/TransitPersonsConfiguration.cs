using System.Data.Entity.ModelConfiguration;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.AgreementLoader.DataAccess
{
    internal class TransitPersonsConfiguration : EntityTypeConfiguration<TransitPersons>
    {
        public TransitPersonsConfiguration()
        {
            ToTable("TRANSIT_PERSONS", "COLLECTSM");

            Property(t => t.PersonId).HasColumnName("PERSONID");
            Property(t => t.Inn).HasColumnName("INN");
            Property(t => t.LastName).HasColumnName("LASTNAME");
            Property(t => t.FirstName).HasColumnName("FIRSTNAME");
            Property(t => t.Patronymic).HasColumnName("PATRONYMIC");
            Property(t => t.JuridicalName).HasColumnName("JURIDICALNAME");
            Property(t => t.Fio).HasColumnName("FIO");            

            HasKey(t => t.PersonId);

            HasMany(t => t.Agreements)
                .WithOptional(t => t.Person)
                .HasForeignKey(t => t.PersonId);

            HasMany(t => t.ContactsAdress)
                .WithOptional(t => t.Person)
                .HasForeignKey(t => t.PersonId);

            HasMany(t => t.LinkedPersons)
                .WithOptional(t => t.Persons)
                .HasForeignKey(t => t.LincedPersonId);
        }
    }
}
