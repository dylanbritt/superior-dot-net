using Superior.BusinessLogic.Interfaces;
using Superior.Domain.Enums;
using Superior.Domain.Models;
using System.Web.Mvc;

namespace superior_dot_net.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var result = _userService.AuthenticateUser(user);

            string error = null;

            switch (result)
            {
                case AuthenticationCode.Unauthenticated:
                    error = "Invalid Username or Password.";
                    break;
                case AuthenticationCode.Locked:
                    error =
                        @"Invalid Username or Password.
                        Your account is temporarily locked. 
                        Please try again after a minute.";
                    break;
            }

            ViewBag.Error = error;
            return View();
        }

        public ActionResult CreateLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateLogin(User user)
        {
            _userService.CreateUser(user);

            return null;
        }
    }
}