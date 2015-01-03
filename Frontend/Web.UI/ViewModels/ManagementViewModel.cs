using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Web.UI.ViewModels
{
    public class ManagementViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources.Translations.Resources), Name = "NotifyHours")]
        [Range(0, 23)]
        public int SelectedNotifyHour { get; set; }

        public SelectList Hours { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.Translations.Resources), Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public string OldEmail { get; set; }
    }
}