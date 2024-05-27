using Microsoft.AspNetCore.Mvc;
using UI.ViewModels.Users;
using WebAPI.Controllers;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using NuGet.ProjectModel;
using UI.Enums;
using WebAPI;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Net;
using Azure;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using UI.ActionFilters;
using WebAPI.Authentication;

namespace UI.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(CreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.IsMale = model.Gender == 0;

            HttpResponseMessage response = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKeyHeaderValue);
                string data=JsonConvert.SerializeObject(model);
                StringContent content=new StringContent(data,Encoding.UTF8,
                    "application/json");
                response = client.PostAsync(client.BaseAddress
                    +"Users/PostUser", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    data = response.Content.ReadAsStringAsync().Result;
                    JObject result = JObject.Parse(data);
                    this.HttpContext.Session.SetString("loggedUser", result.GetValue<string>("id"));
                    return RedirectToAction("Profile", new { id = result.GetValue<int>("id") });
                }
                else
                {
                    return View(model);
                }
            }

        }

        [AuthActionFilter]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (int.Parse(this.HttpContext.Session.GetString("loggedUser")) != id)
            {
                return RedirectToAction("Index", "Home");
            }
            EditVM model = new EditVM();
            HttpResponseMessage response = new HttpResponseMessage();
            JObject data=new JObject();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKeyHeaderValue);
                response = client.GetAsync(client.BaseAddress
                    + "Users/GetUser/"+id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    data=JObject.Parse(result);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            model.Id = data.GetValue<int>("id");
            model.Username = data.GetValue<string>("username");
            model.FirstName = data.GetValue<string>("firstName");
            model.LastName = data.GetValue<string>("lastName");
            model.BirthDate = DateOnly.Parse(data["birthDate"].ToString());
            model.Password=data.GetValue<string>("password");
            if (data.GetValue<bool>("isMale"))
            {
                model.Gender = Gender.Male;
            }
            else
            {
                model.Gender = Gender.Female;
            }

            return View(model);
        }

        [AuthActionFilter]
        [HttpPost]
        public IActionResult Edit(EditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.IsMale = model.Gender == 0;

            HttpResponseMessage response = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKeyHeaderValue);
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8,
                    "application/json");
                response = client.PutAsync(client.BaseAddress
                    + "Users/PutUser",content).Result;

                return RedirectToAction("Profile", new { id = model.Id });
            }
        }

        [AuthActionFilter]
        [HttpGet]
        public IActionResult ChangePassword(int id)
        {
            if (int.Parse(this.HttpContext.Session.GetString("loggedUser")) != id)
            {
                return RedirectToAction("Index", "Home");
            }
            ChangePasswordVM model = new ChangePasswordVM();
            model.Id = id;

            return View(model);
        }

        [AuthActionFilter]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            HttpResponseMessage response = new HttpResponseMessage();
            JObject data = new JObject();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKeyHeaderValue);
                response = client.GetAsync(client.BaseAddress
                    + "Users/ChangePassword/" + model.Id+", "
                    + model.OldPassword+", "+model.NewPassword ).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Profile", "Users", new
                    {
                        id = model.Id
                    });
                }
                else
                {
                    this.ModelState.AddModelError("wrongPassword", "Incorrect old password");
                    return View(model);
                }
            }
        }

        [AuthActionFilter]
        public IActionResult Delete(int id)
        {
            if (int.Parse(this.HttpContext.Session.GetString("loggedUser")) != id)
            {
                return RedirectToAction("Index", "Home");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKeyHeaderValue);
                HttpResponseMessage response = client.DeleteAsync(client.BaseAddress
                    + "Users/DeleteUser/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    this.HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Profile", "Users", new
                    {
                        id = id
                    });
                }
            }

        }

        [AuthActionFilter]
        [HttpGet]
        public IActionResult Profile(int id)
        {
            if (int.Parse(this.HttpContext.Session.GetString("loggedUser")) !=id)
            {
                return RedirectToAction("Index", "Home");
            }
            ProfileVM model = new ProfileVM();
            model.Id = id;
            HttpResponseMessage response = new HttpResponseMessage();
            JObject data = new JObject();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKeyHeaderValue);
                response = client.GetAsync(client.BaseAddress
                 + "Users/GetUser/" + model.Id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    data = JObject.Parse(result);
                    response = client.GetAsync(client.BaseAddress
                    + "Users/GetBalance/" + model.Id).Result;
                    string balance = response.Content.ReadAsStringAsync().Result;
                    data.Add("balance", balance);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return RedirectToAction("Login", "Home");
                    }
                }
            }
            model.Username = data["username"].ToString();
            model.FirstName = data["firstName"].ToString();
            model.LastName = data["lastName"].ToString();
            model.BirthDate = DateOnly.Parse(data["birthDate"].ToString());
            model.IsMale = bool.Parse(data["isMale"].ToString());
            model.Balance = data["balance"].ToString();
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }
            HttpResponseMessage response=new HttpResponseMessage();
            int id = 0;
            using (var client=new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName, AuthConstants.ApiKeyHeaderValue);
                response = client.GetAsync(client.BaseAddress
                    +"Users/Login/"+model.Username
                    +", "+model.Password).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    id = JsonConvert.DeserializeObject<int>(data);
                    this.HttpContext.Session.SetString("loggedUser",id.ToString());
                }
                else
                {
                    if (response.StatusCode==HttpStatusCode.NotFound)
                    {
                        this.ModelState.AddModelError("authError", "Incorrect username or password");
                        return View();
                    }
                }
            }
            return RedirectToAction("Profile", new
            {
                id = id
            });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            this.HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
