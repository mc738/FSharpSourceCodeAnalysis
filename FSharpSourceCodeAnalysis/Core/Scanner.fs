﻿namespace FSharpSourceCodeAnalysis.Core

module Scanner =
    
    type WatchCondition =
        | String


    type Condition =
        | HasAttribute
        | Not of Condition
        | Any of Condition list
        | All of Condition list

    and AttributeCondition =
        {
            AttributeName: string
            
        }
    