using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor.Data
{
    public class PoopImageData : INotifyPropertyChanged
    {
        private bool _isSelected;
        public string PoopImgName { get; private set; }
        public string ImgUrl { get; private set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }

        public PoopImageData(string poopImgName)
        {
            this.PoopImgName = poopImgName;
            this.ImgUrl = "/Assets/img/samplePoopImage/" + poopImgName;
            this.IsSelected = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
