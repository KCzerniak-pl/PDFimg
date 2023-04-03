using PDFimg.ViewModels.Dialogs;
using PDFimg.Views;
using PDFimg.Views.Dialogs;
using Prism.DryIoc;
using Prism.Ioc;
using System.Windows;

namespace PDFimg
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindowView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<ImgDataView, ImgDataViewModel>("ImgDataDialog");
        }
    }
}
