using System;
using System.Windows.Input;
using TestMachineFrontend1.ViewModel;

namespace TestMachineFrontend1.Commands
{
    public class SetHICommand : ICommand
    {
        RemoteControllerViewModel remoteVM;
        DebugViewModel debugVM;

        public SetHICommand()
        {
            remoteVM = MainWindowViewModel.CurrentViewModelRemoteController;
            debugVM = MainWindowViewModel.CurrentViewModelDebug;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {

            remoteVM.Model = remoteVM.SelectedHI.Content.ToString();
            remoteVM.Family = remoteVM.SelectedHI.Name;

            if (remoteVM.Model == "" || remoteVM.Family == "")
            {
                debugVM.AddDebugInfo("SetHICommand", "selected item is not valid.");
            }

            try
            {

                await remoteVM.SelectedRaspiItem.raspi.SetHI(remoteVM.Family, remoteVM.Model);
                remoteVM.detectVC_type();
                debugVM.AddDebugInfo("SetHICommand", remoteVM.Family + ", " + remoteVM.Model);
            }
            catch (Exception e)
            {
                debugVM.AddDebugInfo("SetHICommand", "Failed");
            }
        }
    }
}
