using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EventHorizon.Generators;

[Generator]
internal class EventHorizonSchemaGenerator : IIncrementalGenerator
{
    private readonly List<string> _attributes =
        [
            nameof(HandlerAttribute),
            nameof(MapsAttribute<object, object>),
            nameof(TriggersAttribute<object>),
            nameof(ProducesAttribute<object>),
            nameof(ProducesErrorAttribute<object>),
            nameof(StartFlowAttribute),
        ];
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Access global options (MSBuild properties) and parse the boolean
        var shouldRunSourceGenerator = context.AnalyzerConfigOptionsProvider
                                            .Select((provider, _) =>
                                            {
                                                if (provider.GlobalOptions.TryGetValue("build_property.Eventhorizon.GenerateSchema", out var propertyValue))
                                                {
                                                    // Try to parse the boolean value
                                                    return bool.TryParse(propertyValue, out bool result) && result;
                                                }
                                                return false;
                                            });

        // Set up the rest of the pipeline conditionally
        context.RegisterSourceOutput(shouldRunSourceGenerator, (ctx, isEnabled) =>
        {
            if (!isEnabled)
            {
                // If the property is false, skip the rest of the pipeline
                return;
            }

            var handlers = context.SyntaxProvider
                            .CreateSyntaxProvider(
                                predicate: static (node, _) => Filter(node),
                                transform: static (ctx, _) => TransformClass(ctx))
                            .Where(static m => m is not null);

            context.RegisterSourceOutput(handlers, (spContext, classInfo) => ExecuteSourceGeneration(spContext, classInfo));
        });


    }
    private static bool Filter(SyntaxNode node)
    {
        return (node is ClassDeclarationSyntax classDeclaration && HasCustomAttribute(classDeclaration)) ||
               (node is MethodDeclarationSyntax methodDeclaration && HasCustomAttribute(methodDeclaration));
    }

    private static bool HasCustomAttribute(ClassDeclarationSyntax classDeclaration)
    {
        return HasCustomAttributes(classDeclaration.AttributeLists);
    }

    private static bool HasCustomAttribute(MethodDeclarationSyntax methodDeclaration)
    {
        return HasCustomAttributes(methodDeclaration.AttributeLists);
    }

    private static bool HasCustomAttributes(SyntaxList<AttributeListSyntax> attributeLists)
    {
        return attributeLists
            .SelectMany(attrList => attrList.Attributes)
            .Any(attr => IsCustomAttribute(attr));
    }

    private static bool IsCustomAttribute(AttributeSyntax attribute)
    {
        // Implement logic to identify if the attribute is one of the custom attributes you're interested in
        var attributeName = attribute.Name.ToString();
        var customAttributes = new List<string> { "HandlerAttribute", "MapsAttribute", /* other attributes */ };
        return customAttributes.Contains(attributeName);
    }

    private static object TransformClass(GeneratorSyntaxContext ctx)
    {
        // Transform logic to build up your schema from the class information
        return new();
    }

    private static void ExecuteSourceGeneration(GeneratorExecutionContext context, object classInfo)
    {
        // Serialization and source generation logic
    }
}
