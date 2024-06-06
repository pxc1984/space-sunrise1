using Content.Shared.Actions;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Sunrise.Enderchest;

[RegisterComponent]
public sealed partial class EnderchestComponent : Component
{
	[DataField("enderchestActionPrototype")]
	public EntProtoId OpenEnderchestActionPrototype = "OpenEnderchestAction";
}

public sealed partial class EnderchestOpenActionEvent : InstantActionEvent
{

}
