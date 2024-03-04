namespace PhotoTour


type Attraction (card : AttractionCard) =
    member this.Card = card


/// Generated.
module Attraction =
    type ЗаповедникБасеги private () =
        inherit Attraction(AttractionCards.ЗаповедникБасеги)
        static member val instance = ЗаповедникБасеги()

    type СкульптураОлень private () =
        inherit Attraction(AttractionCards.СкульптураОлень)
        static member val instance = СкульптураОлень()

    type УтёсСтепанаРазина private () =
        inherit Attraction(AttractionCards.УтёсСтепанаРазина)
        static member val instance = УтёсСтепанаРазина()

    type НабережнаяБрюгге private () =
        inherit Attraction(AttractionCards.НабережнаяБрюгге)
        static member val instance = НабережнаяБрюгге()

    type ЖигулёвскиеГоры private () =
        inherit Attraction(AttractionCards.ЖигулёвскиеГоры)
        static member val instance = ЖигулёвскиеГоры()

    type ДворецЗемледельцев private () =
        inherit Attraction(AttractionCards.ДворецЗемледельцев)
        static member val instance = ДворецЗемледельцев()

    type ХребетЯлангас private () =
        inherit Attraction(AttractionCards.ХребетЯлангас)
        static member val instance = ХребетЯлангас()

/// Generated.
module Attractions =
    let ЗаповедникБасеги = Attraction.ЗаповедникБасеги.instance
    let СкульптураОлень = Attraction.СкульптураОлень.instance
    let УтёсСтепанаРазина = Attraction.УтёсСтепанаРазина.instance
    let НабережнаяБрюгге = Attraction.НабережнаяБрюгге.instance
    let ЖигулёвскиеГоры = Attraction.ЖигулёвскиеГоры.instance
    let ДворецЗемледельцев = Attraction.ДворецЗемледельцев.instance
    let ХребетЯлангас = Attraction.ХребетЯлангас.instance
