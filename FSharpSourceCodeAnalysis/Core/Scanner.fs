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

    type BindingWatcher = { Condition: BindingWatcherCondition }

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
                        |> List.exists (fun sa ->
                            sa.TypeName.LongIdent[0].idText = attributeCondition.AttributeName))
            | Bespoke fn -> fn binding
            | Not bindingWatcherCondition -> bindingWatcherCondition.Test binding |> not
            | And(a, b) -> a.Test binding && b.Test binding
            | Or(a, b) -> a.Test binding || b.Test binding
            | Any conditions -> conditions |> List.exists (fun c -> c.Test binding)
            | All conditions -> conditions |> List.forall (fun c -> c.Test binding)

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
        
        static member Empty() = { WatchedBinding: [] }

    type ScannerSettings =
        {
          BindingWatchers: BindingWatcher list  
        }


    let run (settings: ScannerSettings) =
        
        ()