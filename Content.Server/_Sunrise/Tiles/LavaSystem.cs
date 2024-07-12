using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.StepTrigger.Systems;

namespace Content.Server._Sunrise.Tiles;

/// <summary>
/// This handles...
/// </summary>
public sealed class LavaSystem : EntitySystem
{
    [Dependency] private readonly FlammableSystem _flammable = default!;
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<LavaComponent, StepTriggeredOnEvent>(OnStepTriggered);
        SubscribeLocalEvent<LavaComponent, StepTriggerAttemptEvent>(OnStepTriggerAttempt);
    }

    private void OnStepTriggerAttempt(EntityUid uid, LavaComponent component, ref StepTriggerAttemptEvent args)
    {
        if (!HasComp<FlammableComponent>(args.Tripper))
            return;
        args.Continue = true;
    }

    private void OnStepTriggered(EntityUid uid, LavaComponent component, ref StepTriggeredOnEvent args)
    {
        Log.Info($"Step triggered {uid} {component.FireStacks}, {args.Tripper} {args.Source}");

        if (!TryComp<FlammableComponent>(args.Tripper, out var flammableComponent))
            return;

        var num = flammableComponent.FireStacks == 0.0 ? 5f : 1f;
        _flammable.AdjustFireStacks(args.Tripper, component.FireStacks * num, flammableComponent);
        _flammable.Ignite(args.Tripper, uid, flammableComponent);
    }
}
