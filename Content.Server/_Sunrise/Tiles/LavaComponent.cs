using Robust.Shared.Audio;

namespace Content.Server._Sunrise.Tiles;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class LavaComponent : Component
{
    [DataField] public SoundSpecifier DesintegrationSound = new SoundPathSpecifier("/Audio/Effects/lightburn.ogg");
    [DataField] public float FireStacks = 1.25f;
}
