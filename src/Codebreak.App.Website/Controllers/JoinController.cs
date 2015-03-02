using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebreak.App.Website.Controllers
{
    public class AccountModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "error_no_name")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "error_name_short")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "error_no_pseudo")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "error_pseudo_short")]
        public string Pseudo { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "error_no_password")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "error_password_short")]
        public string Password { get; set; }
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage="error_password_confirm")]
        public string PasswordConfirmation { get; set; }
        [EmailAddress(ErrorMessage="error_invalid_mail")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "error_no_question")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "error_question_short")]
        public string Question { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "error_no_answer")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "error_answer_short")]
        public string Answer { get; set; }
    }

    public class JoinController : WrappedController
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        
        public ActionResult Register()
        {
            ViewBag.RegisterErrors = new List<string>();
            ViewBag.Name = "";
            ViewBag.Pseudo = "";
            ViewBag.Password = "";
            ViewBag.Confirm = "";
            ViewBag.Email = "";
            ViewBag.Question = "";
            ViewBag.Answer = "";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AccountModel account)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Name = account.Name == null ? "" : account.Name;
                ViewBag.Pseudo = account.Pseudo == null ? "" : account.Pseudo;
                ViewBag.Password = account.Password == null ? "" : account.Password;
                ViewBag.Email = account.Email == null ? "" : account.Email;
                ViewBag.Question = account.Question == null ? "" : account.Question;
                ViewBag.Answer = account.Answer == null ? "" : account.Answer;
                var errors = new List<string>();
                foreach(var state in ModelState)                
                    foreach (var error in state.Value.Errors)
                        errors.Add(error.ErrorMessage);

                ViewBag.RegisterErrors = errors;

                return View();
            }

            return Redirect(Url.Action("Index", "Home"));
        }

        public ActionResult Download()
        {
            return View();
        }
    }
}