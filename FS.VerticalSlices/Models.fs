module FS.VerticalSlices.Models

open System
open System.Collections.Generic

type OrderId = int
type CustomerId = int
type ProductId = int
type SupplierId = int
type OrderNumber = string
type LineNumber = int
type [<CLIMutable>] Order = { 
    Id: OrderId
    OrderDate: DateTime
    OrderNumber: OrderNumber
    CustomerId: CustomerId
    Lines: ICollection<Line>
} 

and [<CLIMutable>] Line = {
    OrderId: OrderId
    Number: LineNumber
    ProductId: ProductId
    UnitPrice: decimal
    Quantity: int
}

type [<CLIMutable>] Customer = { 
    Id: CustomerId
    FirstName: string
    LastName: string
    City: string
    Country: string
    Phone: string
}

type [<CLIMutable>] Product = {
    Id: ProductId
    ProductName: string
    SupplierId: SupplierId
    UnitPrice: decimal
    Package: string
    IsDiscontinued: bool
}

type [<CLIMutable>] Supplier = {
    Id: SupplierId
    CompanyName: string
    ContactName: string
    ContactTitle: string
    City: string
    Country: string
    Phone: string
    Fax: string
}