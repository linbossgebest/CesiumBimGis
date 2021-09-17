using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cesium.ViewModels
{
    public class SceneTreeModel
    {
        public class Rootobject
        {
            public Scene[] scenes { get; set; }
        }

        public class Scene
        {
            public Child[] children { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public float[] sphere { get; set; }
            public string type { get; set; }
        }

        public class Child
        {
            public Child[] children { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public float[] sphere { get; set; }
            public string type { get; set; }
        }
    }
}
