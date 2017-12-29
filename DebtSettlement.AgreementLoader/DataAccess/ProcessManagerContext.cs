using System.Data.Entity;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.AgreementLoader.DataAccess
{
    internal class ProcessManagerContext : DbContext
    {
        public ProcessManagerContext()
            :base("ProcessManagerContext")
        {
        }

        public DbSet<DictionaryActionType> DictionaryActionType { get; set; }

        public DbSet<Process> Processes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("PROCESSMANAGER");

            modelBuilder.Configurations.Add(new DictionaryActionTypeConfiguration());
            modelBuilder.Configurations.Add(new ProcessConfiguration());
        }
    }
}
