﻿using System.ComponentModel.DataAnnotations;

namespace Elevate.Accounts.Web.Areas.UserAccount.Models
{
    public class LoginInputModel
    {
        [Required]
        [Display(Name="Username or Email")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        [ScaffoldColumn(false)]
        public string ReturnUrl { get; set; }
    }
}