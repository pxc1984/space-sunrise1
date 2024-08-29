using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Robust.Shared.Timing;

namespace Content.Server._Sunrise.GasRegeneration;

public sealed class GasRegenerationSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;

    public TimeSpan NextTick = TimeSpan.Zero;
    public TimeSpan RefreshCooldown = TimeSpan.FromSeconds(1);

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GasRegenerationComponent, EntityUnpausedEvent>(OnUnpaused);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (NextTick > _timing.CurTime)
            return;

        NextTick += RefreshCooldown;

        var query = EntityQueryEnumerator<GasRegenerationComponent, GasTankComponent>();
        while (query.MoveNext(out var uid, out var gasRegen, out var gasTank))
        {
            if (_timing.CurTime < gasRegen.NextRegenTime)
                continue;

            gasRegen.NextRegenTime = _timing.CurTime + gasRegen.Duration;
            _atmosphereSystem.Merge(gasTank.Air, gasRegen.AirRegen.Clone());
        }
    }

    private void OnUnpaused(EntityUid uid, GasRegenerationComponent comp, ref EntityUnpausedEvent args)
    {
        comp.NextRegenTime += args.PausedTime;
    }
}
