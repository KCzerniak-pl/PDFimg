using Prism.Mvvm;
using System;

namespace PDFimg.Models
{
    public class JsonDataModel : BindableBase
    {
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
    }
}
