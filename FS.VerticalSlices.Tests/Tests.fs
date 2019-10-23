namespace FS.VerticalSlices.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open FS.VerticalSlices.Features
open FS.VerticalSlices.Infrastructure
open FS.VerticalSlices.Data

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member _.TestMethodPassing () =
        use ctx = createInMemoryShopContext()

        let createCommand : Orders.Create.Command = { 
            OrderDate = DateTime.Now; 
            OrderNumber = "myordernumber"; 
            Lines = [{ Number = 1; ProductId = 1; Quantity = 1; }];
        }

        let createHandler = Orders.Create.Handler ctx
        let create = createHandler.Handle createCommand

        Assert.IsTrue(create |> resultIsOk);

        let detailsQuery: Orders.Details.Query = { Id = 1 }
        let detailsHandler = Orders.Details.Handler ctx
        let details = detailsHandler.Handle detailsQuery

        Assert.IsTrue(details |> resultIsOk);
