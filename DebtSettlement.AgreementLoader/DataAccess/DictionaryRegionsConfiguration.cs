using System;

using System.Data.Entity.ModelConfiguration;
using DebtSettlement.AgreementLoader.Models;


namespace DebtSettlement.AgreementLoader.DataAccess
{
    internal class DictionaryRegionsConfiguration : EntityTypeConfiguration<DictionaryRegions>
    {
        public DictionaryRegionsConfiguration()
        {
            ToTable("DICTIONARY_REGIONS", "COLLECTSM");

            Property(t => t.RegionId).HasColumnName("REGIONID");
            Property(t => t.Region).HasColumnName("REGION");

            HasKey(t => t.RegionId);

            HasMany(t => t.ContactsAdress)
                .WithOptional(t => t.Region)
                .HasForeignKey(t => t.RegionId);

        }
    }
}
