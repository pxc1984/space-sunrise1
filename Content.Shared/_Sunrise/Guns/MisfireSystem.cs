using Content.Shared.Popups;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Shared._Sunrise.Guns;

/// <summary>
/// This handles...
/// </summary>
public sealed class MisfireSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<CanMisfireComponent, ShotAttemptedEvent>(OnShotAttempted);
        SubscribeLocalEvent<ChamberMagazineAmmoProviderComponent, GunBoltOpenedEvent>(OnGunBoltOpened);
    }

    private void OnShotAttempted(EntityUid uid, CanMisfireComponent component, ref ShotAttemptedEvent args)
    {
        if (component.Jammed)
            args.Cancel();

        if (!component.Jammed)
        {
            if (_random.NextFloat() > component.Probability)
                return;

            component.Jammed = true;
            args.Cancel();
        }
        _popup.PopupCoordinates("click", Transform(uid).Coordinates);
        _audio.PlayPvs(new SoundPathSpecifier("/Audio/Weapons/click.ogg"), Transform(uid).Coordinates);
    }

    private void OnGunBoltOpened(EntityUid uid, ChamberMagazineAmmoProviderComponent component, GunBoltOpenedEvent args)
    {
        if (!TryComp<CanMisfireComponent>(uid, out var misfireComponent))
            return;

        if (misfireComponent.Jammed)
            misfireComponent.Jammed = false;
    }
}
