namespace FSharpSourceCodeAnalysis.Core

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

    type AnalyzerRuleResult = { RuleId: string; Range: SourceRange }

    and SourceRange =
        { StartLine: int
          StartColumn: int
          EndLine: int
          EndColumn: int }
        
        
