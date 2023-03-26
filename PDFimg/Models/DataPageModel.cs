using PDFimg.Interfaces;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PDFimg.Models
{
    public class DataPageModel : INotifyPropertyChanged, IDeepCopy<DataPageModel>
    {
        // Declare the event.
        public event PropertyChangedEventHandler? PropertyChanged;

        public Guid Guid { get; init; }

        private string _name = default!;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private string _pathToImage = default!;
        public string PathToImage
        {
            get { return _pathToImage; }
            set { _pathToImage = value; OnPropertyChanged(); }
        }

        private string _pageNumbers = default!;
        public string PageNumbers
        {
            get { return _pageNumbers; }
            set { _pageNumbers = value; OnPropertyChanged(); }
        }

        private int _positionX;
        public int PositionX
        {
            get { return _positionX; }
            set { _positionX = value; OnPropertyChanged(); }
        }

        private int _positionY;
        public int PositionY
        {
            get { return _positionY; }
            set { _positionY = value; OnPropertyChanged(); }
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

        // Make a deep copy of the current object.
        public DataPageModel DeepCopy()
        {
            return new DataPageModel
            {
                Guid = this.Guid,
                Name = this.Name,
                PathToImage = this.PathToImage,
                PageNumbers = this.PageNumbers,
                PositionX = this.PositionX,
                PositionY = this.PositionY
            };
        }
    }
}
