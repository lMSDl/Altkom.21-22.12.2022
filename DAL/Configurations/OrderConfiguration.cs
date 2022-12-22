using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    internal class OrderConfiguration : EntityConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            //Token współbieżności
            builder.Property(x => x.DateTime).IsConcurrencyToken();

            builder.Property(x => x.OrderType)/*.HasConversion(x => x.ToString(),
                                                             x => Converter(x))*/
                                                //.HasConversion(new EnumToStringConverter<OrderTypes>());
                                                .HasConversion<string>();
                }

        OrderTypes Converter(string input)
        {
            return Enum.TryParse<OrderTypes>(input, out var result) ? result : OrderTypes.Unknown;
        }
    }
}
