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

                match bindings[0] with
                | SynBinding(accessibility,
                             kind,
                             isInline,
                             isMutable,
                             attributes,
                             xmlDoc,
                             valData,
                             headPat,
                             returnInfo,
                             expr,
                             range,
                             debugPoint,
                             trivia) ->
                    match headPat with
                    | SynPat.Const(constant, range) -> failwith "todo"
                    | SynPat.Wild range -> failwith "todo"
                    | SynPat.Named(ident, isThisVal, accessibility, range) -> failwith "todo"
                    | SynPat.Typed(pat, targetType, range) -> failwith "todo"
                    | SynPat.Attrib(pat, attributes, range) -> failwith "todo"
                    | SynPat.Or(lhsPat, rhsPat, range, trivia) -> failwith "todo"
                    | SynPat.ListCons(lhsPat, rhsPat, range, trivia) -> failwith "todo"
                    | SynPat.Ands(pats, range) -> failwith "todo"
                    | SynPat.As(lhsPat, rhsPat, range) -> failwith "todo"
                    | SynPat.LongIdent(longDotId, extraId, typarDecls, argPats, accessibility, range) -> failwith "todo"
                    | SynPat.Tuple(isStruct, elementPats, commaRanges, range) -> failwith "todo"
                    | SynPat.Paren(pat, range) -> failwith "todo"
                    | SynPat.ArrayOrList(isArray, elementPats, range) -> failwith "todo"
                    | SynPat.Record(fieldPats, range) -> failwith "todo"
                    | SynPat.Null range -> failwith "todo"
                    | SynPat.OptionalVal(ident, range) -> failwith "todo"
                    | SynPat.IsInst(pat, range) -> failwith "todo"
                    | SynPat.QuoteExpr(expr, range) -> failwith "todo"
                    | SynPat.InstanceMember(thisId, memberId, toolingId, accessibility, range) -> failwith "todo"
                    | SynPat.FromParseError(pat, range) -> failwith "todo"

                    failwith "todo"

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
