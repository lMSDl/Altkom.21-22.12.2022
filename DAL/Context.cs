using DAL.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Security.Cryptography.X509Certificates;

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


            modelBuilder.Model.GetEntityTypes()
                              .SelectMany(x => x.GetProperties())
                              .Where(x => x.Name == "Key")
                              .ToList()
                              .ForEach(x =>
                              {
                                  x.IsNullable = false;
                                  x.DeclaringEntityType.SetPrimaryKey(x);
                              });

            modelBuilder.Model.GetEntityTypes()
                              .SelectMany(x => x.GetProperties())
                              .Where(x => x.PropertyInfo?.PropertyType == typeof(string))
                              .ToList()
                              .ForEach(x =>
                              {
                                  x.IsNullable = true;
                                  x.SetColumnName("s_" + x.GetColumnBaseName());
                              });

            /*modelBuilder.Model.GetEntityTypes()
                              .SelectMany(x => x.GetProperties())
                              .Where(x => x.PropertyInfo?.PropertyType == typeof(Roles))
                              .ToList()
                              .ForEach(x =>
                              {
                                  x.SetValueConverter(new EnumToStringConverter<Roles>());
                              });*/

            /*modelBuilder.Model.GetEntityTypes()
                .ToList()
                .ForEach(x =>
                {
                    x.SetTableName(x.GetDefaultTableName() + "s");
                });*/


            modelBuilder.HasSequence<int>("ProductPrice", "sequences")
                .StartsAt(100)
                .HasMax(300)
                .HasMin(10)
                .IsCyclic()
                .IncrementsBy(33);
        }


        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
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