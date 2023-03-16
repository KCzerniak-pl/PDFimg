using Prism.Commands;
using Prism.Mvvm;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace PDFimg.ViewModels
{
    class PdfImgViewModel : BindableBase
    {
        // Path to the selected folder.
        private string _pathToFolderFull = null!;
        public string PathToFolderFull
        {
            get { return _pathToFolderFull; }
            set { SetProperty(ref _pathToFolderFull, value); }
        }

        private string _pathToFolderShort = "Click the button to select a folder containing PDF files.";
        public string PathToFolderShort
        {
            get { return _pathToFolderShort; }
            set { SetProperty(ref _pathToFolderShort, value); }
        }

        // Total files in the selected folder.
        private string _countFiles = "0";
        public string CountFiles
        {
            get { return _countFiles; }
            set { SetProperty(ref _countFiles, value); }
        }

        // Button to open the folder browser dialog.
        private ICommand? _folderBrowserDialog;
        public ICommand FolderBrowserDialog { get => _folderBrowserDialog ?? (_folderBrowserDialog = new DelegateCommand(ExecuteFolderBrowserDialog)); }

        // Folder browser dialog.
        private void ExecuteFolderBrowserDialog()
        {
            // Create a folder browser dialog using 'Ookii Dialogs'
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                // Get path to selected folder.
                int maxChar = 55;
                PathToFolderFull = dialog.SelectedPath;
                PathToFolderShort = PathToFolderFull.Length > maxChar ? $"{PathToFolderFull.Substring(0, maxChar)}..." : PathToFolderFull;

                // Count files (with chosen extension) in the selected folder.
                int countFiles = (from file in Directory.EnumerateFiles(dialog.SelectedPath, "*.pdf", SearchOption.TopDirectoryOnly) select file).Count();
                CountFiles = countFiles.ToString();
            }
        }
    }
}
