using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessor
{
    internal class Model
    {
        public Model()
        {

        }
        public async void WaitFor(int seconds)
        {
            await Task.Delay(seconds * 100);
        }
    }
}
