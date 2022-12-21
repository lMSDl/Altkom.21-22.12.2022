


using DAL;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.ObjectModel;

var contextOptions = new DbContextOptionsBuilder<Context>()
    .UseSqlServer(@"Server=.\SqlExpress;Database=EF6Core;Integrated security=true")
    //śledzenie zmian przez proxy
    .UseChangeTrackingProxies()
    .Options;

var context = new Context(contextOptions);

context.Database.EnsureDeleted();
context.Database.EnsureCreated();


var order = context.CreateProxy<Order>();
var product = new Product() { Name = "Marchewka"};
//order.Products.Add(product);
order.Products.Add(context.CreateProxy<Product>(x => x.Name = "Marchewka"));


Console.WriteLine("Order: " + context.Entry(order).State);
Console.WriteLine("Product: " + context.Entry(product).State);

context.Attach(order);
Console.WriteLine("Order: " + context.Entry(order).State);
Console.WriteLine("Product: " + context.Entry(product).State);

context.Add(order);
Console.WriteLine("Order: " + context.Entry(order).State);
Console.WriteLine("Product: " + context.Entry(product).State);
context.SaveChanges();
Console.WriteLine("Order: " + context.Entry(order).State);
Console.WriteLine("Product: " + context.Entry(product).State);

order.DateTime = DateTime.Now;
Console.WriteLine("Order: " + context.Entry(order).State);
Console.WriteLine("Product: " + context.Entry(product).State);
context.SaveChanges();
Console.WriteLine("Order: " + context.Entry(order).State);
Console.WriteLine("Product: " + context.Entry(product).State);

context.Remove(order);
Console.WriteLine("Order: " + context.Entry(order).State);
Console.WriteLine("Product: " + context.Entry(product).State);
context.SaveChanges();
Console.WriteLine("Order: " + context.Entry(order).State);
Console.WriteLine("Product: " + context.Entry(product).State);

order.DateTime = DateTime.Now;
Console.WriteLine("Order: " + context.Entry(order).State);
Console.WriteLine("Product: " + context.Entry(product).State);


for(int i =0; i < 3; i ++)
{
    order = new Order() { DateTime = DateTime.Now.AddMinutes(-i * 32) };
    order.Products = new ObservableCollection<Product>(Enumerable.Range(1, new Random(i).Next(3, 10)).Select(x => new Product { Name = x.ToString(), Price = x }).ToList());

    context.Add(order);
}

Console.WriteLine(context.ChangeTracker.DebugView.ShortView);
Console.WriteLine("------------");
Console.WriteLine(context.ChangeTracker.DebugView.LongView);
context.SaveChanges();
Console.WriteLine("------------");
Console.WriteLine(context.ChangeTracker.DebugView.LongView);

order.DateTime = DateTime.Now;
order.Products.First().Name = "aaa";

Console.WriteLine("------------");
Console.WriteLine(context.ChangeTracker.DebugView.ShortView);

//context.ChangeTracker.DetectChanges();
Console.WriteLine("------------");
Console.WriteLine(context.ChangeTracker.DebugView.ShortView);

context.Entry(order.Products.Skip(1).First()).Property(x => x.Name).CurrentValue = "bbb";
Console.WriteLine("------------");
Console.WriteLine(context.ChangeTracker.DebugView.LongView);

context.ChangeTracker.AutoDetectChangesEnabled = false;

//AutoDetectChanges - działa w takich przypadkach jak pobranie Entries, pobranie Local albo wywołanie SaveChanges(Async)
context.ChangeTracker.Entries();
_ = context.Set<Product>().Local;
Console.WriteLine("------------");
Console.WriteLine(context.ChangeTracker.DebugView.ShortView);
Console.WriteLine("------------");
Console.WriteLine(context.Entry(order.Products.First()).State);


order.Products.Skip(2).First().Price = 12;
Console.WriteLine("------------");
Console.WriteLine(context.ChangeTracker.DebugView.ShortView);

order.DateTime = DateTime.Now;
Console.WriteLine("------------");
Console.WriteLine(context.ChangeTracker.DebugView.ShortView);

context.SaveChanges();