using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Utility;

namespace Content.Server._Sunrise.StationDeadsquad;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class StationMassmediaMonitoringComponent : Component
{
    [DataField]
    public TimeSpan Delay = TimeSpan.FromMinutes(10);

    [DataField]
    public TimeSpan RestartroundTimer = TimeSpan.FromMinutes(10);

    [DataField]
    public ResPath Shuttle = new ResPath("/Maps/_Sunrise/Shuttles/death_squad_shuttle.yml");

    [DataField]
    public string PaperPrototype = "Paper";

    [DataField]
    public string Operative = "RandomHumanoidSpawnerDeathSquad";

    [DataField]
    public List<string> TargetGoalId = ["VirusZombie", "VirusologyAmbusol"];

    [DataField]
    public List<string> TriggerWords =
        ["революция", "рева", "смерть нт", "viva", "la", "revolution", "секретная цель"];

    [DataField]
    public string AlertLevel = "epsilon";
}
