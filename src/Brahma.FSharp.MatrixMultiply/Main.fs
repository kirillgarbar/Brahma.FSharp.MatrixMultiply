namespace Brahma.FSharp.MatrixMultiply

open System.Reflection

module Main =

    open Argu
    open System.IO
    open MatrixMultiply
    open MatrixIO

    type Platform =
        | NVIDIA
        | AMD
        | Any

        member this.toString =
            match this with
            | NVIDIA -> "NVIDIA*"
            | AMD -> "AMD*"
            | Any -> "*"

    type CLIArguments =
        | FirstMatrix of file:string
        | SecondMatrix of file:string
        | OutputFile of file:string
        | Platform of Platform

        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | FirstMatrix _ -> "File that contains first matrix"
                | SecondMatrix _ -> "File that contains second matrixe" 
                | OutputFile _ -> "File that will contain the result of multiplication"
                | Platform _ -> "Platform for openCL. Make sure openCL is installed. Run \"clinfo\" to check your platforms. Options:\"NVIDIA\", \"AMD\", \"Any\""

    [<EntryPoint>]
    let main (argv: string array) =
        try
            let parser = ArgumentParser.Create<CLIArguments>(programName = "Brahma.FSharp.MatrixMultiply")
            let results = parser.Parse(argv)
            if results.Contains FirstMatrix && results.Contains SecondMatrix && results.Contains OutputFile && results.Contains Platform
            then
                let mrc1 = readMatrix(results.GetResult FirstMatrix)
                let mrc2 = readMatrix(results.GetResult SecondMatrix)
                let outputFile = results.GetResult OutputFile
                let platform = results.GetResult(Platform).toString
                match mrc1, mrc2 with
                | (m1, r1, c1), (m2, r2, c2) ->
                    printfn "Multiplying matrices, please wait..." 
                    let m3 = multiply platform m1 r1 c1 m2 r2 c2
                    writeMatrix outputFile m3 r1 c2
                    printfn "The task is finished" 
            else
                printfn "%s" (parser.PrintUsage())
            0
        with
        | ex ->
            printfn "%A" ex.Message
            1
