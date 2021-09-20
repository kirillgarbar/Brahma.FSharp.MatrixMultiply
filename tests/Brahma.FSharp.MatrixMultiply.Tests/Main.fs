namespace Brahma.FSharp.MatrixMultiply.Tests

module ExpectoTemplate =

    open Expecto

    [<EntryPoint>]
    let main argv =
        Tests.runTestsInAssembly { Impl.ExpectoConfig.defaultConfig with fsCheckMaxTests = 20 } argv
