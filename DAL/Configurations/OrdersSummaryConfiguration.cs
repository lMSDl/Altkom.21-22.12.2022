using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    internal class OrdersSummaryConfiguration : IEntityTypeConfiguration<OrdersSummary>
    {
        public void Configure(EntityTypeBuilder<OrdersSummary> builder)
        {
            //builder.ToTable(name: null);
            builder.ToView("View_OrdersSummary");
        }
    }
}
