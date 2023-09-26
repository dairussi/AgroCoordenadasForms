using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroCoordenadas.Interface
{
    internal interface IFilter
    {
        Dictionary<string, List<string>> Filter(string text);

    }
}
