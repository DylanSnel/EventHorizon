namespace EventHorizon;

#pragma warning disable CS9113 // Parameter is unread.
#pragma warning disable IDE1006 // Naming Styles

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HandlerAttribute(string? Tag = null) : Attribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HandlerAttribute<TEvent>(string? Tag = null) : HandlerAttribute;

[AttributeUsage(AttributeTargets.Method)]
public class ProducesAttribute<TEvent>(string? Condition = null, string? Remarks = null) : Attribute;

[AttributeUsage(AttributeTargets.Method)]
public class ProducesErrorAttribute<TEvent>(string? Condition = null, string? Remarks = null) : Attribute;

[AttributeUsage(AttributeTargets.Class)]
public class StartFlowAttribute(string FlowName, string[]? Tag = null) : Attribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class Maps<TIncoming, TOutGoing>(bool Temporary = false) : Attribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class Triggers<TIncoming, TOutGoing>(bool Temporary = false) : Attribute;


#pragma warning restore CS9113 // Parameter is unread.
#pragma warning restore IDE1006 // Naming Styles
