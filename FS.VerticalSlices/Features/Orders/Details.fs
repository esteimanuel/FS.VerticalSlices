module FS.VerticalSlices.Features.Orders.Details

open System
open FS.VerticalSlices.Infrastructure
open FS.VerticalSlices.Models
open FS.VerticalSlices.Data

type [<CLIMutable>] Query = { Id: int }

type Model = {
    OrderDate: DateTime
    OrderNumber: string
    TotalAmount: decimal
    Lines: Line seq 
}

and Line = {
    Number: int
    ProductId: ProductId
    UnitPrice: decimal
    Quantity: int
}

type Handler(ctx: ShopContext) =
    member _.Handle(request) =
        let lineEntityToModel = 
            fun (line: FS.VerticalSlices.Models.Line) -> {
                Number = line.Number
                ProductId = line.ProductId
                UnitPrice = line.UnitPrice 
                Quantity = line.Quantity
            }
                
        let entity = 
            query {
                for order in ctx.Orders do 
                    where (order.Id = request.Id)
                    select order
                    exactlyOneOrDefault 
            } |> nullToOption

        let countLineItemPrices = 
            fun (lines: FS.VerticalSlices.Models.Line seq) -> 
                lines |> Seq.sumBy (fun line -> line.UnitPrice)

        match entity with
        | Some order -> Ok { 
                OrderNumber = order.OrderNumber 
                OrderDate = order.OrderDate
                TotalAmount = order.Lines |> countLineItemPrices
                Lines = order.Lines |> Seq.map lineEntityToModel
            }
        | None -> Error "order not found"
    