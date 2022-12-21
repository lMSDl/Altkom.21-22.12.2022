


using DAL;
using Microsoft.EntityFrameworkCore;

var contextOptions = new DbContextOptionsBuilder<Context>()
    .UseSqlServer(@"Server=.\SqlExpress;Database=EF6Core;Integrated security=true")
    .Options;

var context = new Context(contextOptions);

context.Database.EnsureDeleted();
context.Database.EnsureCreated();