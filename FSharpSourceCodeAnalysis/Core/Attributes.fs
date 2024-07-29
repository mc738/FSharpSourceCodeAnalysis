namespace FSharpSourceCodeAnalysis.Core

module Attributes =

    type VulnerableAttribute() =
        inherit System.Attribute()

    type SanitizerAttribute() =
        inherit System.Attribute()
