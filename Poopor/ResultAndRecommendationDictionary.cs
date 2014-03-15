using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Poopor
{
    [DataContact]
    public class ResultAndRecommendationDictionary
    {
        [datamember]
        public string Key { get; set; }

        [datamember]
        public List<string> Value { get; set; }

        public ResultAndRecommendationDictionary()
        {

        }

        public ResultAndRecommendationDictionary(string key, List<string> value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
