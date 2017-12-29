namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class DebtSettlementCreationResultDTO
    {
        /// <summary>
        /// Gets or sets the agreement identifier.
        /// </summary>
        /// <value>The agreement identifier.</value>
        public int AgreementId { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        /// <value>The error description.</value>
        public string Description { get; set; }
    }
}
