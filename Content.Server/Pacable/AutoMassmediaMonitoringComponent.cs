namespace Content.Server.MassMedia;

[RegisterComponent]
public sealed partial class AutoMassmediaMonitoringComponent : Component
{
	[DataField, ViewVariables(VVAccess.ReadOnly)]
	public List<string> KeyWords = new();

	[DataField, ViewVariables(VVAccess.ReadOnly)]
	public int CountTriggerArticles = 0;
}
