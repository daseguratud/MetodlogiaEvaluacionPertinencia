using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifaeTools
{
    public interface ISyncUpdate
    {
        public void Start();
        public void Finish();
        public void Update(string Message,int Max,int Min,int Progress);
    }
}
