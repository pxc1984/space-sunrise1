using System.Numerics;

namespace Content.Shared._Sunrise.SniperZoom;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class TakeAimComponent : Component
{
    [DataField] public bool Enabled;
    [DataField] public string TakeAimAction = "ActionTakeAim";
    [DataField] public EntityUid? TakeAimActionEntity;
    [DataField] public Vector2 Zoom = new (1.25f, 1.25f);
}
