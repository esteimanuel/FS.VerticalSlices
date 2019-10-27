namespace FS.VerticalSlices.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open FS.VerticalSlices.Features
open FS.VerticalSlices.Infrastructure
open FS.VerticalSlices.Data

[<TestClass>]
type OrdersCreate () =

    [<TestMethod>]
    member _.TestMethodPassing () =
        use ctx = createInMemoryShopContext()

        let createCommand : Orders.Create.Command = { 
            OrderDate = DateTime.Now; 
            OrderNumber = "myordernumber"; 
            Lines = [{ Number = 1; ProductId = 834675; Quantity = 3; }];
        }

        let createHandler = Orders.Create.Handler ctx
        let createResult = createHandler.Handle createCommand

        Assert.IsTrue(createResult |> resultIsOk);

        let detailsQuery: Orders.Details.Query = { Id = 1 }
        let detailsHandler = Orders.Details.Handler ctx
        let detailsResult = detailsHandler.Handle detailsQuery

        Assert.IsTrue(detailsResult |> resultIsOk);

        let orderDetails = match detailsResult with | Ok result -> result
        Assert.AreEqual(createCommand.OrderDate, orderDetails.OrderDate)
        Assert.AreEqual(createCommand.OrderNumber, orderDetails.OrderNumber)
        let commandLine1 = createCommand.Lines |> Seq.head
        let detailsLine1 = orderDetails.Lines |> Seq.head
        Assert.AreEqual(commandLine1.Number, detailsLine1.Number)
        Assert.AreEqual(commandLine1.ProductId, detailsLine1.ProductId)
        Assert.AreEqual(commandLine1.Quantity, detailsLine1.Quantity)

