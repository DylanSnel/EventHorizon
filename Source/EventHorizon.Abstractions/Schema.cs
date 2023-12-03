

namespace EventHorizon;
public class Schema
{
    //Get from csproj file property if exists : <Eventhorizon.ApplicationName>ApplicationName</Eventhorizon.ApplicationName> or the Assembly Name
    public string ApplicationName { get; set; } = string.Empty;

    //Get from csproj file property if exists : <Eventhorizon.Subdomain>SubDomain</Eventhorizon.Subdomain>
    public string SubDomain { get; set; } = string.Empty;

    //Get from csproj file property if exists : <Eventhorizon.Domain>Domain</Eventhorizon.Domain>
    public string Domain { get; set; } = string.Empty;

    //Find all classes with HandlerAttribute
    public List<HandlerDefinition> Handlers { get; set; } = [];

    //Find all classes with TriggersAttribute
    public List<TriggerDefinition> TriggerDefinitions { get; set; } = [];

    //Find all classes with MapsAttribute
    public List<MapsDefinition> MapsDefinitions { get; set; } = [];

    //Find all objects that are used as types in the Handlers, the produced events, the triggers and the maps
    public List<TypeDefinition> TypeDefinitions { get; set; } = [];
}

public class TypeDefinition
{
    public string Name { get; set; } = string.Empty;
    public string Namespace { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;

    //Using newtonsoft json schema
    //Generate the schema for the type
    public string Schema { get; set; } = string.Empty;
}

public class MapsDefinition
{
    // Get from MapsAttribute Deprecated property
    public bool Deprecated { get; set; } = false;

    // Get from MapsAttribute Temporary property
    public bool Temporary { get; set; } = false;

    //The class name of the handler
    public string ClassName { get; set; } = string.Empty;

    //The income object type that will be mapped to the outgoing object type. Get it by the Generic Type TIncoming of the MapsAttribute
    public string Type { get; set; } = string.Empty;

    //The outgoing object type that will be mapped from the incoming object type. Get it by the Generic Type TOutGoing of the MapsAttribute
    public string MapsTo { get; set; } = string.Empty;
}

public class HandlerDefinition
{
    // Get from HandlerAttribute Name property or the class name
    public string Name { get; set; } = string.Empty;
    // Get from HandlerAttribute Description property
    public string Description { get; set; } = string.Empty;

    // Get from HandlerAttribute Deprecated property
    public bool Deprecated { get; set; } = false;

    // Get from HandlerAttribute Temporary property
    public bool Temporary { get; set; } = false;

    // Get from HandlerAttribute Tags property
    public List<string> Tags { get; set; } = [];

    //The class name of the handler
    public string ClassName { get; set; } = string.Empty;

    //The object type that the handler handles.  Get it by examining the first parameter generic of the interface or baseclass or by the Generic Type of the HandlerAttribute. When both fail you should create a diagnostic. The generic of the attribute is leading
    public string Type { get; set; } = string.Empty;

    //Find all ProducesAttribute or ProducesErrorAttribute on the class
    public List<ProducesDefinition> Produces { get; set; } = [];

    //Find all StartFlowAttributes on the class
    public List<FlowDefinition> StartsFlow { get; set; } = [];
}

public class FlowDefinition
{
    // Get from StartFlowAttribute FlowName property
    public string FlowName { get; set; } = string.Empty;

    // Get from StartFlowAttribute Tag property
    public List<string> Tags { get; set; } = [];

    //The class name of the handler
    public string ClassName { get; set; } = string.Empty;
}

public class ProducesDefinition
{
    // Get from ProducesAttribute Condition property
    public string Condition { get; set; } = string.Empty;

    // Get from ProducesAttribute Remarks property
    public string Remarks { get; set; } = string.Empty;

    //The class name of the handler
    public string ClassName { get; set; } = string.Empty;

    //The object type that the handler handles. Get it by the Generic Type of the ProducesAttribute
    public string Type { get; set; } = string.Empty;
    //The type of event that is produced. Get it by the attribute type ProducesAttribute or ProducesErrorAttribute
    public EventType EventType { get; set; } = EventType.Success;
}

public enum EventType
{
    Success, //Produced by ProducesAttribute
    Error //Produced by ProducesErrorAttribute
}

public class TriggerDefinition
{
    // Get from TriggersAttribute Description property
    public string Description { get; set; } = string.Empty;
    //Get from the TriggersAttribute Generic Type
    public string Type { get; set; } = string.Empty;
    //The class name of the handler
    public string ClassName { get; set; } = string.Empty;
}
