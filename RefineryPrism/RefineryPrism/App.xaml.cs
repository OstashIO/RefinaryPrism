using Prism.Ioc;
using RefineryPrism.Services;
using RefineryPrism.Services.Interfaces;
using RefineryPrism.Views;
using System.Windows;
using RefineryPrism.Behaiviours;
using RefineryPrism.DataAccessLayer;

namespace RefineryPrism
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<IDataReader, DataReader>();
            containerRegistry.RegisterSingleton<IDataWriter, DataWriter>();
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
        }
    }
}
