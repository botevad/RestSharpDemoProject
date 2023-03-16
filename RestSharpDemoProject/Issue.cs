using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpDemoProject
{
    [Serializable]
      public class Issue
    {
        public int number { get; set; }
        public string title { get; set; }


    }
}
