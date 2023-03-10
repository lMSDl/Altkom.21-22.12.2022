using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    internal class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable(x => x.IsTemporal(
                xx =>
                {
                    xx.HasPeriodStart("Start");
                    xx.HasPeriodEnd("End");
                }
                ));

            builder.Ignore(x => x.Value);
        }
    }
}
