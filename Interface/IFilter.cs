using AgroCoordenadas.Service;
using System.Collections.Generic;

namespace AgroCoordenadas.Interface
{
    internal interface IFilter
    {
        FilterService.FilteredData Filter(string text);
    }
}
