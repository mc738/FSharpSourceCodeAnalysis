open FSharp.Compiler.CodeAnalysis
open FSharp.Compiler.Syntax
open FSharp.Compiler.Text

let checker = FSharpChecker.Create()

let t =
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
        let rec test (decl: SynModuleDecl) =
            match decl with
            | SynModuleDecl.ModuleAbbrev(ident, longId, range) -> printf "mod abbrev"

            | SynModuleDecl.NestedModule(moduleInfo, isRecursive, decls, isContinuing, range, trivia) ->
                printfn "nested module"
                decls |> List.map test |> ignore

            | SynModuleDecl.Let(isRecursive, bindings, range) ->
                //bindings
                //|> List.map (fun sb ->
                //    match sb with
                //    | SynBinding(accessibility, kind, isInline, isMutable, attributes, xmlDoc, valData, headPat, returnInfo, expr, range, debugPoint, trivia) -> failwith "todo")
                
                
                printfn "let"

            | SynModuleDecl.Expr(expr, range) -> printfn "expr"
            | SynModuleDecl.Types(typeDefns, range) -> printfn "types"
            | SynModuleDecl.Exception(exnDefn, range) -> printfn "exception"
            | SynModuleDecl.Open(target, range) -> printfn "open"

            | SynModuleDecl.Attributes(attributes, range) -> printfn "attributes"
            | SynModuleDecl.HashDirective(hashDirective, range) -> printfn "has directive"
            | SynModuleDecl.NamespaceFragment fragment -> printfn "namespace fragment"


        match fileResults.ParseTree with
        | ParsedInput.ImplFile parsedImplFileInput ->
            parsedImplFileInput.Contents
            |> List.map (fun c ->
                match c with
                | SynModuleOrNamespace.SynModuleOrNamespace(longId,
                                                            isRecursive,
                                                            kind,
                                                            decls,
                                                            xmlDoc,
                                                            attribs,
                                                            accessibility,
                                                            range,
                                                            trivia) ->

                    decls |> List.map test)

        //failwith "todo"
        | ParsedInput.SigFile parsedSigFileInput -> failwith "todo"

//failwith "todo"


// For more information see https://aka.ms/fsharp-console-apps
printfn "Hello from F#"
