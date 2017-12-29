using System.Data.Entity;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.AgreementLoader.DataAccess
{
    internal class CollectSmContext : DbContext
    {
        public CollectSmContext()
            : base("CollectSmContext")
        {
        }

        public DbSet<DictionaryProductCode> DictionaryProductCode { get; set; }

        public DbSet<DictionaryHistoryAction> DictionaryHistoryAction { get; set; }

        public DbSet<TransitAgreems> TransitAgreems { get; set; }

        public DbSet<TransitPersons> TransitPersons { get; set; }

        public DbSet<OperWorkflow> OperWorkflow { get; set; }

        public DbSet<Process> Process { get; set; }

        public DbSet<TransitContactsAdress> TransitContactsAdress { get; set; }

        public DbSet<DictionaryRegions> DictionaryRegions { get; set; }

        public DbSet<TransitLinkedPersons> TransitLinkedPersons { get; set; }

        public DbSet<TransitDelinquency> TransitDelinquency { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("COLLECTSM");

            modelBuilder.Configurations.Add(new DictionaryProductCodeConfiguration());
            modelBuilder.Configurations.Add(new DictionaryHistoryActionConfiguration());
            modelBuilder.Configurations.Add(new DictionaryRegionsConfiguration());
            modelBuilder.Configurations.Add(new TransitAgreemsConfiguration());
            modelBuilder.Configurations.Add(new TransitPersonsConfiguration());
            modelBuilder.Configurations.Add(new TransitContactsAdressConfiguration());
            modelBuilder.Configurations.Add(new OperWorkflowConfiguration());
            modelBuilder.Configurations.Add(new ProcessConfiguration());
            modelBuilder.Configurations.Add(new TransitLinkedPersonsConfiguration());
            modelBuilder.Configurations.Add(new TransitDelinquencyConfiguration());

        }
    }
}
