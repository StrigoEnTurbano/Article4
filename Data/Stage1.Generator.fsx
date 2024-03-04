#load "Stage1.Handmade.fs"
#load "../MyAstHelpers.fsx"
#load "../Generator.Literals.fs"

open RecordExts
open UnionExts
open AstHelpers
open MyAstHelpers
open PhotoTour
open Generator.Literals

let inline (^) f x = f x

open Fantomas.FCS
open Fantomas.FCS.Syntax
open Fantomas.FCS.Xml
open Fantomas.FCS.Text
open Fantomas.FCS.Text.Range
open Fantomas.FCS.SyntaxTrivia

"""
let listSample = [12;23;34]
"""
|> SourceText.ofString
|> Parse.parseFile false <| []
|> fst

let attractions = [
    let regions = [
        Region.Volga
        , [
            15, "Заповедник Басеги", "Пермский край", Kind.Natural, 2, []
            16, "Скульптура \"Олень\"", "Нижний Новгород", Kind.Urban, 1, [Smartphone]
            17, "Утёс Степана Разина", "Саратовская область", Kind.Natural, 4, [Tripod; Smartphone]
            18, "Набережная Брюгге", "Йошкар-Ола", Kind.Urban, 1, [Lens]
            19, "Жигулёвские горы", "Самарская область", Kind.Natural, 2, [Smartphone; Quadcopter]
            20, "Дворец земледельцев", "Казань", Kind.Urban, 1, [Quadcopter; Quadcopter]
            21, "Хребет Ялангас", "Республика Башкортостан", Kind.Natural, 3, [Lens; Tripod; Smartphone]
        ]
    ]
    for region, preCards in regions do
        for id, name, place, kind, victoryPoints, photoEquipment in preCards do
            {
                AttractionCard.Id = id
                Name = name
                Place = place
                Kind = kind
                Region = region
                VictoryPoints = victoryPoints
                PhotoEquipments = photoEquipment
            }
]

module Field =
    let create = SynExprRecordField.create
    let make valueToSynExpr name = valueToSynExpr >> create name
    let int32 = make SynExpr.Const.int32
    let string = make SynExpr.Const.string
    let longIdent = make Ident.parseSynExprLong

Code.fromModules [
    SynModuleOrNamespace.createNamespace Namespaces.root [
        [Messages.generated]
        |> SynModuleDecl.NestedModule.create Modules.cards [
            for attraction in attractions do
                SynExpr.Record.create [
                    Field.int32 $"{Types.card}.Id" attraction.Id
                    Field.string "Name" attraction.Name
                    Field.string "Place" attraction.Place
                    Field.longIdent "Kind" $"Kind.{attraction.Kind}"
                    Field.longIdent "Region" $"Region.{attraction.Region}"
                    Field.int32 "VictoryPoints" attraction.VictoryPoints
                    Field.create "PhotoEquipments" ^ SynExpr.ArrayOrList.list [
                        for item in attraction.PhotoEquipments do
                            Ident.parseSynExprLong ^ string item
                    ]
                ]
                |> SynModuleDecl.Let.value ^ Syntaxify.name attraction

            SynExpr.ArrayOrList.list [
                for Syntaxify.Name attractionName in attractions do
                    Ident.parseSynExprLong attractionName
            ]
            |> SynModuleDecl.Let.value "all"
        ]
    ]
]
|> fun content ->
    System.IO.File.WriteAllText(
        System.IO.Path.Combine(
            __SOURCE_DIRECTORY__
            , "Stage1.Generated.fs"
        )
        , content
    )