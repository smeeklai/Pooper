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
            this.Add(new PoopImageData("poop1"));
            this.Add(new PoopImageData("poop2"));
            this.Add(new PoopImageData("poop3"));
            this.Add(new PoopImageData("poop4"));
            this.Add(new PoopImageData("poop5"));
            this.Add(new PoopImageData("poop6"));
            this.Add(new PoopImageData("poop7"));
            this.Add(new PoopImageData("poop8"));
            this.Add(new PoopImageData("poop9"));
        }
    }
}
