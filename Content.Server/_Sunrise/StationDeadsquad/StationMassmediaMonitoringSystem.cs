using Content.Server._Sunrise.StationGoal;
using Content.Server.AlertLevel;
using Content.Server.Chat.Systems;
using Content.Server.Explosion.EntitySystems;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Paper;
using Content.Server.RoundEnd;
using Content.Server.Spawners.Components;
using Content.Server.Station.Systems;
using Content.Shared._Sunrise.Shuttles;
using Content.Shared.Fax.Components;
using Content.Shared.GameTicking;
using Content.Shared.MassMedia.Components;
using Content.Shared.MassMedia.Systems;
using Content.Shared.Paper;
using Robust.Server.GameObjects;
using Robust.Server.Maps;
using Robust.Shared.Map;
using Robust.Shared.Timing;

namespace Content.Server._Sunrise.StationDeadsquad;

/// <summary>
/// This handles...
/// </summary>
public sealed class StationMassmediaMonitoringSystem : EntitySystem
{
    [Dependency] private readonly EntityManager _entMan = default!;
    [Dependency] private readonly IMapManager _mapMan = default!;
    [Dependency] private readonly MapLoaderSystem _loader = default!;
    [Dependency] private readonly AlertLevelSystem _alert = default!;
    [Dependency] private readonly RoundEndSystem _roundEnd = default!;
    [Dependency] private readonly ExplosionSystem _explosion = default!;
    [Dependency] private readonly PaperSystem _paper = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<StationInitializedEvent>(OnStationInitialized);
        SubscribeLocalEvent<StationMassmediaMonitoringComponent, ComponentStartup>(OnComponentStartup);
        SubscribeLocalEvent<StationMassmediaMonitoringComponent, NewsArticlePublishedEvent>(OnNewsArticlePublished);
    }

    private void OnStationInitialized(StationInitializedEvent args)
    {
        var revQuery = AllEntityQuery<RevolutionaryRuleComponent>();
        if (!revQuery.MoveNext(out var _, out var _))
            return;
        revQuery.Dispose();

        if (!TryComp<StationNewsComponent>(args.Station, out var stationNews))
            return;

        var stationMassmediaMonitoringComponent = EnsureComp<StationMassmediaMonitoringComponent>(args.Station);
    }

    private void OnComponentStartup(EntityUid uid, StationMassmediaMonitoringComponent comp, ComponentStartup ev)
    {

    }

    private void OnNewsArticlePublished(EntityUid uid,
        StationMassmediaMonitoringComponent comp,
        NewsArticlePublishedEvent ev)
    {
        if (CheckTriggerWords(ev.Article.Content, comp.TriggerWords))
        {
            SendAlertPaper(comp);
            _chat.DispatchStationAnnouncement(uid, Loc.GetString("massmedia-announcement"), Loc.GetString("sod-name"), colorOverride: Color.Purple);
            Timer.Spawn(comp.Delay, () => { TrySendDeathsquad(uid, comp); });
        }
    }

    private EntityUid? SendAlertPaper(StationMassmediaMonitoringComponent comp)
    {
        var query = EntityQueryEnumerator<FaxMachineComponent>();
        while (query.MoveNext(out var faxId, out var fax))
        {
            if (!fax.ReceiveStationGoal)
                continue;

            var printout = new FaxPrintout(Loc.GetString("massmedia-paper-revolution"),
                Loc.GetString("massmedia-paper-name"),
                null,
                null,
                "paper_stamp-centcom",
                new List<StampDisplayInfo>
                {
                    new() { StampedName = Loc.GetString("sod-name"), StampedColor = Color.Purple },
                    new() { StampedName = Loc.GetString("tango-name"), StampedColor = Color.Firebrick },
                });

            if (!TryComp<TransformComponent>(faxId, out var xform))
                continue;

            var paper = _entMan.SpawnEntity(comp.PaperPrototype, xform.Coordinates);
            _paper.SetContent(paper, printout.Content);
            foreach (var stamp in printout.StampedBy)
            {
                if (printout.StampState == null)
                    continue;
                _paper.TryStamp(paper, stamp, printout.StampState);
            }

            return paper;
        }

        return null;
    }

    private void TrySendDeathsquad(EntityUid stationUid, StationMassmediaMonitoringComponent comp)
    {
        if (!TryComp<StationNewsComponent>(stationUid, out var stationNewsComponent))
            return;

        foreach (var newsArticle in stationNewsComponent.Articles)
        {
            if (CheckTriggerWords(newsArticle.Content, comp.TriggerWords))
            {
                SendDeathsquad(stationUid, comp);
                return;
            }
        }
    }

    public void SendDeathsquad(EntityUid stationUid, StationMassmediaMonitoringComponent comp)
    {
        var map = _mapMan.CreateMap();
        if (!_loader.TryLoad(map, comp.Shuttle.ToString(), out var rootUids, new MapLoadOptions()))
            return;
        var centCommShuttleComponent = EnsureComp<CentCommShuttleComponent>(rootUids[0]);
        if (!TryComp<TransformComponent>(rootUids[0], out var deadsquadXform))
            return;

        var query = AllEntityQuery<SpawnPointComponent>();
        while (query.MoveNext(out var spawnPointUid, out var spawnPointComponent))
        {
            if (!TryComp<TransformComponent>(spawnPointUid, out var tempXform))
                continue;

            if (tempXform.MapID != deadsquadXform.MapID)
                continue;

            _entMan.SpawnEntity(comp.Operative, tempXform.Coordinates);
            _entMan.DeleteEntity(spawnPointUid);
        }
        _mapMan.SetMapPaused(map, false);

        _alert.SetLevel(stationUid, comp.AlertLevel, true, true, true, true);

        Timer.Spawn(comp.RestartroundTimer, () => _roundEnd.EndRound(TimeSpan.FromSeconds(30)));
    }

    private bool CheckTriggerWords(string content, List<string> words)
    {
        var found = false;

        foreach (var word in words)
        {
            if (content.Contains(word, StringComparison.CurrentCultureIgnoreCase))
            {
                found = true;
            }
        }

        return found;
    }
}
