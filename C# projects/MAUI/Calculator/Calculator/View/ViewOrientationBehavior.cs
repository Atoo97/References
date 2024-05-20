using System;

namespace ELTE.Calculator.View
{
    /// <summary>
    /// Tájolással kapcsolatos viselkedés vezérlőkre.
    /// </summary>
    public class ViewOrientationBehavior : Behavior<Microsoft.Maui.Controls.View>
    {
        protected override void OnAttachedTo(Microsoft.Maui.Controls.View bindable)
        {
            // feliratkozunk a méretváltozás eseményére
            bindable.SizeChanged += new EventHandler(Bindable_SizeChanged);

            // meg kell hívnunk az ős metódusát is
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Microsoft.Maui.Controls.View bindable)
        {
            // leiratkozunk a méretváltozás eseményéről
            bindable.SizeChanged -= Bindable_SizeChanged;

            base.OnDetachingFrom(bindable);
        }

        private void Bindable_SizeChanged(object? sender, EventArgs e)
        {
            if (sender is Microsoft.Maui.Controls.View view)
            {
                // az eszköz tájolásának függvényében változtatunk a vezérlő elhelyezkedésén
                switch (DeviceDisplay.MainDisplayInfo.Orientation)
                {
                    case DisplayOrientation.Landscape:
                        if (!view.HorizontalOptions.Equals(LayoutOptions.Fill))
                            view.HorizontalOptions = LayoutOptions.Fill;
                        break;
                    case DisplayOrientation.Portrait:
                        if (!view.HorizontalOptions.Equals(LayoutOptions.Center))
                            view.HorizontalOptions = LayoutOptions.Center;
                        break;
                }
            }
        }
    }
}
