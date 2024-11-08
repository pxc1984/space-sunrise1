﻿using Content.Client.UserInterface.Controls;
using Content.Shared._Sunrise.CentCom.BUIStates;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.XAML;

namespace Content.Client._Sunrise.CentCom.UI;

[GenerateTypedNameReferences]
public sealed partial class CentComCargoConsoleWindow : FancyWindow
{
    public CentComCargoConsoleWindow()
    {
        RobustXamlLoader.Load(this);


        AddElement("test124123",
            "test description",
            new List<string>()
            {
                "test1",
                "test2",
                "test3",
            });
        AddElement("test124213",
            "test description",
            new List<string>()
            {
                "test1",
                "test2",
                "test3",
            });
        AddElement("test124",
            "test description",
            new List<string>()
            {
                "test1",
                "test2",
                "test3",
            });
        AddElement("test1321",
            "test description",
            new List<string>()
            {
                "test1",
                "test2",
                "test3",
            });
        AddElement("test12",
            "test description",
            new List<string>()
            {
                "test1",
                "test2",
                "test3",
            });
        AddElement("test21",
            "test description",
            new List<string>()
            {
                "test1",
                "test2",
                "test3",
            });
        // Ты тут закончил
    }

    public void UpdateState(CentComCargoConsoleBoundUserInterfaceState state)
    {

    }

    public void AddElement(string title, string description, List<string> contents)
    {
        var newControl = new CentComGiftsConsoleEntry(title, description, contents);
        Container.AddChild(newControl);
    }
}

