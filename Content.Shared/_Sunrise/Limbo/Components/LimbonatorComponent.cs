using Content.Shared.Actions;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Sunrise.Limbo.Components;

[RegisterComponent]
public sealed partial class LimbonatorComponent : Component
{
    [DataField]
    public EntProtoId LimboTransferAction = "LimboTransferAction";
}
