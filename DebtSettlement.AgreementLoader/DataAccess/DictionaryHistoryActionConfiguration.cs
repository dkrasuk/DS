using System.Data.Entity.ModelConfiguration;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.AgreementLoader.DataAccess
{
    internal class DictionaryHistoryActionConfiguration : EntityTypeConfiguration<DictionaryHistoryAction>
    {
        public DictionaryHistoryActionConfiguration()
        {
            ToTable("DICTIONARY_HIST_ACTION", "COLLECTSM");

            Property(t => t.Id).HasColumnName("ACTIONTYPEID");
            Property(t => t.ActionType).HasColumnName("ACTIONTYPE");

            HasKey(t => t.Id);
        }
    }
}
