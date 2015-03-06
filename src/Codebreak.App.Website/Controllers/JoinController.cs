using Codebreak.App.Website.Models.Authservice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;

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
    
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "error_no_name")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "error_no_password")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "error_password_short")]
        public string Password { get; set; }
    }

    public class AccountTicket : IPrincipal
    {
        public IIdentity Identity
        {
            get;
            set;
        }
        public bool IsInRole(string role)
        {
            return false;
        }

        public Account Account
        {
            get;
            set;
        }

        public AccountTicket(string name)
        {
            Identity = new GenericIdentity(name);
            Account = AccountRepository.Instance.GetByName(name);
        }
    }

    public class AccountComparer : IEqualityComparer<AccountTicket>
    {        
        public bool Equals(AccountTicket x, AccountTicket y)
        {
            return x.Account.Id == y.Account.Id;
        }

        public int GetHashCode(AccountTicket obj)
        {
            return obj.Account.Id.GetHashCode();
        }
    }

    public class JoinController : WrappedController
    {
        private static object RegisterLock = new object();
        
        public ActionResult Login()
        {
            ViewBag.LoginErrors = new List<string>();
            ViewBag.Name = "";
            ViewBag.Password = "";

            return View();
        }


        [ChildActionOnly]
        [OutputCache(Duration = GENERIC_CACHE_DURATION)]
        public ActionResult LoginContent()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            bool valid = false;
            var errors = new List<string>();
            Account account = null;
            ViewBag.LoginErrors = errors;
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                    foreach (var error in state.Value.Errors)
                        errors.Add(error.ErrorMessage);
            }
            else
            {
                account = AccountRepository.Instance.GetByName(model.Name);
                if (account == null)
                {
                    errors.Add("error_invalid_credentials");
                }
                else
                {
                    valid = true;
                }
            }

            if (!valid)
            {
                ViewBag.Name = model.Name == null ? "" : model.Name;

                return View();
            }
            else
            {
                FormsAuthentication.SetAuthCookie(account.Name, true);
                return Redirect(GetRedirectUrl());
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Register", "Join");
            }

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

        [ChildActionOnly]
        [OutputCache(Duration = GENERIC_CACHE_DURATION)]
        public ActionResult RegisterContent()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AccountModel model)
        {
            bool valid = false;
            var errors = new List<string>();
            Account account = null;
            ViewBag.RegisterErrors = errors;

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                    foreach (var error in state.Value.Errors)
                        errors.Add(error.ErrorMessage);
            }
            else
            {
                lock (RegisterLock)
                {
                    account = AccountRepository.Instance.GetByName(model.Name);
                    if (account != null)
                    {
                        errors.Add("error_name_exists");
                    }
                    else if ((account = AccountRepository.Instance.GetByPseudo(model.Pseudo)) != null)
                    {
                        errors.Add("error_pseudo_exists");
                    }
                    else if ((account = AccountRepository.Instance.Create(model.Name, model.Pseudo, model.Password, model.Email, model.Question, model.Answer)) == null)
                    {
                        errors.Add("error_database");
                    }
                    else
                    {
                        valid = true;
                    }
                }
            }

            if (!valid)
            {
                ViewBag.Name = model.Name == null ? "" : model.Name;
                ViewBag.Pseudo = model.Pseudo == null ? "" : model.Pseudo;
                ViewBag.Password = model.Password == null ? "" : model.Password;
                ViewBag.Email = model.Email == null ? "" : model.Email;
                ViewBag.Question = model.Question == null ? "" : model.Question;
                ViewBag.Answer = model.Answer == null ? "" : model.Answer;

                return View();
            }
            else
            {
                FormsAuthentication.SetAuthCookie(account.Name, true);

                return Redirect(GetRedirectUrl());
            }
        }

        public ActionResult Download()
        {
            return View();
        }
        
        [ChildActionOnly]
        [OutputCache(Duration = GENERIC_CACHE_DURATION)]
        public ActionResult DownloadContent()
        {
            return PartialView();
        }
    }
}