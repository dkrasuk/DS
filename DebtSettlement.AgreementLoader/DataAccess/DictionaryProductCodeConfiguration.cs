using System.Data.Entity.ModelConfiguration;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.AgreementLoader.DataAccess
{
    internal class DictionaryProductCodeConfiguration : EntityTypeConfiguration<DictionaryProductCode>
    {
        public DictionaryProductCodeConfiguration()
        {
            ToTable("DICTIONARY_PRODUCTCODE", "COLLECTSM");

            Property(t => t.Id).HasColumnName("PRODUCTCODEID");
            Property(t => t.ProductCode).HasColumnName("PRODUCTCODE");

            HasKey(t => t.Id);

            HasMany(t => t.Agreements)
                .WithOptional(t => t.ProductCode)
                .HasForeignKey(t => t.ProductCodeId);
        }
    }
}
