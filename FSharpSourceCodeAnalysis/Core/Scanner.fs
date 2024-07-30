namespace FSharpSourceCodeAnalysis.Core

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

    type WatchedBinding =
        {
            Name: string
            
        }
        
    and WatchedBindingType =
        | Function
        | Method
    
    type AnalyzerRuleResult = { RuleId: string; Range: SourceRange }

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
