using Content.Server.Mule.Components;

namespace Content.Server.Mule;

public sealed class MuleGoalSystem : EntitySystem
{
	public override void Initialize()
	{
		base.Initialize();
		SubscribeLocalEvent<MuleGoalComponent, ComponentStartup>(OnComponentStartup);
	}

	private void OnComponentStartup(EntityUid uid, MuleGoalComponent comp, ComponentStartup args)
	{

	}
}
