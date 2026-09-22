using System.Linq;

namespace JustMusic.Behaviors;

public class PlayingGradientBehavior : Behavior<Border>
{
    public static readonly BindableProperty IsPlayingProperty =
        BindableProperty.Create(
            nameof(IsPlaying),
            typeof(bool),
            typeof(PlayingGradientBehavior),
            false,
            propertyChanged: OnIsPlayingChanged);

    public bool IsPlaying
    {
        get => (bool)GetValue(IsPlayingProperty);
        set => SetValue(IsPlayingProperty, value);
    }

    private Border? _border;

    protected override void OnAttachedTo(Border bindable)
    {
        base.OnAttachedTo(bindable);

        _border = bindable;

        // ВАЖЛИВО для Behavior у DataTemplate:
        BindingContext = bindable.BindingContext;

        bindable.BindingContextChanged += Border_BindingContextChanged;

        if (IsPlaying)
            StartAnimation();
    }

    protected override void OnDetachingFrom(Border bindable)
    {
        bindable.BindingContextChanged -= Border_BindingContextChanged;
        bindable.AbortAnimation("PlayingGradient");

        _border = null;

        base.OnDetachingFrom(bindable);
    }

    private void Border_BindingContextChanged(object? sender, EventArgs e)
    {
        if (sender is Border border)
            BindingContext = border.BindingContext;
    }

    private static void OnIsPlayingChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var behavior = (PlayingGradientBehavior)bindable;

        if ((bool)newValue)
            behavior.StartAnimation();
        else
            behavior.StopAnimation();
    }

    private void StartAnimation()
    {
        if (_border == null)
            return;

        _border.AbortAnimation("PlayingGradient");

        GradientStopCollection? gradientStops = null;

        if (_border.Background is LinearGradientBrush linear)
        {
            gradientStops = linear.GradientStops;
        }
        else if (_border.Background is RadialGradientBrush radial)
        {
            gradientStops = radial.GradientStops;
        }

        if (gradientStops == null || gradientStops.Count < 2)
            return;

        int count = gradientStops.Count;

        // Початкові кольори беремо з XAML
        Color[] colors = gradientStops
            .Select(stop => stop.Color)
            .ToArray();

        var animation = new Animation(value =>
        {
            double position = value * count;

            int step = (int)Math.Floor(position);
            double progress = position - step;

            for (int i = 0; i < count; i++)
            {
                int fromIndex = (i + step) % count;
                int toIndex = (i + step + 1) % count;

                gradientStops[i].Color = LerpColor(
                    colors[fromIndex],
                    colors[toIndex],
                    progress
                );
            }
        });

        animation.Commit(
            _border,
            "PlayingGradient",
            rate: 32,
            length: 8000,
            easing: Easing.Linear,
            repeat: () => IsPlaying
        );
    }
    private static Color LerpColor(
        Color from,
        Color to,
        double amount)
    {
        float t = (float)amount;

        return new Color(
            from.Red + (to.Red - from.Red) * t,
            from.Green + (to.Green - from.Green) * t,
            from.Blue + (to.Blue - from.Blue) * t,
            from.Alpha + (to.Alpha - from.Alpha) * t
        );
    }

    private static float Wrap(double value)
    {
        value %= 1;

        if (value < 0)
            value += 1;

        return (float)value;
    }

    private void StopAnimation()
    {
        if (_border == null)
            return;

        _border.AbortAnimation("PlayingGradient");
    }
}