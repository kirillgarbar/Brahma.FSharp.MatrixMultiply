namespace Brahma.FSharp.MatrixMultiply

module MatrixIO =

    let readMatrix filePath =
        let input = System.IO.File.ReadAllLines(filePath)
        let string2DArray = input |> Array.map (fun s -> s.Trim().Split())
        let resultingArray = (input |> Array.map (fun s -> s.Trim()) |> String.concat " ").Split() |> Array.map (float)
        let rows = string2DArray.Length
        let cols = string2DArray.[0].Length

        let isRectangular = Array.fold (fun isTrue (x:string[]) -> isTrue && (x.Length = cols)) true string2DArray

        if isRectangular then (resultingArray, rows, cols) else failwith "Matrix should be rectangular"

    let writeMatrix filePath (matrix:float[]) rows cols =
        if matrix.Length <> rows * cols then failwith "Size of array doesn't match with given dimensions"

        let stringMatrix = Array.map string matrix
        let writableArray = Array.create rows ""

        for i in 1..rows do
            writableArray.[i - 1] <- (String.concat " " stringMatrix.[(i - 1) * cols .. i * cols - 1]).Trim()

        System.IO.File.WriteAllLines(filePath, writableArray)
