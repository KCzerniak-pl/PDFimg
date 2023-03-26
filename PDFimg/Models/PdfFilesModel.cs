using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PDFimg.Models
{
    public class PdfFilesModel : INotifyPropertyChanged
    {
        // Declare the event.
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _pathToFolder = default!;
        public string PathToFolder
        {
            get { return _pathToFolder; }
            set { _pathToFolder = value; OnPropertyChanged(); }
        }

        private ICollection<string> _files = default!;
        public ICollection<string> Files
        {
            get { return _files; }
            set { _files = value; OnPropertyChanged(); }
        }

        private int _countFiles = default!;
        public int CountFiles
        {
            get { return _countFiles; }
            set { _countFiles = value; OnPropertyChanged(); }
        }

        // Create the OnPropertyChanged method to raise the event.
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
