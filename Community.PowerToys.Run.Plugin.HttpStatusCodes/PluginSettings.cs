namespace Community.PowerToys.Run.Plugin.HttpStatusCodes;

public class PluginSettings
{
    public ReferenceType ReferenceType { get; set; } = ReferenceType.Rfc;
}

public enum ReferenceType
{
    Rfc = 0,
    Mdn = 1,
}