﻿using Robust.Shared.Audio;
using Robust.Shared.Network;

namespace Content.Server._Sunrise.CryoTeleport;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class StationCryoTeleportComponent : Component
{
    [DataField]
    public TimeSpan TransferDelay = TimeSpan.FromSeconds(300);  // TODO: debug

    [DataField]
    public string PortalPrototype = "PortalCryo";

    [DataField]
    public SoundSpecifier TransferSound = new SoundPathSpecifier("/Audio/Effects/teleport_departure.ogg");
}
