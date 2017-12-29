using System.Data.Entity.ModelConfiguration;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.AgreementLoader.DataAccess
{
    internal class DictionaryActionTypeConfiguration : EntityTypeConfiguration<DictionaryActionType>
    {
        public DictionaryActionTypeConfiguration()
        {
            ToTable("DICTIONARY_ACTIONTYPE", "PROCESSMANAGER");

            Property(t => t.Id).HasColumnName("ID");
            Property(t => t.Name).HasColumnName("NAME");

            HasKey(t => t.Id);
        }
    }
}
