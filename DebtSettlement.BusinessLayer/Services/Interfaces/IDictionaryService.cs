using DebtSettlement.Model.Dictionary;
using DebtSettlement.Model.Dictionary.AdvancedDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtSettlement.BusinessLayer.Services.Interfaces
{
    public interface IDictionaryService
    {
        Task<IEnumerable<DictionaryItemValue>> GetDictionaryByNameAsync(string name, string version);
        Task<IEnumerable<PortfolioSegment>> GetDictionaryPortfolioSegmentByNameAsync(string name, string version);
    }
}
