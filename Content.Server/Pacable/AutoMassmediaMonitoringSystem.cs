using System;
using System.Collections.Generic;
using Content.Shared.MassMedia.Systems;
using Content.Shared.MassMedia.Components;
using Content.Server.Chat.Managers;
using Microsoft.CodeAnalysis;
using Robust.Server.Console.Commands;
using Content.Server.Station.Systems;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
using Content.Server.AlertLevel;

namespace Content.Server.MassMedia;

public sealed class AutoMassmediaMonitoringSystem : EntitySystem
{
	[Dependency] private readonly IChatManager _chatManager = default!;
	[Dependency] private readonly StationSystem _stationSystem = default!;
	[Dependency] private readonly MapSystem _mapSystem = default!;
	[Dependency] private readonly MapLoaderSystem _mapLoaderSystem = default!;
	[Dependency] private readonly ILogManager _logManager = default!;
	[Dependency] private readonly IMapManager _mapManager = default!;
	[Dependency] private readonly AlertLevelSystem _alertLevelSystem = default!;
	public override void Initialize()
	{
		base.Initialize();
		SubscribeLocalEvent<AutoMassmediaMonitoringComponent, NewsArticlePublishedEvent>(OnNewsArticlePublished);
		SubscribeLocalEvent<AutoMassmediaMonitoringComponent, NewsArticleDeletedEvent>(OnNewsArticleDeleted);
	}

	private void OnNewsArticlePublished(EntityUid uid, AutoMassmediaMonitoringComponent comp, ref NewsArticlePublishedEvent args)
	{
		Console.WriteLine($"New article published with content:\n{args.Article.Content}");
		if (IsTriggerArticle(args.Article.Content, comp.KeyWords))
		{
			_chatManager.SendAdminAlert(uid, Loc.GetString("admin-revolution-alert-underway"));
			comp.CountTriggerArticles++;
			var containingStationUid = _stationSystem.GetOwningStation(uid);
			if (containingStationUid is null)
				return;
			// _alertLevelSystem.SetLevel(containingStationUid, "epsilon", true, true, true, true);
			var map = _mapManager.CreateMap();
			_mapLoaderSystem.TryLoad(map, "/Maps/_Sunrise/Shuttles/death_squad_shuttle.yml", out var roots);
		}
	}

	private void OnNewsArticleDeleted(EntityUid uid, AutoMassmediaMonitoringComponent comp, ref NewsArticleDeletedEvent args)
	{
		var containingStationUid = _stationSystem.GetOwningStation(uid);
		if (containingStationUid is null)
			return;
		Console.WriteLine($"I'm on station: {containingStationUid}");
		StationNewsComponent? newsComponent;
		if (!TryComp<StationNewsComponent>(containingStationUid, out newsComponent))
			return;
		// int cnt = 0;
		// foreach (var article in newsComponent.Articles)
		// {
		// 	if (IsTriggerArticle(article.Content, comp.KeyWords))
		// 		cnt++;
		// }
		// if (cnt < comp.CountTriggerArticles)  // Trigger article was deleted
		// {

		// }
	}

	private bool IsTriggerArticle(string text, List<string> keyWords)
	{
		bool flag = false;
		foreach (string word in keyWords)
		{
			if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
			{
				flag = true;
			}
		}
		return flag;
	}
}
