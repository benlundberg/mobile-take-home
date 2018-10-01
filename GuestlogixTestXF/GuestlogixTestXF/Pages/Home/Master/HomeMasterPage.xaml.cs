using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuestlogixTestXF
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeMasterPage : MasterDetailPage
    {
        public HomeMasterPage()
        {
            InitializeComponent();

            Master = ViewContainer.Current.CreatePage(new MasterViewModel(this));
        }
    }
}