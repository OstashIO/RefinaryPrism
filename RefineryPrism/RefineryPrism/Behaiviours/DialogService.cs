using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using Prism.Services.Dialogs;

namespace RefineryPrism.Behaiviours
{
    public class DialogService : IDialogService
    {
        public string Show()
        {
            var saveFileDialog = new SaveFileDialog {Filter = "Excel (*.xlsx)|*.xlsx" };

            var temp = saveFileDialog.ShowDialog();

            if (temp.HasValue)
            {
                if (temp.Value)
                {
                    return saveFileDialog.FileName;
                }
            }

            return string.Empty;
        }
    }
}
