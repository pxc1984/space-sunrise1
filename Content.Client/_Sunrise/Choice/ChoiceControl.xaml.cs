﻿using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Utility;

// Taken from RMC14 build.
// https://github.com/RMC-14/RMC-14

namespace Content.Client._Sunrise.Choice;

[GenerateTypedNameReferences]
[Virtual]
public partial class ChoiceControl : Control
{
    public ChoiceControl() => RobustXamlLoader.Load(this);

    public void Set(string name, Robust.Client.Graphics.Texture? texture)
    {
        NameLabel.SetMessage(name);
        Icon.Texture = texture;
    }

    public void Set(FormattedMessage msg, Robust.Client.Graphics.Texture? texture)
    {
        NameLabel.SetMessage(msg);
        Icon.Texture = texture;
    }
}
