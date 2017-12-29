using System;
using AlfaBank.Logger;

namespace DebtSettlement.BusinessLayer
{
    public class DebtSettlementException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DebtSettlementException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="logger">The logger.</param>
        public DebtSettlementException(string message, ILogger logger)
            :base(message)
        {
            logger.Warning(message);
        }
    }
}
