namespace Brahma.FSharp.MatrixMultiply.Tests

open Expecto
open Brahma.FSharp.MatrixMultiply
open MatrixIO

module Tests =

    let random = new System.Random()
    
    let genRandomMatrix rows cols =
        Array.init (rows * cols) (fun i -> float (random.Next(100)))
    
    let multiplyStandard (a:array<_>) aRows aCols (b:array<_>) bRows bCols =
        let c = Array.zeroCreate (aRows * bCols)
        for i in 0 .. aRows - 1 do
            for j in 0 .. bCols - 1 do
                let mutable buf = 0.0
                for k in 0 .. aCols - 1 do
                     buf <- buf + a.[i * aCols + k] * b.[k * bCols + j]
                c.[i * bCols + j] <- buf
        c  
        
    [<Tests>]
    let matrixMultiplyTests =
        testList "MatrixMultiplyTests" [
            testProperty "Multiplying matrices" <| fun (a, b, c) ->
                let rows = abs(a) % 64 + 1
                let cols = abs(b) % 64 + 1
                let colsrows = abs(c) % 64 + 1
                let m1 = genRandomMatrix rows colsrows
                let m2 = genRandomMatrix colsrows cols
                let m3 = MatrixMultiply.multiply "*" m1 rows colsrows m2 colsrows cols
                let m3Standard = multiplyStandard m1 rows colsrows m2 colsrows cols
                Expect.equal m3Standard m3 "Mupltiply is wrong"

            testCase "Wrong sized matrices multiply attempt" <| fun _ ->
                let r1 = 5
                let c1 = 10
                let r2 = 8
                let c2 = 7
                let m1 = genRandomMatrix r1 c1
                let m2 = genRandomMatrix r2 c2
                Expect.throws (fun _ -> MatrixMultiply.multiply "NVIDIA*" m1 r1 c1 m2 r2 c2 |> ignore) "Exception should be raised"
        ]
    [<Tests>]
    let matrixIOTests =
        testList "MatrixIOTests" [
            testProperty "writeMatrix and readMatrix id test" <| fun (a, b) ->
                let rows = abs(a) % 64 + 1
                let cols = abs(b) % 64 + 1
                let matrix = genRandomMatrix rows cols
                writeMatrix "writeAndReadMatrixTest.txt" matrix rows cols
                let readMatrix = readMatrix "writeAndReadMatrixTest.txt"
                Expect.equal (matrix, rows, cols) readMatrix "write and read should be id"

            testCase "NonRectangular matrix read attempt" <| fun _ ->
                let matrix = "1.0 1.0 1.0\n1.0 1.0".Split("\n")
                System.IO.File.WriteAllLines("nonRectangularReadTest.txt", matrix)
                Expect.throws (fun _ -> readMatrix "nonRectangularReadTest.txt" |> ignore) "Exception should be raised"

            testCase "Wrong format matrix read attempt" <| fun _ ->
                let matrix = "a b c\n1.0 1.0".Split("\n")
                System.IO.File.WriteAllLines("wrongFormatReadTest.txt", matrix)
                Expect.throws (fun _ -> readMatrix "wrongFormatReadTest.txt" |> ignore) "Exception should be raised"

            testCase "Wrong sized matrix for write given" <| fun _ ->
                let matrix = [| 1.0; 1.0; 1.0; 1.0; 1.0 |]
                Expect.throws (fun _ -> writeMatrix "wrongSizedWriteTest.txt" matrix 2 3 |> ignore) "Exception should be raised"
        ]
