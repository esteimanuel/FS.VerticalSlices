module FS.VerticalSlices.Features.Orders.Create

open System
open FS.VerticalSlices.Models
open FS.VerticalSlices.Data

type [<CLIMutable>] Command = {
    OrderDate: DateTime
    OrderNumber: string
    Lines: Line seq 
}

and Line = {
    Number: int
    ProductId: ProductId
    Quantity: int
}

type Handler(ctx: ShopContext) =
    member _.Handle(request) =
        let lineToEntity = fun (line: Line) -> { 
            OrderId = 0; 
            Number = line.Number; 
            ProductId = line.ProductId; 
            UnitPrice = 0m; 
            Quantity = line.Quantity
        }

        let entity = { 
            Id = 0
            OrderDate = request.OrderDate
            OrderNumber = request.OrderNumber
            CustomerId = 0
            Lines = request.Lines |> Seq.map lineToEntity 
        } 

        ctx.Orders.Add(entity) |> ignore

        if ctx.SaveChanges() = 1 
        then Ok "Order created"
        else Error "Failed to create order"