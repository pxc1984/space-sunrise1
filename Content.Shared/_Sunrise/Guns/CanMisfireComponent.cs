using Robust.Shared.GameStates;

namespace Content.Shared._Sunrise.Guns;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class CanMisfireComponent : Component
{
    [DataField]
    public float Probability = 0.05f;

    [DataField]
    public bool Jammed;
}
