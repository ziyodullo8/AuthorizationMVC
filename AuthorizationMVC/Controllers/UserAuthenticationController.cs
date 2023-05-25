using AuthorizationMVC.Models.DTO;
using AuthorizationMVC.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationMVC.Controllers
{
    public class UserAuthentication : Controller
    {
        private readonly IUserAuthService _service;
        public UserAuthentication(IUserAuthService service)
        {
            _service = service;
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            model.Role = "user";
            var result=await _service.RegistrationAsynnc(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
           
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.LOginAsync(model);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Display","Dashboard");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _service.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        //public async Task<IActionResult> Reg()
        //{
        //    var model = new RegistrationModel()
        //    {
        //        UserName = "admin",
        //        Name = "ziyo",
        //        Email = "admin@gmail.com",
        //        Password = "admin@12345#",
        //    };
        //    model.Role = "admin";
        //    var result =await _service.RegistrationAsynnc(model);
        //    return Ok(result);
        //}
    }
}
