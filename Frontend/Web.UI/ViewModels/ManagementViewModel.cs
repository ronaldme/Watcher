using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Web.UI.ViewModels
{
    public class ManagementViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources.Resources), Name = "NotifyHours")]
        [Range(0, 23)]
        public int SelectedNotifyHour { get; set; }

        public SelectList Hours { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.Resources), Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public string OldEmail { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "NotifyDayLater")]
        public bool NotifyDayLater { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "NotifyMyAndroidKey")]
        public string NotifyMyAndroidKey { get; set; }
    }
}