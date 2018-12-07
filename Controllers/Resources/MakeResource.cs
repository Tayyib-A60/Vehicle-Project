using System.Collections;
using System.Collections.Generic;

namespace Vega.Controllers.Resources {
    public class MakeResource : KeyValuePairResource {
        public ICollection<KeyValuePairResource> Models { get; set; }
        public MakeResource () {
            Models = new List<KeyValuePairResource> ();
        }
    }
}