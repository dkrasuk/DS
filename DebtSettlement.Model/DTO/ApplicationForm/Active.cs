﻿using WorkflowEngine.Client.Attributes;

namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class Active
    {
        public string PropertyType { get; set; }

        public string PropertyDescription { get; set; }

        public int? MembershipInterestOfProperty { get; set; }

        public double ApproximValue { get; set; }

        public string SourceOfInfo { get; set; }

        public string CommentOnProperty { get; set; }
    }
}
