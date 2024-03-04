#load "Stage1.Handmade.fs"
#load "Stage1.Generated.fs"
#load "Stage2.Handmade.fs"
#load "../MyAstHelpers.fsx"
#load "../Generator.Literals.fs"

open RecordExts
open UnionExts
open AstHelpers
open MyAstHelpers

let inline (^) f x = f x

open Fantomas.FCS
open Fantomas.FCS.Syntax
open Fantomas.FCS.Xml
open Fantomas.FCS.Text
open Fantomas.FCS.Text.Range
open Fantomas.FCS.SyntaxTrivia

"""type Q =
    static member val Q = 42"""
|> SourceText.ofString 
|> Parse.parseFile false <| []
|> fst

open PhotoTour
open Generator.Literals

let attractionDefinition = """
type Attraction (card : AttractionCard) =
    member this.Card = card
"""

Code.fromModules [
    SynModuleOrNamespace.createNamespace Namespaces.root [
        Ident.parseSynExprLong attractionDefinition
        |> SynModuleDecl.Expr.CreateByDefault

        [Messages.generated]
        |> SynModuleDecl.NestedModule.create Modules.attraction [
            for Syntaxify.Name attractionName in AttractionCards.all do
                SynModuleDecl.Types.CreateByDefault [
                    SynTypeDefn.``type <name> private () =`` attractionName [
                        SynMemberDefn.ImplicitInherit.CreateByDefault(
                            Ident.parseSynType Types.attraction
                            , Ident.parseSynExprLong $"{Modules.cards}.{attractionName}"
                                |> SynExpr.paren
                        )

                        Ident.parseSynExprLong attractionName
                        |> SynExpr.app SynExpr.Const.unit
                        |> SynMemberDefn.Member.staticMemberVal Types.Attraction.instance
                    ]
                ]
        ]

        [Messages.generated]
        |> SynModuleDecl.NestedModule.create Modules.attractions [
            for Syntaxify.Name attractionName in AttractionCards.all do
                Ident.parseSynExprLong $"{Modules.attraction}.{attractionName}.{Types.Attraction.instance}"
                |> SynModuleDecl.Let.value attractionName
        ]
    ]
]
|> fun content ->
    System.IO.File.WriteAllText(
        System.IO.Path.Combine(
            __SOURCE_DIRECTORY__
            , "Stage2.Generated.fs"
        )
        , content
    )