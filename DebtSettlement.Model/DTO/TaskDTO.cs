using System;
using System.Collections.Generic;
using WorkflowEngine.Client.Attributes;
using WorkflowEngine.Client.Models;

namespace DebtSettlement.Model.DTO
{
    public class TaskDTO : ParametersConverterBaseModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>    
        public Guid? ID { get; set; }

        public Guid? ProcessId {get; set;}
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [PersistanceProcessParameter]
        public string Title { get; set; }


        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [PersistanceProcessParameter]
        public string Description { get; set; }


        /// <summary>
        /// Gets or sets the Comment.
        /// </summary>
        /// <value>
        /// The Comment.
        /// </value>
        [PersistanceProcessParameter]
        public string Comment { get; set; }


        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [PersistanceProcessParameter]
        public string TaskType { get; set; }


        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }


        /// <summary>
        /// Gets or sets the date create.
        /// </summary>
        /// <value>
        /// The date create.
        /// </value>
        [PersistanceProcessParameter]
        public DateTime? CreateDate { get; set; }


        /// <summary>
        /// Gets or sets the planed date.
        /// </summary>
        /// <value>
        /// The planed date.
        /// </value>
        [PersistanceProcessParameter]
        public DateTime? PlannedDate { get; set; }


        /// <summary>
        /// Gets or sets the Observer.
        /// </summary>
        /// <value>
        /// The Observer.
        /// </value>
        [PersistanceProcessParameter]
        public string Observer { get; set; }


        /// <summary>
        /// Gets or sets the responsible.
        /// </summary>
        /// <value>
        /// The responsible.
        /// </value>
        [PersistanceProcessParameter]
        public string Responsible { get; set; }


        /// <summary>
        /// Gets or sets the initiator.
        /// </summary>
        /// <value>
        /// The initiator.
        /// </value>
        [PersistanceProcessParameter]
        public string Initiator { get; set; }


        /// <summary>
        /// Gets or sets the agreement identifier.
        /// </summary>
        /// <value>
        /// The agreement identifier.
        /// </value>
        [PersistanceProcessParameter]
        public int? AgreementId { get; set; }

        /// <summary>
        /// Gets or sets agreement process id
        /// </summary>
        [PersistanceProcessParameter]
        public string AgreementProcessId { get; set; }

        /// <summary>
        /// Agreement info for current task
        /// </summary>
        public object Agreement { get; set; }

        /// <summary>
        /// Available states for current workflow instance
        /// </summary>
        public List<WorkflowState> AvailableStates { get; set; }

        /// <summary>
        /// Available commands for current workflow instance
        /// </summary>
        public List<WorkflowCommand> AvailableCommands { get; set; }

        /// <summary>
        /// Gets or sets the initiator.
        /// </summary>
        /// <value>
        /// The initiator.
        /// </value>
        [PersistanceProcessParameter]
        public int? NotLegalTypeActionId { get; set; }


        /// <summary>
        /// Gets or sets the initiator.
        /// </summary>
        /// <value>
        /// The initiator.
        /// </value>
        [PersistanceProcessParameter]
        public int? LegalActionTypeId { get; set; }


        /// <summary>
        /// Gets or sets the initiator.
        /// </summary>
        /// <value>
        /// The initiator.
        /// </value>
        [PersistanceProcessParameter]
        public int? TaskCategoryId { get; set; }
    }
}
