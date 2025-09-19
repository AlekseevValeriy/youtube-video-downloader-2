using Microsoft.Maui.Layouts;

namespace YoTuViLo2.Controls;

public partial class TabPanel : ContentView
{
    Dictionary<String, ContentView> _pages = new();

    public static readonly BindableProperty TabsContentProperty = BindableProperty.Create(nameof(TabsContent), typeof(View), typeof(TabPanel), default(View));
    public View TabsContent
    {
        get => (View)GetValue(TabsContentProperty);
        private set => SetValue(TabsContentProperty, value);
    }

    public static readonly BindableProperty TabsTypeProperty = BindableProperty.Create(nameof(TabsType), typeof(TabsContentLayoutType), typeof(TabPanel), TabsContentLayoutType.Scroll);
    public TabsContentLayoutType TabsType
    {
        get => (TabsContentLayoutType)GetValue(TabsTypeProperty);
        set => SetValue(TabsTypeProperty, value);
    }

    public static readonly BindableProperty PageContentProperty = BindableProperty.Create(nameof(PageContent), typeof(View), typeof(TabPanel), default(View));
    public View PageContent
    {
        get => (View)GetValue(PageContentProperty);
        private set => SetValue(PageContentProperty, value);
    }

    public static readonly BindableProperty PagesSourceProperty = BindableProperty.Create(nameof(PagesSource), typeof(List<ContentView>), typeof(TabPanel), default(List<ContentView>), propertyChanged: OnPagesSourceChanged);
    static void OnPagesSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TabPanel tabPanel && newValue is List<ContentView> pages)
        {
            System.Diagnostics.Debug.WriteLine($"PagesSource updated: {pages.Count} pages");
            tabPanel.UpdateTabs();
        }
    }
    public List<ContentView> PagesSource
    {
        get => (List<ContentView>)GetValue(PagesSourceProperty);
        set => SetValue(PagesSourceProperty, value);
    }

    public static readonly BindableProperty PageTitleWidthRequestProperty = BindableProperty.Create(nameof(PageTitleWidthRequest), typeof(UInt32), typeof(TabPanel), 0u);
    public UInt32 PageTitleWidthRequest
    {
        get => (UInt32)GetValue(PageTitleWidthRequestProperty);
        set => SetValue(PageTitleWidthRequestProperty, value);
    }

    public static readonly BindableProperty TabSpacingProperty = BindableProperty.Create(nameof(TabSpacing), typeof(UInt32), typeof(TabPanel), 0u);
    public UInt32 TabSpacing
    {
        get => (UInt32)GetValue(TabSpacingProperty);
        set => SetValue(TabSpacingProperty, value);
    }

    public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(UInt32), typeof(TabPanel), 0u);
    public UInt32 Spacing
    {
        get => (UInt32)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public static readonly BindableProperty TabIndexProperty = BindableProperty.Create(nameof(TabIndex), typeof(UInt32), typeof(TabPanel), 0u);
    public UInt32 TabIndex
    {
        get => (UInt32)GetValue(TabIndexProperty);
        set => SetValue(TabIndexProperty, value);
    }

    public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(String), typeof(TabPanel), "TabPanel");
    public String Name
    {
        get => (String)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public TabPanel()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    void OnLoaded(Object? sender, EventArgs e)
    {
        if (Preferences.ContainsKey(Name))
        {
            TabIndex = (UInt32)Preferences.Get(Name, (Int32)TabIndex);
        }

        PageContent = _pages.ElementAtOrDefault(new Index((Int32)TabIndex)).Value;
    }

    public void UpdateTabs()
    {
        if (PagesSource is null) return;
        Boolean TabsTypeIsScroll = TabsType == TabsContentLayoutType.Scroll;

        FlexLayout layout = new FlexLayout()
        {
            Direction = FlexDirection.Row,
            Wrap = FlexWrap.NoWrap
        };

        foreach (ContentView page in PagesSource)
        {
            Button btn = new Button()
            {
                Text = page.GetType().Name.Replace("Page", ""),
                MinimumWidthRequest = 0
            }.Also(e =>
            {
                e.Clicked += (Object? sender, EventArgs e) =>
                {
                    if (sender is not Button btn) return;
                    PageContent = _pages[btn.Text];

                    TabIndex = (UInt32)_pages.Values.ToList().IndexOf(_pages[btn.Text]);
                    Preferences.Set(Name, TabIndex);
                };
            }).Also(e =>
            {
                if (TabsTypeIsScroll)
                {
                    e.WidthRequest = PageTitleWidthRequest;
                }
                else
                {
                    FlexLayout.SetBasis(e, new FlexBasis(0, true));
                    FlexLayout.SetGrow(e, 1);
                }
            });

            layout.Add(btn);
            layout.Add(new BoxView() { WidthRequest = TabSpacing, BackgroundColor = Colors.Transparent, MinimumWidthRequest = 0 });

            _pages[btn.Text] = page;
        }

        layout.RemoveAt(layout.Count - 1);

        switch (TabsType)
        {
            case TabsContentLayoutType.Scroll:
                {
                    TabsContent = new ScrollView()
                    {
                        Content = layout,
                        Orientation = ScrollOrientation.Horizontal
                    };
                    break;
                }
            case TabsContentLayoutType.Stretch:
                {
                    layout.JustifyContent = FlexJustify.SpaceBetween;
                    TabsContent = layout;
                    break;
                }
        }
    }
}