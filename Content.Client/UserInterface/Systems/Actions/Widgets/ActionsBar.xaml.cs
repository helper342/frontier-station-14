using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.UserInterface.Systems.Actions.Widgets;

[GenerateTypedNameReferences]
public sealed partial class ActionsBar : UIWidget
{
    public ActionsBar()
    {
        RobustXamlLoader.Load(this);
    }
}

