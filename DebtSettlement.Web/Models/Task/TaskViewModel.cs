using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DebtSettlement.Model.Enums;

namespace DebtSettlement.Web.Models.Task
{
    /// <summary>
    /// Class TaskViewModel.
    /// </summary>
    public class TaskViewModel
    {
        /// <summary>
        /// Gets or sets the IsMultiTask parameter.
        /// </summary>
        /// <value>The title.</value>
        [Display(Name = "Список договоров")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public bool IsMultiTask { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Display(Name = "Краткое название задачи")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание задачи")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        [Display(Name = "Комментарий")]
        [Editable(false)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TaskViewModel"/> is type.
        /// </summary>
        /// <value><c>true</c> if type; otherwise, <c>false</c>.</value>
        [Display(Name = "Важная задача")]
        public bool Type { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [Display(Name = "Статус задачи")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the planned date.
        /// </summary>
        /// <value>The planned date.</value>
        //[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата исполнения")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string PlannedDate { get; set; }

        /// <summary>
        /// Gets or sets the responsible.
        /// </summary>
        /// <value>The responsible.</value>
        [Display(Name = "Исполнитель")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Responsible { get; set; }

        /// <summary>
        /// Bind ResponsibleFullName to model.
        /// </summary>
        /// <value>The ResponsibleFullName.</value>
        public string ResponsibleFullName { get; set; }

        /// <summary>
        /// Gets or sets the observer.
        /// </summary>
        /// <value>The observer.</value>
        [Display(Name = "Наблюдатель")]
        public string Observer { get; set; }

        /// <summary>
        /// Bind ResponsibleFullName to model.
        /// </summary>
        /// <value>The ResponsibleFullName.</value>
        public string ObserverFullName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AssignedUser AssignedResponsible { get; set; }

        /// <summary>
        /// Gets or sets the initiator.
        /// </summary>
        /// <value>The initiator.</value>
        [Display(Name = "Постановщик")]
        public string Initiator { get; set; }

        /// <summary>
        /// Gets or sets the agreement identifier.
        /// </summary>
        /// <value>The agreement identifier.</value>
        [Display(Name = "Номер договора")]
        [Required]
        public string AgreementId { get; set; }

        /// <summary>
        /// Get or set the agreement process id
        /// </summary>
        [Display(Name = "Номер процеса договора")]
        public string AgreementProcessId { get; set; }

        /// <summary>
        /// Gets or sets the not legal type action identifier.
        /// </summary>
        /// <value>The not legal type action identifier.</value>
        [Display(Name = "Тип действия (кроме Legal)")]
        public int? NotLegalTypeActionId { get; set; }

        /// <summary>
        /// Gets or sets the general action types list.
        /// </summary>
        /// <value>The general action types list.</value>
        public List<SelectListItem> GeneralActionTypesList { get; set; }

        /// <summary>
        /// Gets or sets the legal action type identifier.
        /// </summary>
        /// <value>The legal action type identifier.</value>
        [Display(Name = "Тип действия (для Legal)")]
        public int? LegalActionTypeId { get; set; }

        /// <summary>
        /// Gets or sets the legal action types list.
        /// </summary>
        /// <value>The legal action types list.</value>
        public List<SelectListItem> LegalActionTypesList { get; set; }

        /// <summary>
        /// Gets or sets the task category identifier.
        /// </summary>
        /// <value>The legal task category identifier.</value>
        [Display(Name = "Категория задач")]
        public int? TaskCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the task category list.
        /// </summary>
        /// <value>The task categories list.</value>
        public List<SelectListItem> TaskCategoryList { get; set; }

        /// <summary>
        /// Gets or sets the full name of the person.
        /// </summary>
        /// <value>The full name of the person.</value>
        [Display(Name = "ФИО клиента")]
        public string PersonFullName { get; set; }

        /// <summary>
        /// Gets or sets the product code.
        /// </summary>
        /// <value>The product code.</value>
        [Display(Name = "Продукт")]
        public string ProductCode { get; set; }


        /// <summary>
        /// Gets or sets the open date close date.
        /// </summary>
        /// <value>The open date close date.</value>
        [Display(Name = "Дата начала-Дата окончания")]
        // ReSharper disable once InconsistentNaming
        public string OpenDate_CloseDate { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>The number.</value>
        [Display(Name = "Номер сделки")]
        public string Number { get; set; }
    }
}