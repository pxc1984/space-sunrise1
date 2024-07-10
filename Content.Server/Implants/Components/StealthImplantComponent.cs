namespace Content.Server.Implants.Components;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class StealthImplantComponent : Component
{
    [DataField]
    public bool Enabled;

    [DataField]
    public TimeSpan UseTime = TimeSpan.FromSeconds(10);

    [DataField]
    public TimeSpan? EnabledTime;

    [DataField]
    public float TargetVisibility = -1f;
}
