using System.Windows.Input;
using Xamarin.Forms;

namespace GuestlogixTestXF
{
    public class ExtendedEntry : Entry
    {
        public ExtendedEntry()
        {
            this.TextChanged += ExtendedEntry_TextChanged;
        }

        private void ExtendedEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChangedCommand?.Execute(e.NewTextValue);
        }

        public ICommand FocusCommand
        {
            get { return (ICommand)GetValue(FocusCommandProperty); }
            set { SetValue(FocusCommandProperty, value); }
        }

        public static readonly BindableProperty FocusCommandProperty =
            BindableProperty.Create(
                propertyName: "FocusCommand",
                returnType: typeof(ICommand),
                declaringType: typeof(ExtendedEntry),
                defaultValue: null);

        public ICommand TextChangedCommand
        {
            get { return (ICommand)GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        public static readonly BindableProperty TextChangedCommandProperty =
            BindableProperty.Create(
                propertyName: "TextChangedCommand",
                returnType: typeof(ICommand),
                declaringType: typeof(ExtendedEntry),
                defaultValue: null);

        private bool focusOnStart;
        public bool FocusOnStart
        {
            set
            {
                focusOnStart = value;
                if (focusOnStart)
                {
                    this.Focus();
                }
            }
            get { return focusOnStart; }
        }
    }
}
