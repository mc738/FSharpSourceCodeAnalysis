open FSharp.Compiler.CodeAnalysis
open FSharp.Compiler.Syntax
open FSharp.Compiler.Text
open FSharpSourceCodeAnalysis.Core
open FSharpSourceCodeAnalysis.Core.Scanner

let checker = FSharpChecker.Create()

let settings =
    ({ BindingWatchers =
        [ { Condition = BindingWatcherCondition.HasAttribute { AttributeName = "Vulnerable" }
            Rules =
              [ { Id = ""
                  Name = ""
                  Description = ""
                  Condition = Condition.All [] } ] } ] }
    : ScannerSettings)

let result =
    match
        checker.ParseFile(
            "C:\\Users\\44748\\Projects\\FSharpSourceCodeAnalysis\\FSharpSourceCodeAnalysis\\Examples\\Example1.fs",
            SourceText.ofString (
                System.IO.File.ReadAllText
                    "C:\\Users\\44748\\Projects\\FSharpSourceCodeAnalysis\\FSharpSourceCodeAnalysis\\Examples\\Example1.fs"
            ),
            { FSharpParsingOptions.Default with
                SourceFiles =
                    [| "C:\\Users\\44748\\Projects\\FSharpSourceCodeAnalysis\\FSharpSourceCodeAnalysis\\Examples\\Example1.fs" |] }
        )
        |> Async.RunSynchronously
    with
    | fileResults ->
        match fileResults.ParseTree with
        | ParsedInput.ImplFile parsedImplFileInput -> Scanner.run settings parsedImplFileInput
        //failwith "todo"
        | ParsedInput.SigFile parsedSigFileInput -> failwith "todo"



//failwith "todo"


// For more information see https://aka.ms/fsharp-console-apps
printfn "Hello from F#"
