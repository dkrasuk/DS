namespace DebtSettlement.Model.Dictionary
{
    /// <summary>
    /// Class DictionaryItem.
    /// </summary>
    public class DictionaryItem
    {
        /// <summary>
        /// Gets or sets the value identifier.
        /// </summary>
        /// <value>The value identifier.</value>
        public string ValueId { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public DictionaryItemValue Value { get; set; }
    }
}