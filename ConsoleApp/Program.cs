


using DAL;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

var contextOptions = new DbContextOptionsBuilder<Context>()
    .UseSqlServer(@"Server=.\SqlExpress;Database=EF6Core;Integrated security=true")
    //śledzenie zmian przez proxy
    //.UseChangeTrackingProxies()
    //LazyLoading
    .UseLazyLoadingProxies()
    .LogTo(x => Console.WriteLine(x))
    .Options;

using (var context = new Context(contextOptions))
{
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}

Transactions(contextOptions);

using (var context = new Context(contextOptions))
{
    var product = context.Set<Product>().First();

    product.Manufacturer = new Manufacturer() { Name = "Altkom" };

    product = context.Set<Product>().Skip(1).First();
    context.Entry(product).Property("ManufacturerId").CurrentValue = 1;
    context.SaveChanges();

    var products = context.Set<Product>().Where(x => EF.Property<int?>(x, "ManufacturerId") != null).ToList();

}

GlobalFilters(contextOptions);


    static void ChangeTacker(Context context)
{
    var order = new Order();
    var product = new Product() { Name = "Marchewka" };
    order.Products.Add(product);
    //order.Products.Add(context.CreateProxy<Product>(x => x.Name = "Marchewka"));


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


    for (int i = 0; i < 3; i++)
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
}

static void ConcurrencyToken(DbContextOptions<Context> contextOptions)
{
    using (var context = new Context(contextOptions))
    {
        var product = new Product() { Name = "Marchewka" };
        context.Add(product);
        context.SaveChanges();
        //context.ChangeTracker.Clear();
    }

    using (var context = new Context(contextOptions))
    {
        var product = context.Set<Product>().First();

        product.Price += 10;
        //product.Name = "abc";

        var saved = false;
        while (!saved)
        {
            try
            {
                context.SaveChanges();
                saved = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    //wartości stanu jaki chcemy wprowadzić do bazy danych
                    var currentValues = entry.CurrentValues;
                    //wartości, które aktualnie znajdują się w bazie danych
                    var databaseValues = entry.GetDatabaseValues();
                    //wartości, które zostały pobrane z bazy
                    var originalValues = entry.OriginalValues;


                    switch (entry.Entity)
                    {
                        case Product:
                            {
                                var property = currentValues.Properties.SingleOrDefault(x => x.Name == nameof(Product.Price));
                                var currentPricePropertyValue = (float)currentValues[property];
                                var databasePricePropertyValue = (float)databaseValues[property];
                                var originalPricePropertyValue = (float)originalValues[property];

                                currentValues[property] = databasePricePropertyValue + (currentPricePropertyValue - originalPricePropertyValue);

                                //aktualizacja wartości oryginalnych do zgodności z wartościami w brazie danych w celu aktualizacji tokena konkurencyjności
                                entry.OriginalValues.SetValues(databaseValues);
                                break;
                            }

                        case Order:
                            break;
                    }
                }

            }
        }



    }
}

static void Transactions(DbContextOptions<Context> contextOptions)
{
    var products = Enumerable.Range(100, 50).Select(x => new Product { Name = $"Product {x}"/*, Price = 1.23f * x*/ }).ToList();
    var orders = Enumerable.Range(0, 5).Select(x => new Order() { DateTime = DateTime.Now.AddMinutes(-3.21 * x), OrderType = (OrderTypes)(x % 3) }).ToList();

    using (var context = new Context(contextOptions))
    {
        using (var transaction = context.Database.BeginTransaction(/*System.Data.IsolationLevel.Serializable*/))
        {

            //context.RandomFail = true;


            for (int i = 0; i < 5; i++)
            {
                string savepoint = i.ToString();
                transaction.CreateSavepoint(savepoint);

                try
                {
                    var subProducts = products.Skip(i * 10).Take(10).ToList();

                    foreach (var product in subProducts)
                    {
                        context.Add(product);
                        context.SaveChanges();
                    }

                    orders[i].Products = subProducts;
                    context.Add(orders[i]);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    //wycofanie zmian
                    //transaction.Rollback();
                    //wyucofanie zmian do wskazanego savepointa
                    transaction.RollbackToSavepoint(savepoint);
                    context.ChangeTracker.Clear();
                }

            }


            /*var dbProducts = context.Set<Product>().ToList();
            if (dbProducts.Any())
            {
                dbProducts.First().Name = "Jabłko";
                context.SaveChanges();
            }*/

            transaction.Commit();
        }

    }
}

static void Loading(DbContextOptions<Context> contextOptions)
{
    Order order;
    using (var context = new Context(contextOptions))
    {
        //Eager loading
        //order = context.Set<Order>().Include(x => x.Products).ThenInclude(x => x.Orders).First();

        //Explicit loading
        //order = context.Set<Order>().First();
        order = new Order() { Id = 1 };
        context.Attach(order);
        //context.Entry(order).Property(x => x.<property z referencją>).Load();
        context.Entry(order).Collection(x => x.Products).Load();

        DoSthWithOrderProducts(order);
    }

    using (var context = new Context(contextOptions))
    {
        context.Set<Order>().Load();
        context.Set<Product>().Load();
        context.Set<Dictionary<string, object>>("OrderProduct").Load();

        order = context.Set<Order>().First();
    }

    //Lazy loading
    using (var context = new Context(contextOptions))
    {
        order = context.Set<Order>().First();

        DoSthWithOrderProducts(order);
    }
    using (var context = new Context(contextOptions))
    {
        var product = context.Set<Product>().First();

        product.Orders.ToList();
    }


    void DoSthWithOrderProducts(Order order)
    {
        order.Products.ToList();
    }
}

static void GlobalFilters(DbContextOptions<Context> contextOptions)
{
    using (var context = new Context(contextOptions))
    {
        var order = context.Set<Order>().First();
        //order.IsDeleted = true;
        context.Entry(order).Property("IsDeleted").CurrentValue = true;
        context.SaveChanges();
    }

    using (var context = new Context(contextOptions))
    {
        var order = context.Set<Order>()/*.Where(x => !x.IsDeleted)*/.First();

        var product = context.Set<Product>().Include(x => x.Orders/*.Where(x => !x.IsDeleted)*/).First();

        context.ChangeTracker.Clear();

        context.Attach(product);
        context.Entry(product).Collection(x => x.Orders).Load();

    }
}