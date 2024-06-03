using Content.Server.Administration.Logs;
using Robust.Shared.Random;
using Content.Server.Antag;
using Content.Server.EUI;
using Content.Server.Flash;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Mind;
using Content.Server.Popups;
using Content.Server.Revolutionary;
using Content.Server.Revolutionary.Components;
using Content.Server.Roles;
using Content.Server.RoundEnd;
using Content.Server.Shuttles.Systems;
using Content.Server.Station.Systems;
using Content.Shared.Database;
using Content.Shared.Humanoid;
using Content.Shared.IdentityManagement;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.Mindshield.Components;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.NPC.Prototypes;
using Content.Shared.NPC.Systems;
using Content.Shared.Revolutionary.Components;
using Content.Shared.Stunnable;
using Content.Shared.Zombies;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Content.Server.GameTicking.Components;
using Content.Shared.Cuffs.Components;

namespace Content.Server.GameTicking.Rules;

public sealed class MuleRuleSystem : GameRuleSystem<MuleRuleComponent>
{
    [Dependency] private readonly AntagSelectionSystem _antag = default!;
    [Dependency] private readonly MindSystem _mindSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MuleRuleComponent, AfterAntagEntitySelectedEvent>(AfterAntagSelected);
        SubscribeLocalEvent<MuleRoleComponent, GetBriefingEvent>(OnGetBriefing);
    }

    private void AfterAntagSelected(Entity<MuleRuleComponent> ent, ref AfterAntagEntitySelectedEvent args)
    {
        if (!_mindSystem.TryGetMind(args.EntityUid, out var mindId, out var mind))
            return;

        _antag.SendBriefing(args.EntityUid, MakeBriefing(args.EntityUid), null, null);
    }

    private void OnGetBriefing(Entity<MuleRoleComponent> mule, ref GetBriefingEvent args)
    {
        if (!TryComp<MindComponent>(mule.Owner, out var mind) || mind.OwnedEntity == null)
            return;
        args.Append(MakeBriefing(mind.OwnedEntity.Value));
    }

    private string MakeBriefing(EntityUid mule)
    {
        return Loc.GetString("mule-role-greeting") + "\n \n" + Loc.GetString("mule-role-greeting-equipment") + "\n";
    }
}
