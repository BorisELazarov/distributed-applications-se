using DAL.Entities;
using DTL.DTO;
using DTL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using NuGet.Protocol;
using System.Net;
using System.Text;
using System.Text.Json;
using UI.ActionFilters;
using UI.ViewModels.Cards;
using UI.ViewModels.Shared;
using WebAPI;
using WebAPI.Authentication;
using WebAPI.Controllers;

namespace UI.Controllers
{
    [AuthActionFilter]
    public class CardsController : Controller
    {

        // GET: CardsController
        [HttpGet]
        public IActionResult Index(IndexVM model)
        {
            int id = int.Parse(this.HttpContext.Session.GetString("loggedUser"));
            model.Pager ??= new PagerVM();
            if (model.Pager.Page<=0)
            {
                model.Pager.Page = 1;
            }
            if (model.Pager.ItemsPerPage <= 0)
            {
                model.Pager.ItemsPerPage = 10;
            }
            model.Filter ??= new FilterVM();
            if (id > 0)
                model.Filter.UserId = id;
            model.OrderBy ??= new SortVM();
            model.OrderBy.Key = (int)model.OrderBy.Value;
            HttpResponseMessage response = new HttpResponseMessage();
            List<DetailsVM> fullList = new List<DetailsVM>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName,
                    AuthConstants.ApiKeyHeaderValue);
                response = client.GetAsync(client.BaseAddress
                       + "Cards/GetCards/" + model.Filter.UserId + ", "
                       + model.Filter.Balance + ", " + model.OrderBy.Key + ", "
                       + model.Pager.Page + ", " + model.Pager.ItemsPerPage).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    model.Items = JsonConvert.DeserializeObject<List<DetailsVM>>(result);
                    response = client.GetAsync(client.BaseAddress
                       + "Cards/GetCards/" + model.Filter.UserId + ", "
                       + 0 + ", " + model.OrderBy.Key + ", "
                       + 1 + ", " + int.MaxValue).Result;
                    result= response.Content.ReadAsStringAsync().Result;
                    fullList= JsonConvert.DeserializeObject<List<DetailsVM>>(result);

                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            fullList=fullList.Where(x => string.IsNullOrEmpty(model.Filter.Title)
            || x.Title.Contains(model.Filter.Title)).ToList();
            if (model.Items!=null)
                model.Items = model.Items.Where(x => string.IsNullOrEmpty(model.Filter.Title)
            || x.Title.Contains(model.Filter.Title)).ToList();
            model.Pager.PagesCount = (int)Math.Ceiling(fullList.Count
            / (double)model.Pager.ItemsPerPage);
            model.Items ??= new List<DetailsVM>();

            return View(model);
        }

        // GET: CardsController/Details/5
        public IActionResult Details(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            DetailsVM model=new DetailsVM();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName,
                    AuthConstants.ApiKeyHeaderValue);
                response = client.GetAsync(client.BaseAddress
                   + "Cards/GetCard/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<DetailsVM>(result);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            if (int.Parse(this.HttpContext.Session.GetString("loggedUser")) != model.UserId)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        // GET: CardsController/Create
        [HttpGet]
        public ActionResult Create(int id)
        {
            if (int.Parse(this.HttpContext.Session.GetString("loggedUser")) != id)
            {
                return RedirectToAction("Index", "Home");
            }
            CreateVM model = new CreateVM();
            model.UserId= id;
            return View(model);
        }

        // POST: CardsController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(CreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            InputVM input = new InputVM();
            input.UserId = model.UserId;
            input.Title = model.Title;
            input.ValidThru = model.ValidThru;
            input.Balance = model.Balance;
            input.CardNumber = "****************";
            input.IBAN = "**********************";
            input.SecurityCode = "***";
            HttpResponseMessage response = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName,
                    AuthConstants.ApiKeyHeaderValue);
                string data = JsonConvert.SerializeObject(input);
                StringContent content = new StringContent(data, Encoding.UTF8,
                    "application/json");
                response = client.PostAsync(client.BaseAddress
                    + "Cards/PostCard", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    data = response.Content.ReadAsStringAsync().Result;
                    DetailsVM details = JsonConvert.DeserializeObject<DetailsVM>(data);
                    return RedirectToAction("Details", new { id=details.Id});
                }
                else
                {
                    return RedirectToAction("Index", new { id = model.UserId });
                }
            }
        }

        [HttpGet]
        public IActionResult ChangeInfo(int id)
        {
            ChangeInfoVM model = new ChangeInfoVM();
            JObject data = new JObject();
            HttpResponseMessage response = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName,
                    AuthConstants.ApiKeyHeaderValue);
                response = client.GetAsync(client.BaseAddress
                   + "Cards/GetCard/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<ChangeInfoVM>(result);
                    if (int.Parse(this.HttpContext.Session.GetString("loggedUser")) != model.UserId)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return View(model);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return RedirectToAction("Details", new { id=id});
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: CardsController/ChangeInfo/5
        [HttpPost]
        public IActionResult ChangeInfo(ChangeInfoVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            HttpResponseMessage response = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName,
                    AuthConstants.ApiKeyHeaderValue);
                string data = JsonConvert.SerializeObject(model);
                JObject job = JObject.Parse(data);
                StringContent content = new StringContent(data, Encoding.UTF8,
                    "application/json");
                response = client.PutAsync(client.BaseAddress
                    + "Cards/ChangeInfo", content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { id = model.UserId });
                }
            }
            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpGet]
        public IActionResult Renew(int id)
        {
            if (IsValid(id))
            {
                return RedirectToAction("Index", "Home");
            }
            RenewVM model = new RenewVM();
            HttpResponseMessage response = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName,
                    AuthConstants.ApiKeyHeaderValue);
                response = client.GetAsync(client.BaseAddress
                   + "Cards/GetCard/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<RenewVM>(result);
                    return View(model);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return RedirectToAction("Index", new { id = model.UserId });
                    }
                }
            }
            return RedirectToAction("Details", new {id=id});
        }

        [HttpPost]
        public IActionResult Renew(RenewVM model)
        {
            if (model.ValidThru <= DateOnly.Parse(DateTime.Now.ToShortDateString()))
            {
                this.ModelState.AddModelError("dateError", "Please, select future date!");
                return View(model);
            }

            HttpResponseMessage response = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName,
                    AuthConstants.ApiKeyHeaderValue);
                response = client.GetAsync(client.BaseAddress
                   + "Cards/Renew/" + model.Id+", "+model.ValidThru.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<RenewVM>(result);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return RedirectToAction("Index", new { id = model.UserId });
                    }
                }
            }

            return RedirectToAction("Details", new { id = model.Id });
        }

        public IActionResult Delete(int id, int userId)
        {
            if (int.Parse(this.HttpContext.Session.GetString("loggedUser")) != userId)
            {
                return RedirectToAction("Index", "Home");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName,
                    AuthConstants.ApiKeyHeaderValue);
                HttpResponseMessage response = client.DeleteAsync(client.BaseAddress
                    + "Cards/DeleteCard/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new
                    {
                        id = userId
                    });
                }
                else
                {
                    return RedirectToAction("Details", new
                    {
                        id = id
                    });
                }
            }
        }

        private bool IsValid(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            DetailsVM model = new DetailsVM();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName,
                    AuthConstants.ApiKeyHeaderValue);
                response = client.GetAsync(client.BaseAddress
                   + "Cards/GetCard/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<DetailsVM>(result);
                }
                else
                {
                    return false;
                }
            } 
            return int.Parse(this.HttpContext.Session.GetString("loggedUser")) != model.UserId;
        }
    }
}
