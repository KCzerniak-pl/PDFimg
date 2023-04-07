using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PDFimg.Models
{
    public class PdfDataModel : BindableBase
    {
        public PdfDataModel()
        {
            Files = new List<string>();
            ImgData = new ObservableCollection<ImgDataModel>();
        }

        private Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
            set { SetProperty(ref _guid, value); }
        }

        private string _name = default!;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _pathToFolder = default!;
        public string PathToFolder
        {
            get { return _pathToFolder; }
            set { SetProperty(ref _pathToFolder, value); }
        }

        private ICollection<string> _files = default!;
        public ICollection<string> Files
        {
            get { return _files; }
            set { SetProperty(ref _files, value); }
        }

        private int _countFiles = default!;
        public int CountFiles
        {
            get { return _countFiles; }
            set { SetProperty(ref _countFiles, value); }
        }

        private ObservableCollection<ImgDataModel> _imgData = default!;
        public ObservableCollection<ImgDataModel> ImgData
        {
            get { return _imgData; }
            set { SetProperty(ref _imgData, value); }
        }
    }
}
