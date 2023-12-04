using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGeneratorsToolkit;
using SourceGeneratorsToolkit.SyntaxExtensions.Attributes;
using SourceGeneratorsToolkit.SyntaxProviders;
using System.Collections.Immutable;
using System.Diagnostics;

namespace EventHorizon.Generators;

[Generator]
internal class EventHorizonSchemaGenerator : IIncrementalGenerator
{
    static IncrementalValueProvider<ImmutableArray<AttributeSyntaxContext>>? _eventHorizonAttributeProvider;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        //#pragma warning disable IDE0079 // Remove unnecessary suppression
        //#pragma warning disable S125 // Sections of code should not be commented out
        //#if DEBUG
        //        if (!Debugger.IsAttached)
        //        {
        //            Debugger.Launch();
        //        }
        //#endif
        //#pragma warning restore S125 // Sections of code should not be commented out
        //#pragma warning restore IDE0079 // Remove unnecessary suppression
        _eventHorizonAttributeProvider = context.GetAttributeProvider<EventHorizonAttribute>();

        // Select MSBuild properties with the specific prefix
        var eventHorizonProperties = context.AnalyzerConfigOptionsProvider
            .Select((provider, _) =>
            {
                var options = provider.ToProvider("build_property.eventhorizon_");
                return new EventHorizonOptions
                {
                    ApplicationName = options.GetOption("applicationname", string.Empty),
                    SubDomain = options.GetOption("subdomain", string.Empty),
                    Domain = options.GetOption("domain", string.Empty),
                    GenerateSchema = options.GetOption("generateschema", false),
                };
            });
        var handlers = context.FindAttributesProvider<HandlerAttribute, ClassDeclarationSyntax, HandlerDefinition?>(
            null,
            transform: static (ctx, _) =>
            {

                var classDeclaration = ctx.TargetNode as ClassDeclarationSyntax;
                if (classDeclaration == null)
                {
                    return default;
                }

                var handlerDefinition = new HandlerDefinition
                {
                    ClassName = classDeclaration.Identifier.ValueText,
                    Type = classDeclaration.GetBaseTypeGenericTypeParameters().FirstOrDefault(),
                };


                foreach (var attribute in ctx.Attributes)
                {
                    Debug.WriteLine($"Attribute: {attribute.attribute.AttributeClass.ToDisplayString()}");
                }

                return handlerDefinition;

            }, _eventHorizonAttributeProvider).Collect();

        //var mapsClasses = context.FindAttributesProvider<ClassDeclarationSyntax, MapsDefinition>(
        //    attributeType: typeof(MapsAttribute<,>),
        //    null,
        //    transform: static (ctx, _) =>
        //    {

        //        var classDeclaration = ctx.TargetNode as ClassDeclarationSyntax;
        //        if (classDeclaration == null)
        //        {
        //            return default;
        //        }

        //        var handlerDefinition = new MapsDefinition
        //        {
        //            ClassName = classDeclaration.Identifier.ValueText,
        //            Type = classDeclaration.GetBaseTypeGenericTypeParameters().FirstOrDefault(),
        //        };


        //        foreach (var attribute in ctx.Attributes)
        //        {
        //            Debug.WriteLine($"Attribute: {attribute.attribute.AttributeClass.ToDisplayString()}");
        //        }

        //        return handlerDefinition;

        //    }).Collect();
        //var mapsMethods = context.FindAttributesProvider<MapsAttribute<string, string>, MethodDeclarationSyntax, MapsDefinition>(
        //    null,
        //    transform: static (ctx, _) =>
        //    {

        //        var classDeclaration = ctx.TargetNode as MethodDeclarationSyntax;
        //        if (classDeclaration == null)
        //        {
        //            return default;
        //        }

        //        var handlerDefinition = new MapsDefinition
        //        {
        //            ClassName = classDeclaration.Identifier.ValueText,
        //            Type = classDeclaration.GetGenericTypeParameters().FirstOrDefault(),
        //        };


        //        foreach (var attribute in ctx.Attributes)
        //        {
        //            Debug.WriteLine($"Attribute: {attribute.attribute.AttributeClass.ToDisplayString()}");
        //        }

        //        return handlerDefinition;

        //    }).Collect();

        //var collectedTriggers = triggers.Collect();
        //var maps = context.FindAttributesProvider<MapsAttribute>();
        //var collectedMaps = maps.Collect();
        //var combined = handlers.Combine(mapsClasses).Combine(mapsMethods);
        context.RegisterSourceOutput(handlers, (spContext, classInfo) =>
        {

            Debug.WriteLine($"Class: {classInfo}");

        });


        // Set up the rest of the pipeline conditionally
        context.RegisterSourceOutput(eventHorizonProperties, (ctx, properties) =>
        {
            if (!properties.GenerateSchema)
            {
                return;
            }

        });


    }
}
