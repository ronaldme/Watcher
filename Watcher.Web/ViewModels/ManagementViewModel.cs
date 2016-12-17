using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Watcher.Web.Translations;

namespace Watcher.Web.ViewModels
{
    public class ManagementViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "NotifyHours")]
        [Range(0, 23)]
        public int SelectedNotifyHour { get; set; }

        public SelectList Hours { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "GetEmailNotifications")]
        public bool GetEmailNotifications { get; set; }

        public string OldEmail { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "NotifyDayLater")]
        public bool NotifyDayLater { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "NotifyMyAndroidKey")]
        public string NotifyMyAndroidKey { get; set; }
    }
}