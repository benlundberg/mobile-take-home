using GuestlogixTestXF.Core;

namespace GuestlogixTestXF.Android
{
    public class Bootstrapper_Droid
    {
        public static void Initialize()
        {
            // Register common types
            Bootstrapper.RegisterTypes();

            // Register device specific types
            RegisterTypes();
        }

        private static void RegisterTypes()
        {
            // Helpers
            ComponentContainer.Current.Register<ILocalizeHelper, LocalizeHelper_Droid>();
        }
    }
}
