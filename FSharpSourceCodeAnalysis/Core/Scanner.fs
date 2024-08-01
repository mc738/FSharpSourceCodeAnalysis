namespace FSharpSourceCodeAnalysis.Core

open FSharp.Compiler.Syntax
open FSharp.Compiler.Text

module Scanner =

    type AnalyzerRule =
        { Id: string
          Name: string
          Description: string
          Condition: Condition }

    and Condition =
        | HasAttribute of AttributeCondition
        | Not of Condition
        | And of Condition * Condition
        | Or of Condition * Condition
        | Any of Condition list
        | All of Condition list

    and AttributeCondition = { AttributeName: string }

    type BindingWatcher =
        { Condition: BindingWatcherCondition
          Rules: AnalyzerRule list }

    and BindingWatcherCondition =
        | HasAttribute of AttributeCondition
        | Bespoke of Fn: (SynBinding -> bool)
        | Not of BindingWatcherCondition
        | And of BindingWatcherCondition * BindingWatcherCondition
        | Or of BindingWatcherCondition * BindingWatcherCondition
        | Any of BindingWatcherCondition list
        | All of BindingWatcherCondition list

        member bwc.Test(binding: SynBinding) =
            match bwc with
            | HasAttribute attributeCondition ->
                match binding with
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
                    attributes
                    |> List.exists (fun a ->
                        a.Attributes
                        |> List.exists (fun sa -> sa.TypeName.LongIdent[0].idText = attributeCondition.AttributeName))
            | Bespoke fn -> fn binding
            | Not bindingWatcherCondition -> bindingWatcherCondition.Test binding |> not
            | And(a, b) -> a.Test binding && b.Test binding
            | Or(a, b) -> a.Test binding || b.Test binding
            | Any conditions -> conditions |> List.exists (fun c -> c.Test binding)
            | All conditions -> conditions |> List.forall (fun c -> c.Test binding)

    type DeclarationType =
        | ModuleAbbrev
        | NestedModule
        | Let
        | Expr
        | Types
        | Exception
        | Open
        | Attributes
        | HashDirective
        | NamespaceFragment

    type WatchedBinding =
        { Name: string
          Range: SourceRange
          RawBinding: SynBinding }

    and AnalyzerRuleResult = { RuleId: string; Range: SourceRange }

    and SourceRange =
        { StartLine: int
          StartColumn: int
          EndLine: int
          EndColumn: int }

        static member FromRange(range: Range) =
            { StartLine = range.Start.Line
              StartColumn = range.Start.Column
              EndLine = range.End.Line
              EndColumn = range.End.Column }

    type ScannerState =
        { WatchedBindings: WatchedBinding list }

        static member Empty() = { WatchedBindings = [] }

    type ScannerSettings =
        { BindingWatchers: BindingWatcher list }

    let handleBinding (settings: ScannerSettings) (state: ScannerState) (binding: SynBinding) =
        // Check if any og the bindings require watching.

        let newState =
            if settings.BindingWatchers |> List.exists (fun bw -> bw.Condition.Test binding) then
                match binding with
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
                    
                    let newWatcher =
                        { Name = ""
                          Range = SourceRange.FromRange binding.RangeOfHeadPattern
                          RawBinding = binding }

                    { state with
                        WatchedBindings = newWatcher :: state.WatchedBindings }
            else
                state

        newState

    let handleDeclaration (state: ScannerState) (settings: ScannerSettings) (declaration: SynModuleDecl) =
        match declaration with
        | SynModuleDecl.ModuleAbbrev(ident, longId, range) -> state
        | SynModuleDecl.NestedModule(moduleInfo, isRecursive, decls, isContinuing, range, trivia) -> state
        | SynModuleDecl.Let(isRecursive, bindings, range) -> bindings |> List.fold (handleBinding settings) state
        | SynModuleDecl.Expr(expr, range) -> state
        | SynModuleDecl.Types(typeDefns, range) -> state
        | SynModuleDecl.Exception(exnDefn, range) -> state
        | SynModuleDecl.Open(target, range) -> state
        | SynModuleDecl.Attributes(attributes, range) -> state
        | SynModuleDecl.HashDirective(hashDirective, range) -> state
        | SynModuleDecl.NamespaceFragment fragment -> state

    let run (settings: ScannerSettings) (declarations: SynModuleDecl list) =
        let state = ScannerState.Empty




        ()
