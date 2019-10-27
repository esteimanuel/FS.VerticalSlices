module FS.VerticalSlices.Infrastructure

open System.Collections.Generic

let nullToOption x = if box x = null then None else Some x
let resultIsOk result = match result with Ok _ -> true | Error _ -> false
let resultIsError result = not (resultIsOk result)
let seqToCollection seq = seq |> Seq.toArray |> fun x -> x :> ICollection<'a>
let seqMapToCollection map seq = seq |> Seq.map map |> seqToCollection