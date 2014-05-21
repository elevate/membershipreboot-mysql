using System.ComponentModel.DataAnnotations;

namespace Elevate.Accounts.Web.Areas.UserAccount.Models
{
    public class ChangeUsernameInputModel
    {
        [Required]
        public string NewUsername { get; set; }
    }
}