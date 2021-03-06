module Mod

open System.IO
open System.Runtime.CompilerServices

[<Extension>]
type FileExt =
    [<Extension>]
    static member CreateDirectory (fileInfo: FileInfo, safe: bool, s: string) =
            Directory.CreateDirectory fileInfo.Directory.FullName

let x = FileInfo "abc.txt"
{selstart}x.CreateDirectory (true, "hi"){selend}
