namespace Generator.Literals

[<AutoOpen>]
module Auto = 
    let inline (^) f x = f x

module Namespaces =
    let root = "PhotoTour"

module Types =
    let card = "AttractionCard"
    let attraction = "Attraction"
    let _Attraction transport = $"%s{transport}{attraction}"
    let _Crossroad transport = $"%s{transport}Crossroad"
    let attractionCrossroad = "AttractionCrossroad"
    let untypedViaAttraction = $"Untyped{attractionCrossroad}"
    let _Route transport = $"%s{transport}Route"
    let attractionRoute = "AttractionRoute"

    module Attraction =
        let instance = "instance"
        let via_ transport = $"Via%s{transport}"

    module Crossroad =
        let instance = Attraction.instance

    module Route =
        let startRoute = "StartRoute"
        let initalState = "initialState"
        let updater = "updater"
        let State = "State"
        let Updater = "Updater"

module Modules =
    let cards = "AttractionCards"
    let attraction = "Attraction"
    let attractions = "Attractions"
    let _RelationExts transport = $"%s{transport}RelationExts"
    let _Crossroad transport = $"%s{transport}Crossroad"
    let _CrossroadAuto transport = $"{_Crossroad transport}Auto"
    let _Route transport = $"%s{transport}Route"
    let _RouteAuto transport = $"{_Route transport}Auto"

module Names =
    let hayway = "Hayway"
    let railroad = "Railroad"
    let flight = "Flight"

module Messages =
    let generated = " Generated."

module Syntaxify =
    let main str = 
        str
        |> Seq.mapFold (fun needUp char ->
            if System.Char.IsLetterOrDigit char then
                if needUp 
                then System.Char.ToUpper char, false
                else char, false
            else
                ' ', true
        ) true
        |> fst
        |> Seq.filter ^ function
            | ' ' -> false
            | _ -> true
        |> Array.ofSeq
        |> System.String

    let inline private (|HasName|) (hasName : 'a when 'a:(member Name : string))  =
        hasName.Name

    let inline (|Name|) (HasName name) = main name
    let inline name (HasName name) = main name