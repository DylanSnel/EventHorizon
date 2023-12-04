namespace EventHorizon.Generators;
internal class EventHorizonOptions
{
    public string ApplicationName { get; set; } = string.Empty;
    public string SubDomain { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public bool GenerateSchema { get; internal set; }
}
