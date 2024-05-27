using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using System.Net;
using System.Text;
using UI.ActionFilters;
using UI.ViewModels.Shared;
using UI.ViewModels.Transactions;
using WebAPI;
using WebAPI.Authentication;

namespace UI.Controllers
{
    [AuthActionFilter]
    public class TransactionsController : Controller
    {

        // GET: TransactionsController
        [HttpGet]
        public IActionResult Index(IndexVM model, int id = 0)
        {
            if (IsValid(id))
            {
                return RedirectToAction("Index", "Home");
            }
            model.Pager ??= new PagerVM();
            if (model.Pager.Page <= 0)
            {
                model.Pager.Page = 1;
            }
            if (model.Pager.ItemsPerPage <= 0)
            {
                model.Pager.ItemsPerPage = 10;
            }
            model.Filter ??= new FilterVM();
            if (id > 0)
                model.Filter.CardId = id;
            model.OrderBy ??= new SortVM();
            HttpResponseMessage response = new HttpResponseMessage();
            List<DetailsVM> fullList = new List<DetailsVM>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName,
                    AuthConstants.ApiKeyHeaderValue);
                response = client.GetAsync(client.BaseAddress
                       + "Transactions/GetTransactions/" + model.Filter.CardId + ", "
                       + model.Filter.DateOfTransaction.ToString() + ", " + model.Filter.Sum
                       + ", " + (int)model.OrderBy.Value + ", " + model.Pager.Page + ", "
                       + model.Pager.ItemsPerPage).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    model.Items = JsonConvert.DeserializeObject<List<DetailsVM>>(result);
                    response = client.GetAsync(client.BaseAddress
                       + "Transactions/GetTransactions/" + model.Filter.CardId + ", "
                       + DateOnly.MinValue.ToString() + ", " + 0
                       + ", " + (int)model.OrderBy.Value + ", " + 1 + ", "
                       + int.MaxValue).Result;
                    result = response.Content.ReadAsStringAsync().Result;
                    fullList = JsonConvert.DeserializeObject<List<DetailsVM>>(result);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            if(!string.IsNullOrEmpty(model.Filter.IBAN))
            model.Filter.IBAN=model.Filter.IBAN.ToUpper();
            if (model.Items != null)
                model.Items = model.Items.Where(x => (string.IsNullOrEmpty(model.Filter.Title)
            || x.Title.Contains(model.Filter.Title)) &&
            (string.IsNullOrEmpty(model.Filter.IBAN) || x.IBAN.Contains(model.Filter.IBAN))).ToList();
            if(fullList!=null)
                fullList = fullList.Where(x => string.IsNullOrEmpty(model.Filter.Title)
            || x.Title.Contains(model.Filter.Title) &&
            (string.IsNullOrEmpty(model.Filter.IBAN) || x.IBAN.Contains(model.Filter.IBAN))).ToList();
            model.Pager.PagesCount = (int)Math.Ceiling(fullList.Count
            / (double)model.Pager.ItemsPerPage);
            model.Items ??= new List<DetailsVM>();

            return View(model);
        }

        // GET: TransactionsController/Details/5
        public IActionResult Details(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            DetailsVM model = new DetailsVM();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName,
                    AuthConstants.ApiKeyHeaderValue);
                response = client.GetAsync(client.BaseAddress
                   + "Transactions/GetTransaction/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<DetailsVM>(result);

                    if (IsValid(model.CardId))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(model);
        }

        // GET: TransactionsController/Create
        [HttpGet]
        public IActionResult Create(int id)
        {
            if (IsValid(id))
            {
                return RedirectToAction("Index", "Home");
            }
            CreateVM model = new CreateVM();
            model.CardId = id;
            return View(model);
        }

        // POST: TransactionsController/Create
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(CreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.DateOfTransaction = DateOnly.Parse(DateTime.Now.ToShortDateString());
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
                try
                {
                    response = client.PostAsync(client.BaseAddress
                        + "Transactions/PostTransaction", content).Result;
                }
                catch (HttpRequestException)
                {

                    throw;
                }
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { id = model.CardId });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        // GET: TransactionsController/Edit/5
        public IActionResult Edit(int id)
        {
            
            EditVM model = new EditVM();
            HttpResponseMessage response = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName,
                    AuthConstants.ApiKeyHeaderValue);
                response = client.GetAsync(client.BaseAddress
                   + "Transactions/GetTransaction/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<EditVM>(result);
                    if (IsValid(id))
                        return View(model);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return RedirectToAction("Details", new { id = id });
                    }
                }
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: TransactionsController/Edit/5
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(EditVM model)
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
                    + "Transactions/PutTransaction", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", new { id = model.Id });
                }
                else
                {
                    return RedirectToAction("Index", new { id = model.CardId });
                }
            }
        }

        public ActionResult Delete(int id, int cardId)
        {
            if (IsValid(cardId))
            {
                return RedirectToAction("Index", "Home");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalVars.RemoteBaseURL);
                client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName,
                    AuthConstants.ApiKeyHeaderValue);
                HttpResponseMessage response = client.DeleteAsync(client.BaseAddress
                    + "Transactions/DeleteTransaction/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new
                    {
                        id = cardId
                    });
                }
                else
                {

                    return RedirectToAction("Details", new{id = id});
                }
            }
        }
        private bool IsValid(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            ViewModels.Cards.DetailsVM model = new ViewModels.Cards.DetailsVM();
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
                    model = JsonConvert.DeserializeObject<ViewModels.Cards.DetailsVM>(result);
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