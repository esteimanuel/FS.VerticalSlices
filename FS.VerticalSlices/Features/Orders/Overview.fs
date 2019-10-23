module FS.VerticalSlices.Features.Orders.Overview

open FS.VerticalSlices.Models
open FS.VerticalSlices.Data

type [<CLIMutable>] Query = { 
    PageIndex: int
    PageSize: int
     }

type Page = {
    Index: int
    Size: int
    Total: int
    HasNext: bool
    HasPrevious: bool
    Orders: Order seq 
    OrdersTotal: int
}

type Handler(ctx: ShopContext) =
    member _.Handle(request) =
        let entities = 
            query {
                for order in ctx.Orders do 
                    skip (request.PageIndex * request.PageSize)
                    take request.PageSize
                    select order
            }

        let ordersTotal = ctx.Orders |> Seq.length
        let hasPrevious = request.PageIndex > 0
        let pageTotal = int (ceil (float ordersTotal / float request.PageSize))
        let hasNext = ((request.PageIndex * request.PageSize) + request.PageSize) <= ordersTotal

        let page = { 
            Index = request.PageIndex
            Size = request.PageSize
            Total = pageTotal
            HasNext = hasNext
            HasPrevious = hasPrevious
            OrdersTotal = ordersTotal
            Orders = entities }

        page
        