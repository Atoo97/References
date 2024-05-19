using System;

namespace ELTE.Calculator.View
{
    /// <summary>
    /// Tájolással kapcsolatos viselkedés StackLayout vezérlőre.
    /// </summary>
    public class StackLayoutOrientationBehavior : Behavior<StackLayout>
    {
        protected override void OnAttachedTo(StackLayout bindable)
        {
            // feliratkozunk a méretváltozás eseményére
            bindable.SizeChanged += new EventHandler(Bindable_SizeChanged);

            // meg kell hívnunk az ős metódusát is
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(StackLayout bindable)
        {
            // leiratkozunk a méretváltozás eseményéről
            bindable.SizeChanged -= Bindable_SizeChanged;

            base.OnDetachingFrom(bindable);
        }

        private void Bindable_SizeChanged(object? sender, EventArgs e)
        {
            if (sender is StackLayout stackLayout)
            {
                // az eszköz tájolásának függvényében változtatunk a StackLayout tájolásán
                switch (DeviceDisplay.MainDisplayInfo.Orientation)
                {
                    case DisplayOrientation.Landscape:
                        if (stackLayout.Orientation != StackOrientation.Horizontal)
                            stackLayout.Orientation = StackOrientation.Horizontal;
                        break;
                    case DisplayOrientation.Portrait:
                        if (stackLayout.Orientation != StackOrientation.Vertical)
                            stackLayout.Orientation = StackOrientation.Vertical;
                        break;
                }
            }
        }
    }
}
