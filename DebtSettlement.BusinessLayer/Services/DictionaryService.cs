using DebtSettlement.BusinessLayer.Services.Interfaces;
using DebtSettlement.Model.Dictionary;
using DebtSettlement.Model.Dictionary.AdvancedDictionary;
using Dictionaries;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtSettlement.BusinessLayer.Services
{
    public class DictionaryService : IDictionaryService
    {
        IDictionaryOperations DictionaryOperations;
        public DictionaryService(IDictionaryOperations _DictionaryOperations)
        {
            DictionaryOperations = _DictionaryOperations;
        }
        /// <summary>
        /// Get Dictionary All
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DictionaryItemValue>> GetDictionaryByNameAsync(string name, string version)
        {
            var dictionary = await DictionaryOperations.GetDictionaryByNameAndVersionAsync(name, version);
            return dictionary.Items.Select(i => (i.Value as JObject).ToObject<DictionaryItemValue>()).ToList();
        }

        /// <summary>
        /// Get Dictionary DS.PortfolioSegment
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PortfolioSegment>> GetDictionaryPortfolioSegmentByNameAsync(string name, string version)
        {
            var dictionary = await DictionaryOperations.GetDictionaryByNameAndVersionAsync(name, version);
            return dictionary.Items.Select(i => (i.Value as JObject).ToObject<PortfolioSegment>()).ToList();
        }
    }
}
