using EpicEnums.SourceGeneration.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Diagnostics;

namespace SourceGeneratorsToolkit.SyntaxExtensions.Attributes;
public static class AttributeSyntaxExtensions
{
    public static IncrementalValueProvider<ImmutableArray<AttributeSyntaxContext>> GetAttributeProvider<TAttribute>(this IncrementalGeneratorInitializationContext context) where TAttribute : Attribute
        => GetAttributeProvider(context, typeof(TAttribute));

    private static IncrementalValueProvider<ImmutableArray<AttributeSyntaxContext>> GetAttributeProvider(this IncrementalGeneratorInitializationContext context, Type? attributeType = null)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
                           predicate: static (node, _) => node is ClassDeclarationSyntax classDecl && classDecl.BaseList != null,
                           transform: (ctx, cancellationToken) =>
                           {
                               if (ctx.Node is not ClassDeclarationSyntax)
                               {
                                   return default;
                               }

                               var symbol = ctx.SemanticModel.GetDeclaredSymbol(ctx.Node) as INamedTypeSymbol;
                               if (symbol is null)
                               {
                                   return default;
                               }
                               if (!symbol.IsDerivedFromType(attributeType?.FullName ?? typeof(Attribute).FullName))
                               {
                                   return default;
                               }
                               return new AttributeSyntaxContext(ctx.Node, symbol, ctx.SemanticModel);
                           }).Where(x => x is not null)!.Collect();

        return provider!;
    }

    public static IncrementalValuesProvider<MemberAttributeSyntaxContext> FindAttributesProvider<TAttribute, TType>(this IncrementalGeneratorInitializationContext context) where TAttribute : Attribute where TType : MemberDeclarationSyntax
    => FindAttributesProvider<TAttribute, TType, MemberAttributeSyntaxContext>(context, (ctx, _) => ctx);

    public static IncrementalValuesProvider<MemberAttributeSyntaxContext> FindAttributesProvider<TAttribute, TType>(this IncrementalGeneratorInitializationContext context, Func<SyntaxNode, CancellationToken, bool> additionalPredicate) where TAttribute : Attribute where TType : MemberDeclarationSyntax
    => FindAttributesProvider<TAttribute, TType, MemberAttributeSyntaxContext>(context, additionalPredicate: additionalPredicate, transform: (ctx, _) => ctx);

    public static IncrementalValuesProvider<TReturn> FindAttributesProvider<TAttribute, TType, TReturn>(this IncrementalGeneratorInitializationContext context,
                                                                   Func<MemberAttributeSyntaxContext, CancellationToken, TReturn> transform) where TAttribute : Attribute where TType : MemberDeclarationSyntax
        => FindAttributesProvider<TAttribute, TType, TReturn>(context, null, transform);


    public static IncrementalValuesProvider<TReturn> FindAttributesProvider<TAttribute, TType, TReturn>(this IncrementalGeneratorInitializationContext context,
                                                                    Func<SyntaxNode, CancellationToken, bool> additionalPredicate,
                                                                    Func<MemberAttributeSyntaxContext, CancellationToken, TReturn> transform,
                                                                    IncrementalValueProvider<ImmutableArray<AttributeSyntaxContext>>? attributeProvider = null) where TAttribute : Attribute where TType : MemberDeclarationSyntax
    {
        return HandleFindAttributesProvide<TType, TReturn>(context, typeof(TAttribute), additionalPredicate, transform, attributeProvider);


    }

    public static IncrementalValuesProvider<TReturn> FindAttributesProvider<TType, TReturn>(this IncrementalGeneratorInitializationContext context,
                                                                   Type attributeType,
                                                                   Func<SyntaxNode, CancellationToken, bool> additionalPredicate,
                                                                   Func<MemberAttributeSyntaxContext, CancellationToken, TReturn> transform,
                                                                   IncrementalValueProvider<ImmutableArray<AttributeSyntaxContext>>? attributeProvider = null) where TType : MemberDeclarationSyntax
    {
        return HandleFindAttributesProvide<TType, TReturn>(context, attributeType, additionalPredicate, transform, attributeProvider);


    }

    public static IncrementalValuesProvider<TReturn> HandleFindAttributesProvide<TType, TReturn>(this IncrementalGeneratorInitializationContext context,
                                                            Type attributeType,
                                                            Func<SyntaxNode, CancellationToken, bool>? additionalPredicate,
                                                            Func<MemberAttributeSyntaxContext, CancellationToken, TReturn> transform,
                                                            IncrementalValueProvider<ImmutableArray<AttributeSyntaxContext>>? attributeProvider = null)
                                                            where TType : MemberDeclarationSyntax
    {
        var attributeProviderValue = attributeProvider ?? context.GetAttributeProvider();

        return context.SyntaxProvider.CreateSyntaxProvider(
          predicate: (node, _) => node is TType type && type.AttributeLists.Count > 0 && (additionalPredicate?.Invoke(node, _) ?? true),
          transform: (context, cancellationToken) => context)
            .Combine(attributeProviderValue)
            .Select((tuple, cancellationToken) =>
            {
                (var memberContext, var attributes) = tuple;

                if (memberContext.Node is not TType typedNode)
                {
                    return default;
                }

                var symbol = memberContext.SemanticModel.GetDeclaredSymbol(memberContext.Node) as INamedTypeSymbol;
                if (symbol is null)
                {
                    return default;
                }
                var memberAttributes = symbol.GetAttributes();
                if (memberAttributes.Length == 0)
                {
                    return default;
                }
                List<(AttributeData attribute, bool match)> attributeList = memberAttributes.Select(x =>
                {
                    var name = x.AttributeClass?.ToDisplayString();
                    return (x, name == attributeType.FullName);
                }).ToList();


                var attributeFromType = typedNode.AttributeLists.SelectMany(x => x.Attributes);
                foreach (var attribute in attributeFromType)
                {
                    Debug.WriteLine($"Attribute: {attribute.Name}");
                }

                if (!attributeList.Exists(x => x.match))
                {
                    return default;
                }
                return transform(new MemberAttributeSyntaxContext(memberContext.Node, symbol, memberContext.SemanticModel, attributeList.ToImmutableArray()), cancellationToken);
            }
        ).Where(x => x is not null)!;
    }



    // {
    //           if (context.Node is not TType typedNode)
    //           {
    //               return default;
    //           }

    //           var symbol = context.SemanticModel.GetDeclaredSymbol(context.Node) as INamedTypeSymbol;
    //           if (symbol is null)
    //           {
    //               return default;
    //           }
    //           var attributes = symbol.GetAttributes();
    //           if (attributes.Length == 0)
    //           {
    //               return default;
    //           }
    //           List<(AttributeData attribute, bool match)> attributeList = attributes.Select(x =>
    //           {
    //               var name = x.AttributeClass?.ToDisplayString();
    //               return (x, name == attributeType.FullName);
    //           }).ToList();


    //var attributeFromType = typedNode.AttributeLists.SelectMany(x => x.Attributes);
    //           foreach (var attribute in attributeFromType)
    //           {
    //               Debug.WriteLine($"Attribute: {attribute.Name}");
    //           }



    //           if (!attributeList.Exists(x => x.match))
    //           {
    //               return default;
    //           }

    //           return transform(new MemberAttributeSyntaxContext(context.Node, symbol, context.SemanticModel, attributeList.ToImmutableArray()), cancellationToken);

    //       }).Where(x => x is not null)!;

}


