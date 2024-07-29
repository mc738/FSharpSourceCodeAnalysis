namespace FSharpSourceCodeAnalysis.Examples

open FSharpSourceCodeAnalysis.Core.Attributes

module Example1 =


    [<Vulnerable>]
    let myFunction (input: string) = ()

    [<Sanitizer>]
    let sanitizerFunction (input: string) = input


    let testSafeFunction () =
        // This function is safe because we have control over the input.
        let input = "Safe"

        myFunction input

    let testUnsafeFunction (input: string) =
        // This function is unsafe because we do not have control over the input.
        myFunction input

    let testSanitizerFunction (input: string) =
        // This is safe because we pass the input into a function marked as `Sanitizer`,
        // before passing it into the `Vulnerable` function.
        sanitizerFunction input |> myFunction

    ()
