using System.ComponentModel.DataAnnotations;

namespace Elevate.Accounts.Web.Areas.UserAccount.Models
{
    public class ChangeMobileFromCodeInputModel
    {
        [Required]
        public string Code { get; set; }
    }
    
}