using System.Numerics;
using Content.Shared.Actions;
using Content.Shared.Hands;
using Content.Shared.Humanoid;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.Popups;
using Content.Shared.Wieldable;

namespace Content.Shared._Sunrise.SniperZoom;

/// <summary>
/// This handles...
/// </summary>
public sealed class ZoomableGunSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly SharedContentEyeSystem _eye = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _movementSpeedModifier = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<ZoomableGunComponent, TakeAimActionEvent>(OnTakeAimActionEvent);
        SubscribeLocalEvent<ZoomableGunComponent, GetItemActionsEvent>(OnGetItemActions);
        SubscribeLocalEvent<ZoomableGunComponent, ItemWieldedEvent>(OnItemWieldedEvent);
        SubscribeLocalEvent<ZoomableGunComponent, ItemUnwieldedEvent>(OnItemUnwieldedEvent);
        SubscribeLocalEvent<ZoomableGunComponent, ComponentShutdown>(OnComponentShutdown);
    }

    private void OnItemWieldedEvent(EntityUid uid, ZoomableGunComponent comp, ItemWieldedEvent ev)
    {
        comp.Wielded = true;
    }

    private void OnItemUnwieldedEvent(EntityUid uid, ZoomableGunComponent comp, ItemUnwieldedEvent ev)
    {
        comp.Wielded = false;
        if (ev.User == null)
            return;
        DisableZoom(ev.User.Value, comp);
    }

    private void EnableZoom(EntityUid uid, ZoomableGunComponent comp)
    {
        Log.Info($"Enablezoom");

        if (comp.AimComponent == null)
        {
            EnsureComp<TakeAimComponent>(uid, out var takeAimComponent);
            comp.AimComponent = takeAimComponent;
        }

        if (!TryComp<EyeComponent>(uid, out var eyeComponent))
            return;
        comp.Zoom = eyeComponent.Zoom;
        _eye.SetMaxZoom(uid, comp.TargetZoom);
        _eye.SetZoom(uid, comp.TargetZoom, ignoreLimits: false);
        if (comp.BaseWalkSpeed != null || comp.BaseSprintSpeed != null || comp.BaseAcceleration != null)
            return;
        if (!TryComp<MovementSpeedModifierComponent>(uid, out var modifierComponent))
            return;
        comp.BaseSprintSpeed = modifierComponent.BaseSprintSpeed;
        comp.BaseWalkSpeed = modifierComponent.BaseWalkSpeed;
        comp.BaseAcceleration = modifierComponent.Acceleration;
        _movementSpeedModifier.ChangeBaseSpeed(
            uid,
            comp.TargetWalkSpeed,
            comp.TargetSprintSpeed,
            comp.TargetAcceleration);

        comp.Enabled = true;
    }

    private void DisableZoom(EntityUid uid, ZoomableGunComponent comp)
    {
        Log.Info($"Disablezoom");
        if (comp.Zoom == null)
            return;
        if (!HasComp<EyeComponent>(uid))
            return;

        _eye.SetMaxZoom(uid, comp.Zoom.Value);
        _eye.SetZoom(uid, comp.Zoom.Value);

        if (comp.BaseWalkSpeed == null || comp.BaseSprintSpeed == null || comp.BaseAcceleration == null)
            return;
        _movementSpeedModifier.ChangeBaseSpeed(uid, comp.BaseWalkSpeed.Value, comp.BaseSprintSpeed.Value, comp.BaseAcceleration.Value);
        comp.BaseWalkSpeed = null;
        comp.BaseSprintSpeed = null;
        comp.BaseAcceleration = null;

        comp.Enabled = false;
    }

    private void OnComponentShutdown(EntityUid uid, ZoomableGunComponent comp, ComponentShutdown args)
    {
        DisableZoom(uid, comp);
    }

    private void OnGetItemActions(EntityUid uid, ZoomableGunComponent comp, GetItemActionsEvent ev)
    {
        Log.Info($"ongetitemactions: {ev.User}, {ev.Provider}");
        ev.AddAction(ref comp.TakeAimActionEntity, comp.TakeAimAction);
    }

    private void OnTakeAimActionEvent(Entity<ZoomableGunComponent> entityZoomableGunComponent, ref TakeAimActionEvent args)
    {
        Log.Info($"ontakeaimactionevent: {args.Handled}, {args.Performer}, {args.Action}");
        if (args.Handled)
            return;

        if (!entityZoomableGunComponent.Comp.Wielded)
        {
            _popup.PopupClient(Loc.GetString("wieldable-component-requires", ("item", MetaData(entityZoomableGunComponent.Owner).EntityName)), Transform(args.Performer).Coordinates, args.Performer, PopupType.Medium);
            return;
        }

        args.Handled = true;
        if (!entityZoomableGunComponent.Comp.Enabled)
        {
            EnableZoom(args.Performer, entityZoomableGunComponent.Comp);
        }
        else
        {
            DisableZoom(args.Performer, entityZoomableGunComponent.Comp);
        }
    }
}
