namespace Brahma.FSharp.MatrixMultiply

module MatrixMultiply =

    open OpenCL.Net
    open Brahma.OpenCL
    open Brahma.FSharp.OpenCL.Core
    open Brahma.FSharp.OpenCL.Extensions
        
    let multiply platformName (m1:float[]) m1Rows m1Cols (m2:float[]) m2Rows m2Cols =
        if m1Cols <> m2Rows then failwith "Wrong size of matrices"
    
        let command = 
            <@
                fun (r:_2D) (a:array<_>) (b:array<_>) (c:array<_>) -> 
                    let tx = r.GlobalID0
                    let ty = r.GlobalID1
                    let mutable buf = 0.0
                    for k in 0 .. m1Cols - 1 do
                        buf <- buf + (a.[ty * m1Cols + k] * b.[k * m2Cols + tx])
                    c.[ty * m2Cols + tx] <- buf
            @>

        let provider =
            try  ComputeProvider.Create(platformName, DeviceType.Default)
            with 
            | ex -> failwith ex.Message

        let mutable commandQueue = new CommandQueue(provider, provider.Devices |> Seq.head)
        let _, kernelPrepare, kernelRun = provider.Compile command
        let dim = _2D(m2Cols, m1Rows)
        let m3 = Array.zeroCreate(m1Rows * m2Cols)
        kernelPrepare dim m1 m2 m3
        
        commandQueue.Add(kernelRun()).Finish() |> ignore
        commandQueue.Add(m3.ToHost provider).Finish() |> ignore
        commandQueue.Dispose()
        provider.CloseAllBuffers()
        provider.Dispose()    

        m3
