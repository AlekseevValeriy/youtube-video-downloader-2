namespace YoTuViLo2.Controls;

public partial class ExpansionPanel : ContentView
{
    Boolean _isExpanded = false;
    Boolean _isClickable = true;
    protected Boolean? _isHeightRequestSet = null;
    //Boolean _isPropertySet = false;

    public static readonly BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(ExpansionPanel), Colors.Transparent, propertyChanged: OnHeaderBackgroundColorChanged);
    public Color HeaderBackgroundColor
    {
        get => (Color)GetValue(HeaderBackgroundColorProperty);
        set => SetValue(HeaderBackgroundColorProperty, value);
    }
    static void OnHeaderBackgroundColorChanged(BindableObject bindable, Object oldValue, Object newValue)
    {
        if (bindable is ExpansionPanel panel && newValue is Color newColor)
        {
            //if (newColor == Colors.Transparent && Application.Current!.Resources.TryGetValue("Gray300", out Object result))
            //{
            //    panel.HeaderBackgroundColor = (Color)result ?? panel.HeaderBackgroundColor;
            //}
        }
    }

    public static readonly BindableProperty ContentBackgroundColorProperty = BindableProperty.Create(nameof(ContentBackgroundColor), typeof(Color), typeof(ExpansionPanel), Colors.Transparent, propertyChanged: OnContentBackgroundColorChanged);
    public Color ContentBackgroundColor
    {
        get => (Color)GetValue(ContentBackgroundColorProperty);
        set => SetValue(ContentBackgroundColorProperty, value);
    }
    static void OnContentBackgroundColorChanged(BindableObject bindable, Object oldValue, Object newValue)
    {
        if (bindable is ExpansionPanel panel && newValue is Color newColor)
        {
            //if (newColor == Colors.Transparent && Application.Current!.Resources.TryGetValue("Gray300", out Object result))
            //{
            //    panel.ContentBackgroundColor = (Color)result ?? panel.ContentBackgroundColor;
            //}
        }
    }

    public static readonly BindableProperty PanelCornerRadiusProperty = BindableProperty.Create(nameof(PanelCornerRadius), typeof(UInt32), typeof(ExpansionPanel), 0u);
    public UInt32 PanelCornerRadius
    {
        get => (UInt32)GetValue(PanelCornerRadiusProperty);
        set => SetValue(PanelCornerRadiusProperty, value);
    }

    public static readonly BindableProperty ClearanceProperty = BindableProperty.Create(nameof(Clearance), typeof(UInt32), typeof(ExpansionPanel), 0u);
    public UInt32 Clearance
    {
        get => (UInt32)GetValue(ClearanceProperty);
        set => SetValue(ClearanceProperty, value);
    }

    public static readonly BindableProperty ArrowSourceProperty = BindableProperty.Create(nameof(ArrowSource), typeof(ImageSource), typeof(ExpansionPanel), default(ImageSource));
    public ImageSource ArrowSource
    {
        get => (ImageSource)GetValue(ArrowSourceProperty);
        set => SetValue(ArrowSourceProperty, value);
    }

    public static readonly BindableProperty ArrowHeightRequestProperty = BindableProperty.Create(nameof(ArrowHeightRequest), typeof(UInt32), typeof(ExpansionPanel), 0u);
    public UInt32 ArrowHeightRequest
    {
        get => (UInt32)GetValue(ArrowHeightRequestProperty);
        set => SetValue(ArrowHeightRequestProperty, value);
    }

    public static readonly BindableProperty ArrowWidthRequestProperty = BindableProperty.Create(nameof(ArrowWidthRequest), typeof(UInt32), typeof(ExpansionPanel), 0u);
    public UInt32 ArrowWidthRequest
    {
        get => (UInt32)GetValue(ArrowWidthRequestProperty);
        set => SetValue(ArrowWidthRequestProperty, value);
    }

    public static readonly BindableProperty HeaderHeightRequestProperty = BindableProperty.Create(nameof(HeaderHeightRequest), typeof(UInt32), typeof(ExpansionPanel), 0u);
    public UInt32 HeaderHeightRequest
    {
        get => (UInt32)GetValue(HeaderHeightRequestProperty);
        set => SetValue(HeaderHeightRequestProperty, value);
    }

    public static readonly BindableProperty ContentHeightRequestProperty = BindableProperty.Create(nameof(ContentHeightRequest), typeof(UInt32), typeof(ExpansionPanel), 0u, propertyChanged: OnContentHeightRequestChanged);
    public UInt32 ContentHeightRequest
    {
        get => (UInt32)GetValue(ContentHeightRequestProperty);
        set => SetValue(ContentHeightRequestProperty, value);
    }
    static void OnContentHeightRequestChanged(BindableObject bindable, Object oldValue, Object newValue)
    {
        if (bindable is ExpansionPanel panel && newValue is UInt32 newHeightRequest)
        {
            if (panel.ContentHeightRequest != default && panel._isHeightRequestSet != false)
            {
                if (panel._isHeightRequestSet is null) panel._isHeightRequestSet = true;
                panel.ContentHolderHeightRequest = panel.ContentHeightRequest;
                panel.ContentHolderHeightRequest -= (UInt32)panel.ContentContainer.GetEmptySpace();
            }
            else
            {
                if (panel._isHeightRequestSet is null) panel._isHeightRequestSet = false;
                panel.Dispatcher.Dispatch(() =>
                {
                    Size request = panel.PanelContent.Measure(double.PositiveInfinity, double.PositiveInfinity);
                    panel.ContentHolderHeightRequest = (UInt32)request.Height;
                    panel.ContentHeightRequest = panel.ContentHolderHeightRequest + (UInt32)panel.ContentContainer.GetEmptySpace();
                });
            }
        }
    }

    static readonly BindableProperty ContentHolderHeightRequestProperty = BindableProperty.Create(nameof(ContentHolderHeightRequest), typeof(UInt32), typeof(ExpansionPanel), 0u);
    public UInt32 ContentHolderHeightRequest
    {
        get => (UInt32)GetValue(ContentHolderHeightRequestProperty);
        set => SetValue(ContentHolderHeightRequestProperty, value);
    }

    // Работает корректно только с View способным к уменьшению высоты до 0, то есть Layout`ы и элементы с MinimalHeightRequest = 0

    public static readonly BindableProperty PanelContentProperty = BindableProperty.Create(nameof(PanelContent), typeof(View), typeof(ExpansionPanel), default(View));
    public View PanelContent
    {
        get => (View)GetValue(PanelContentProperty);
        set => SetValue(PanelContentProperty, value);
    }

    public static readonly BindableProperty HeaderTitleProperty = BindableProperty.Create(nameof(HeaderTitle), typeof(String), typeof(ExpansionPanel), default(String));
    public String HeaderTitle
    {
        get => (String)GetValue(HeaderTitleProperty);
        set => SetValue(HeaderTitleProperty, value);
    }

    public static readonly BindableProperty ArrowMarginProperty = BindableProperty.Create(nameof(ArrowMargin), typeof(Thickness), typeof(ExpansionPanel), default(Thickness));
    public Thickness ArrowMargin
    {
        get => (Thickness)GetValue(ArrowMarginProperty);
        set => SetValue(ArrowMarginProperty, value);
    }


    public ExpansionPanel()
    {
        InitializeComponent();

        ContentContainer.SizeChanged += OnContentSizeChanged;
        Loaded += OnLoaded;
    }

    void OnLoaded(Object? sender, EventArgs e)
    {
        OnContentHeightRequestChanged(this, ContentHeightRequestProperty.DefaultValue, ContentHeightRequest);
        OnContentBackgroundColorChanged(this, ContentBackgroundColorProperty.DefaultValue, ContentBackgroundColor);
        OnHeaderBackgroundColorChanged(this, HeaderBackgroundColorProperty.DefaultValue, HeaderBackgroundColor);
    }

    void OnContentSizeChanged(Object? sender, EventArgs e)
    {
        ContentContainer.HeightRequest = ContentContainer.Height;
    }

    async void OnHeadTapped(Object? sender, EventArgs e)
    {
        if (!_isClickable) return;
        _isExpanded = !_isExpanded;

        if (_isExpanded)
        {
            _isClickable = false;
            if (Clearance != default)
            {
                AnimateClearance(Layout, 0, Clearance, 500, Easing.SinInOut);
            }
            ContentHolder.HeightRequest = ContentHeightRequest - ContentContainer.GetEmptySpace();
            await Task.WhenAll(
                ArrowIcon.RotateTo(270, 350, Easing.SinOut),
                ContentContainer.LayoutTo(new Rect(
                    ContentContainer.X,
                    ContentContainer.Y,
                    ContentContainer.Width,
                    ContentHeightRequest), 250, Easing.CubicInOut));
            _isClickable = true;
        }
        else
        {
            _isClickable = false;
            await Task.WhenAll(
                ArrowIcon.RotateTo(90, 350, Easing.SinIn),
                ContentContainer.LayoutTo(new Rect(
                    ContentContainer.X, ContentContainer.Y,
                    ContentContainer.Width,
                    0), 250, Easing.CubicInOut));
            if (Clearance != default)
            {
                AnimateClearance(Layout, Clearance, 0, 100, Easing.SinInOut);
            }
            ContentHolder.HeightRequest = 0;
            _isClickable = true;
        }
    }
    void AnimateClearance(VerticalStackLayout layout, Double from, Double to, UInt32 duration = 350, Easing? easing = null)
    {
        easing = easing ?? Easing.Linear;

        Animation animation = new(v =>
        {
            layout.Spacing = v;
        }, from, to);

        animation.Commit(layout, "ClearanceAnimation", length: duration, easing: easing);
    }
}