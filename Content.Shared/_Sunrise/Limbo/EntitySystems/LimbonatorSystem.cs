using Content.Shared._Sunrise.Limbo.Components;
using Content.Shared.Actions;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Shared._Sunrise.Limbo.EntitySystems;

public sealed class SharedLimbonatorSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _action = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<LimbonatorComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<LimbonatorComponent, GoLimboActionEvent>(OnGoLimbo);
    }

    private void OnMapInit(EntityUid uid, LimbonatorComponent component, MapInitEvent args)
    {
        _action.AddAction(uid, component.LimboTransferAction);
    }

    private void OnGoLimbo(EntityUid uid, LimbonatorComponent component, GoLimboActionEvent ev)
    {

    }
}
