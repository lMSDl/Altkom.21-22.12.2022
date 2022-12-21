using DAL.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class Context : DbContext
    {
        public Context()
        {
        }
        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new OrderConfiguration());
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());

            //zaczytujemy wszystkie pliki konfiguracyjne ze wskazanego assembly
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            //włączenie śledzenia zmian przez notyfikacje
            //modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangedNotifications);
        }

        public bool RandomFail { get; set; }
        public override int SaveChanges()
        {
            if(RandomFail && new Random().Next(1, 25) == 1)
            {
                throw new Exception();
            }

            return base.SaveChanges();
        }
    }
}