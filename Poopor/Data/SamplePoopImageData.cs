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
            this.Add(new PoopImageData("Poop1"));
            this.Add(new PoopImageData("Poop2"));
            this.Add(new PoopImageData("Poop3"));
            this.Add(new PoopImageData("Poop4"));
            this.Add(new PoopImageData("Poop5"));
            this.Add(new PoopImageData("Poop6"));
            this.Add(new PoopImageData("Poop7"));
            this.Add(new PoopImageData("Poop8"));
            this.Add(new PoopImageData("Poop9"));
            this.Add(new PoopImageData("Poop10"));
            this.Add(new PoopImageData("Poop11"));
            this.Add(new PoopImageData("Poop12"));
            this.Add(new PoopImageData("Poop13"));
        }
    }
}
