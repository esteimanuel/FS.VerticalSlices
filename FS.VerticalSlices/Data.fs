module FS.VerticalSlices.Data

open FS.VerticalSlices.Models
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.Logging
open Microsoft.EntityFrameworkCore.Metadata.Builders;
open Microsoft.Data.Sqlite
open System.Collections.Generic

type ShopContext =
    inherit DbContext
    
    new() = { inherit DbContext() }
    new(options: DbContextOptions<ShopContext>) = { inherit DbContext(options) }

    override _.OnModelCreating modelBuilder = 

        modelBuilder.Entity<Order>(fun builder -> 
            builder.ToTable("Order", "dbo").HasKey(fun order -> order.Id :> obj) |> ignore

            builder.OwnsMany(
                (fun order -> order.Lines :> IEnumerable<Line>), 
                (fun (ob: OwnedNavigationBuilder<Order,Line>) -> 
                    ob.ToTable("OrderLine", "dbo").HasKey(fun line -> line.OrderId :> obj) |> ignore
                    ob.WithOwner()
                        .HasForeignKey(fun line -> line.OrderId :> obj)
                        .HasPrincipalKey(fun order -> order.Id :> obj) |> ignore
                )
            ) |> ignore            
        ) |> ignore

        modelBuilder.Entity<Customer>().ToTable("Customer", "dbo").HasKey(fun customer -> customer.Id :> obj) |> ignore

        modelBuilder.Entity<Product>().ToTable("Product", "dbo").HasKey(fun product -> product.Id :> obj) |> ignore
        
        modelBuilder.Entity<Supplier>().ToTable("Supplier", "dbo").HasKey(fun supplier -> supplier.Id :> obj) |> ignore
    
    [<DefaultValue>] val mutable orders:DbSet<Order>
    member x.Orders with get() = x.orders and set v = x.orders <- v

    [<DefaultValue>] val mutable customers:DbSet<Customer>
    member x.Customers with get() = x.customers and set v = x.customers <- v 

    [<DefaultValue>] val mutable products:DbSet<Product>
    member x.Products with get() = x.products and set v = x.products <- v

    [<DefaultValue>] val mutable suppliers:DbSet<Supplier>
    member x.Suppliers with get() = x.suppliers and set v = x.suppliers <- v 
    
let configureSqlServerContext (connectionString: string) = 
    let optionsBuilder = new DbContextOptionsBuilder<ShopContext>();
    let logger = new LoggerFactory()
    optionsBuilder
        .UseLoggerFactory(logger)
        .UseSqlServer(connectionString) |> ignore
    new ShopContext(optionsBuilder.Options)
        
let createInMemoryShopContext() = 
    let optionsBuilder = new DbContextOptionsBuilder<ShopContext>();
    let logger = new LoggerFactory()    
    let connection = new SqliteConnection("DataSource=:memory:")

    connection.Open()
    
    optionsBuilder
        .UseLoggerFactory(logger)
        .UseSqlite(connection)
        .EnableSensitiveDataLogging() |> ignore

    let context = new ShopContext(optionsBuilder.Options)
    context.Database.EnsureCreated() |> ignore
    context