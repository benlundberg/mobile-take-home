using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GuestlogixTestXF
{
    public class ExtendedMap : Map
    {
        public ExtendedMap()
        {
            void BindingContextChanged(object sender, EventArgs e)
            {
                this.BindingContextChanged -= BindingContextChanged;
                this.LoadedCommand?.Execute(this);
            }

            this.BindingContextChanged += BindingContextChanged;
        }

        public ICommand LoadedCommand
        {
            get { return (ICommand)GetValue(LoadedCommandProperty); }
            set { SetValue(LoadedCommandProperty, value); }
        }

        public static readonly BindableProperty LoadedCommandProperty =
            BindableProperty.Create(
                propertyName: "LoadedCommand",
                returnType: typeof(ICommand),
                declaringType: typeof(ExtendedMap),
                defaultValue: null);

        public List<Position> RouteCoordinates { get; set; } = new List<Position>();
    }
}
