﻿using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client._Sunrise.Medical.Surgery;
// Based on the RMC14.
// https://github.com/RMC-14/RMC-14
[GenerateTypedNameReferences]
public sealed partial class SurgeryWindow : DefaultWindow
{
    public SurgeryWindow() => RobustXamlLoader.Load(this);
}
