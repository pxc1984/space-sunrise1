using Content.Shared.Actions;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Input.Binding;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

// using System;

namespace Content.Shared._Sunrise.Enderchest;

public sealed class SharedEnderchestSystem : EntitySystem
{
	[Dependency] private readonly SharedActionsSystem _actions = default!;

	public override void Initialize()
	{
		base.Initialize();
		SubscribeLocalEvent<EnderchestComponent, MapInitEvent>(OnMapInit);
		SubscribeLocalEvent<EnderchestComponent, EnderchestOpenActionEvent>(OnOpenEnderchest);
	}

	private void OnMapInit(EntityUid uid, EnderchestComponent component, MapInitEvent args)
	{
		_actions.AddAction(uid, component.OpenEnderchestActionPrototype);
	}

	private void OnOpenEnderchest(EntityUid uid, EnderchestComponent component, EnderchestOpenActionEvent ev)
	{
		// Console.WriteLine("OnOpenEnderchest");
	}
}
