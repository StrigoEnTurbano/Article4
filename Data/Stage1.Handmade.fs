namespace PhotoTour

[<AutoOpen>]
module Auto =
    let inline (^) f x = f x

type Kind =
    | Urban
    | Natural

// В самой игре данные регионы не названы и обозначены лишь цветом.
type Region =
    | North
    | Central
    | Volga
    | South
    | Ural
    | Siberia
    | FarEast

type PhotoEquipment = 
    | Film
    | Lens
    | Smartphone
    | Quadcopter
    | Tripod

// Skipped
//type Bonus = class end

type AttractionCard = {
    Id : int
    Name : string
    // Too long.
    //Description : string
    Place : string
    Kind : Kind
    Region : Region
    VictoryPoints : int
    PhotoEquipments : PhotoEquipment list
    // Skipped
    //Bonus : Bonus option
}