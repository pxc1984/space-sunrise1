using Content.Shared.Antag;
using Robust.Shared.GameStates;
using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;
using Robust.Shared.Audio;

namespace Content.Shared.Mule.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedMuleSystem))]
public sealed partial class MuleComponent : Component//, IAntagStatusIconComponent
{
    // [DataField, ViewVariables(VVAccess.ReadWrite)]
    // public ProtoId<StatusIconPrototype> StatusIcon { get; set; } = "PrisonFaction";

    public override bool SessionSpecific => true;
}
