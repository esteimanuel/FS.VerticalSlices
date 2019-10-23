module FS.VerticalSlices.Infrastructure

let nullToOption x = if box x = null then None else Some x
let resultIsOk result = match result with Ok _ -> true | Error _ -> false
let resultIsError result = not (resultIsOk result)