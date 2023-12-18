using System.Numerics;
using Robust.Client.AutoGenerated;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Utility;

namespace Content.Client.PDA;

[GenerateTypedNameReferences]
public sealed partial class PdaNavigationButton : ContainerButton
{

    private bool _isCurrent;
    private bool _isActive = true;

    private Thickness _borderThickness = new(0, 0, 0, 2);
    private Thickness _currentTabBorderThickness = new(2, 0, 2, 0);

    private readonly StyleBoxFlat _styleBox = new()
    {
        BackgroundColor = Color.FromHex("#202023"),
        BorderColor = Color.FromHex("#5a5a5a"),
        BorderThickness = new Thickness(0, 0, 0, 2)
    };

    public string InactiveBgColor { get; set; } = "#202023";
    public string ActiveBgColor { get; set; } = "#25252a";
    public string InactiveFgColor { get; set; } = "#5a5a5a";
    public string ActiveFgColor { get; set; } = "#FFFFFF";

    public SpriteSpecifier? IconTexture
    {
        set
        {
            Icon.Visible = value != null;
            Label.Visible = value == null;

            if (value is not null)
                Icon.SetFromSpriteSpecifier(value);
        }
    }

    public Vector2 IconScale
    {
        get => Icon.DisplayRect.TextureScale;
        set => Icon.DisplayRect.TextureScale = value;
    }

    public string? LabelText
    {
        get => Label.Text;
        set => Label.Text = value;
    }

    /// <summary>
    /// Sets the border thickness when the tab is not the currently active one
    /// </summary>
    public Thickness BorderThickness
    {
        get => _borderThickness;
        set
        {
            _borderThickness = value;
            _styleBox.BorderThickness = _isCurrent ? _currentTabBorderThickness : value;
        }
    }

    /// <summary>
    /// Sets the border thickness when this tab is the currently active tab
    /// </summary>
    public Thickness CurrentTabBorderThickness
    {
        get => _currentTabBorderThickness;
        set
        {
            _currentTabBorderThickness = value;
            _styleBox.BorderThickness = _isCurrent ? value : _borderThickness;
        }
    }

    public bool IsCurrent
    {
        get => _isCurrent;
        set
        {
            _isCurrent = value;
            _styleBox.BackgroundColor = Color.FromHex(value ? ActiveBgColor : InactiveBgColor);
            _styleBox.BorderThickness = value ? CurrentTabBorderThickness : BorderThickness;
        }
    }

    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            Icon.Modulate = Color.FromHex(value ? ActiveFgColor : InactiveFgColor);
            Label.FontColorOverride = Color.FromHex(value ? ActiveFgColor : InactiveFgColor);
        }
    }

    public PdaNavigationButton()
    {
        RobustXamlLoader.Load(this);
        Background.PanelOverride = _styleBox;
    }
}
