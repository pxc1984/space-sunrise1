using System.Linq;
using Content.Server.Administration;
using Content.Server.Commands;
using Content.Shared.Administration;
using Microsoft.CodeAnalysis;
using Robust.Shared.Console;

namespace Content.Server._Sunrise.StationDeadsquad;

[AdminCommand(AdminFlags.Fun)]
public sealed class SendDeathsquadCommand : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IEntitySystemManager _entitySystems = default!;

    public string Command => "senddeathsquad";
    public string Description => Loc.GetString("");
    public string Help => Loc.GetString("");

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteError(Loc.GetString("shell-invalid-entity-id"));
            return;
        }

        var stationId = new EntityUid(int.Parse(args[0]));
        var entityManager = IoCManager.Resolve<EntityManager>();
        if (!entityManager.TryGetComponent<StationMassmediaMonitoringComponent>(stationId, out var monitoringComponent))
        {
            shell.WriteError(Loc.GetString("unable to send deathsquad, check StationMassmediaMonitoringComponent"));
            return;
        }
        if (!entityManager.TryGetComponent<MetaDataComponent>(stationId, out var metaData))
        {
            shell.WriteError(Loc.GetString("unable to send deathsquad, check MetaDataComponent"));
            return;
        }

        _entitySystems.GetEntitySystem<StationMassmediaMonitoringSystem>().SendDeathsquad(stationId, monitoringComponent);
        shell.WriteLine($"Successfully sent deathsquad to {metaData.EntityName}");
    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        switch (args.Length)
        {
            case 1:
                var stations = ContentCompletionHelper.StationIds(_entManager);
                return CompletionResult.FromHintOptions(stations, "[StationId]");
        }

        return CompletionResult.Empty;
    }
}
