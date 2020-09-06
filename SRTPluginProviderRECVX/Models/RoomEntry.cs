using System;

namespace SRTPluginProviderRECVX.Models
{
    public class RoomEntry : BaseNotifyModel
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                    OnPropertyChanged("Name");
                }
            }
        }

        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                if (_isLoaded != value)
                {
                    _isLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get
            {
                return String.Empty;
            }
        }
    }
}