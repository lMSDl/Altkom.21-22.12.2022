using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    internal class ProductConfiguration : EntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);
            //Token współbieżności za pomocą sygnatury czasowej
            builder.Property(x => x.Timestamp).IsRowVersion();

            builder.Property(x => x.Info).HasComputedColumnSql("[s_Name] + ' ' + Str([Price]) + 'zł'", stored: true);
        }
    }
}
