using Content.Server.Actions;
using Content.Server.Implants.Components;
using Content.Server.Popups;
using Content.Server.Stealth;
using Content.Shared.Actions;
using Content.Shared.Implants.Components;
using Content.Shared.Popups;
using Content.Shared.Stealth.Components;
using Robust.Server.Containers;
using Robust.Shared.Timing;

namespace Content.Server._Sunrise.StealthImplant;

/// <summary>
/// This handles...
/// </summary>
public sealed class StealthImplantSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly StealthSystem _stealth = default!;
    [Dependency] private readonly ContainerSystem _container = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly ActionsSystem _actions = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {

    }

    public override void Update(float delay)
    {
        var query = AllEntityQuery<StealthImplantComponent>();
        while (query.MoveNext(out var uid, out var stealthImplantComponent))
        {

            if (!stealthImplantComponent.Enabled)
                continue;

            if (stealthImplantComponent.EnabledTime == null)
                continue;

            Log.Info($"updatestealthimplantcomponent: {_timing.CurTime - stealthImplantComponent.EnabledTime}, {stealthImplantComponent.UseTime}");
            if (_timing.CurTime - stealthImplantComponent.EnabledTime >= stealthImplantComponent.UseTime)  //
            {
                if (!TryComp<SubdermalImplantComponent>(uid, out var subdermalImplantComponent))
                    continue;

                if (subdermalImplantComponent.ImplantedEntity == null)
                    continue;

                stealthImplantComponent.Enabled = false;
                Log.Info($"disable");
                // disable

                Log.Info($"test");
                if (!HasComp<StealthOnMoveComponent>(subdermalImplantComponent.ImplantedEntity))
                    continue;
                RemComp<StealthOnMoveComponent>(subdermalImplantComponent.ImplantedEntity.Value);

                if (!TryComp<StealthComponent>(subdermalImplantComponent.ImplantedEntity, out var stealthComponent))
                    continue;
                _stealth.SetVisibility(subdermalImplantComponent.ImplantedEntity.Value, 1, stealthComponent);
                RemComp<StealthComponent>(uid);

                _popup.PopupEntity("Выключен инвиз", uid, uid, PopupType.Large);
            }
        }
    }
}
