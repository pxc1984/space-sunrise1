namespace Content.Server.Mule.Components;

[RegisterComponent]
public sealed partial class MuleGoalComponent : Component
{
	[DataField("owner")]
	public EntityUid owner;
}
