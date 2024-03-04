#load "AstHelpers.fsx"

open RecordExts
open UnionExts
open AstHelpers

let inline (^) f x = f x

open Fantomas.FCS
open Fantomas.FCS.Syntax
open Fantomas.FCS.Xml
open Fantomas.FCS.Text
open Fantomas.FCS.Text.Range
open Fantomas.FCS.SyntaxTrivia

type SynModuleOrNamespace with
    static member createNamespace name decls =
        SynModuleOrNamespace.CreateByDefault(
            Ident.parseLong name
            , SynModuleOrNamespaceKind.DeclaredNamespace
            , SynModuleOrNamespaceTrivia.CreateByDefault (
                SynModuleOrNamespaceLeadingKeyword.Namespace.CreateByDefault()
            )
            , decls = decls
        )

type PreXmlDoc with
    static member create messageLines =
        PreXmlDoc.Create(messageLines, Range.Zero)

type SynModuleDecl.NestedModule with
    static member create name decls comments =
        SynModuleDecl.NestedModule.CreateByDefault(
            SynComponentInfo.CreateByDefault(
                Ident.parseLong name
                , xmlDoc = PreXmlDoc.create ^ Array.ofList comments
            )
            , SynModuleDeclNestedModuleTrivia.CreateByDefault(
                Some Range.Zero
                , Some Range.Zero
            )
            , decls = decls
        )

type SynBinding with
    static member create leadingKeyword synPat body =
        SynBinding.CreateByDefault(
            SynValData.empty
            , synPat
            , body
            , SynBindingTrivia.CreateByDefault(
                leadingKeyword
                , equalsRange = Some Range.Zero
            )
        )

type SynPat.Named with
    static member create name =
        SynPat.Named.CreateByDefault ^ Ident.parseSynIdent name

type SynPat.Tuple with
    static member create elemets =
        SynPat.Tuple.CreateByDefault(
            elementPats = List.map SynPat.Named.create elemets
            , commaRanges = List.map (fun _ -> Range.Zero) elemets.Tail
        )

type SynModuleDecl.Let with
    static member create synPat body =
        SynModuleDecl.Let.CreateByDefault(
            bindings = [
                SynBinding.create 
                    (SynLeadingKeyword.Let.CreateByDefault())
                    synPat
                    body
            ]
        )
    static member value name =
        SynPat.Named.create name
        |> SynModuleDecl.Let.create 

type SynExprRecordField with
    static member create name value =
        SynExprRecordField.CreateByDefault(
            (Ident.parseSynLong name, false)
            , equalsRange = Some Range.Zero
            , expr = Some value
        )

type SynExpr.Const with
    static member int32 value =
        SynConst.Int32 value
        |> SynExpr.Const.CreateByDefault

    static member string value =
        SynConst.String.CreateByDefault value
        |> SynExpr.Const.CreateByDefault

    static member unit = 
        SynExpr.Const.CreateByDefault SynConst.Unit

type SynExpr.ArrayOrList with
    static member list exprs =
        SynExpr.ArrayOrList.CreateByDefault(exprs = exprs)

type SynExpr.Record with
    static member create fields =
        SynExpr.Record.CreateByDefault(
            recordFields = fields
        )

type SynModuleDecl.Open with
    static member create path =
        Ident.parseSynLong path
        |> SynOpenDeclTarget.ModuleOrNamespace.CreateByDefault
        |> SynModuleDecl.Open.CreateByDefault

type SynMemberDefn.ImplicitCtor with
    static member privateUnit =
        SynMemberDefn.ImplicitCtor.CreateByDefault(
            SynSimplePats.SimplePats.CreateByDefault[]
            , SynMemberDefnImplicitCtorTrivia.CreateByDefault()
            , Some ^ SynAccess.Private.CreateByDefault()
        )

    static member create pats =
        SynMemberDefn.ImplicitCtor.CreateByDefault(
            pats
            , SynMemberDefnImplicitCtorTrivia.CreateByDefault()
        )

type SynMemberDefn.Member with
    static member staticMemberVal name body =
        SynBinding.create
            (SynLeadingKeyword.StaticMemberVal.CreateByDefault())
            (SynPat.Named.create name)
            body
        |> SynMemberDefn.Member.CreateByDefault

    static member ``member this.<name> =`` name body =
        SynBinding.create
            (SynLeadingKeyword.Member.CreateByDefault())
            (SynPat.Named.create $"this.%s{name}")
            body
        |> SynMemberDefn.Member.CreateByDefault

    static member ``member this.<name> (<args>)`` name args body =
        SynBinding.create 
            (SynLeadingKeyword.Member.CreateByDefault())
            (SynPat.LongIdent.CreateByDefault(
                Ident.parseSynLong ^ $"this.%s{name}"
                , SynArgPats.Pats.CreateByDefault [
                    match args with
                    | [] -> 
                        SynPat.Const.CreateByDefault SynConst.Unit
                    | args ->
                        SynPat.Tuple.CreateByDefault(
                            elementPats = [
                                for arg in args do
                                    SynPat.Named.create arg
                            ]
                            , commaRanges = 
                                List.map (fun _ -> Range.Zero) args.Tail
                        )
                        |> SynPat.Paren.CreateByDefault
                ]
            ))
            body
        |> SynMemberDefn.Member.CreateByDefault

type SynMemberDefn.ImplicitInherit with
    static member create synType args =
        SynMemberDefn.ImplicitInherit.CreateByDefault(
            synType
            , match args with
                | [] -> SynExpr.Const.unit
                | one :: [] -> SynExpr.paren one
                | many -> SynExpr.paren ^ SynExpr.tuple many
        )

type SynTypar with
    static member create name =
        SynTypar.CreateByDefault(
            Ident.parse name
            , TyparStaticReq.None
        )

type SynTyparDecls.SinglePrefix with
    static member create name =
        SynTyparDecl.CreateByDefault(
            SynTypar.create name
            , SynTyparDeclTrivia.Zero
        )
        |> SynTyparDecls.SinglePrefix.CreateByDefault

type SynTypeDefn with
    static member create info implicitConstructor members =
        SynTypeDefn.CreateByDefault(
            info
            , SynTypeDefnRepr.ObjectModel.CreateByDefault()
            , SynTypeDefnTrivia.CreateByDefault(
                SynTypeDefnLeadingKeyword.Type.CreateByDefault()
                , Some Range.Zero
            )
            , implicitConstructor = Some implicitConstructor
            , members = members
        )

    static member ``type <name> private () =`` name members =
        SynTypeDefn.create 
            (SynComponentInfo.CreateByDefault(Ident.parseLong name))
            SynMemberDefn.ImplicitCtor.privateUnit
            members

    static member ``type <name> with =`` name members =
        SynTypeDefn.CreateByDefault(
            SynComponentInfo.CreateByDefault(Ident.parseLong name)
            , SynTypeDefnRepr.ObjectModel.CreateByDefault(
                SynTypeDefnKind.Augmentation.CreateByDefault()
            )
            , SynTypeDefnTrivia.CreateByDefault(
                SynTypeDefnLeadingKeyword.Type.CreateByDefault()
                , withKeyword = Some Range.Zero
            )
            , members = members
        )

    static member ``type '<arg> <name> with =`` arg name members =
        SynTypeDefn.CreateByDefault(
            SynComponentInfo.CreateByDefault(
                Ident.parseLong name
                , typeParams = Some ^ SynTyparDecls.SinglePrefix.create arg
            )
            , SynTypeDefnRepr.ObjectModel.CreateByDefault(
                SynTypeDefnKind.Augmentation.CreateByDefault()
            )
            , SynTypeDefnTrivia.CreateByDefault(
                SynTypeDefnLeadingKeyword.Type.CreateByDefault()
                , withKeyword = Some Range.Zero
            )
            , members = members
        )

type SynType.App with
    static member classicGeneric name args =
        SynType.App.CreateByDefault(
            Ident.parseSynType name
            , Some Range.Zero
            , args
            , commaRanges = List.map (fun _ -> Range.Zero) args.Tail
            , greaterRange = Some Range.Zero
        )

    static member postfix name arg =
        SynType.App.CreateByDefault(
            Ident.parseSynType name
            , typeArgs = [arg]
            , isPostfix = true
        )

type SynModuleDecl.NestedModule with
    static member createAutoOpen name decls =
        SynModuleDecl.NestedModule.CreateByDefault(
            SynComponentInfo.CreateByDefault(
                Ident.parseLong name
                , [
                    SynAttributeList.CreateByDefault [
                        SynAttribute.CreateByDefault(
                            Ident.parseSynLong "AutoOpen"
                            , SynExpr.Const.unit
                        )
                    ]
                ]
            )
            , SynModuleDeclNestedModuleTrivia.CreateByDefault(
                Some Range.Zero
                , Some Range.Zero
            )
            , decls = decls
        )

type SynSimplePat.Id with
    static member create id =
        SynSimplePat.Id.CreateByDefault ^ Ident.parse id

type SynSimplePats.SimplePats with
    static member create args =
        SynSimplePats.SimplePats.CreateByDefault(
            args
            , List.map (fun _ -> Range.Zero) args.Tail
        )
    
    static member simple names =
        names
        |> List.map SynSimplePat.Id.create
        |> SynSimplePats.SimplePats.create

type SynTyparDecls.PostfixList with
    static member create names =
        SynTyparDecls.PostfixList.CreateByDefault [
            for name in names do
                SynTyparDecl.CreateByDefault(
                    SynTypar.create name
                    , SynTyparDeclTrivia.CreateByDefault()
                )
        ]

type SynTypeConstraint.WhereTyparSubtypeOfType with
    static member create name ofType =
        SynTypeConstraint.WhereTyparSubtypeOfType.CreateByDefault(
            SynTypar.create name
            , ofType
        )