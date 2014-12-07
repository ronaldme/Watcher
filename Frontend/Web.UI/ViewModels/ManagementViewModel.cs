using System.ComponentModel.DataAnnotations;

namespace Web.UI.ViewModels
{
    public class ManagementViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources.Translations.Resources), Name = "NotifyHours")]
        [Range(0, 23)]
        public int NotifyHour { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources.Translations.Resources), Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public string OldEmail { get; set; }
    }
}