using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor.Data
{
    public class SamplePoopImageData : ObservableCollection<PoopImageData>
    {
        public SamplePoopImageData()
        {
            this.Add(new PoopImageData("Poop1.jpg"));
            this.Add(new PoopImageData("Poop2.jpg"));
            this.Add(new PoopImageData("Poop3.jpg"));
            this.Add(new PoopImageData("Poop4.jpg"));
            this.Add(new PoopImageData("Poop5.jpg"));
            this.Add(new PoopImageData("Poop6.jpg"));
            this.Add(new PoopImageData("Poop7.jpg"));
            this.Add(new PoopImageData("Poop8.jpg"));
            this.Add(new PoopImageData("Poop9.jpg"));
            this.Add(new PoopImageData("Poop10.jpg"));
            this.Add(new PoopImageData("Poop11.jpg"));
            this.Add(new PoopImageData("Poop12.jpeg"));
            this.Add(new PoopImageData("Poop13.jpg"));
            this.Add(new PoopImageData("Poop14.jpg"));
        }
    }
}
