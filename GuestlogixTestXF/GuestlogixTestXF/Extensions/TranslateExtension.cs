using GuestlogixTestXF.Core;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuestlogixTestXF
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public TranslateExtension()
        {
            translateHelper = ComponentContainer.Current.Resolve<ITranslateHelper>();
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
            {
                return "";
            }

            return translateHelper.Translate(Text);
        }

        private ITranslateHelper translateHelper;
    }
}
