using MyWebAPIDeal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace MyWebAPIDeal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<ClientDealsModel> clientDeals;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44384/api/myDeal/");
                var responseTask = client.GetAsync("getDeal");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ClientDealsModel>>();
                    readTask.Wait();
                    clientDeals = readTask.Result;
                }
                else
                {
                    clientDeals = Enumerable.Empty<ClientDealsModel>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(clientDeals);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ClientDealsModel clientDealsDetails)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44384/api/myDeal/");
                var postTask = client.PostAsJsonAsync<ClientDealsModel>("saveDeal", clientDealsDetails);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError(String.Empty, String.Format("Record already exist {0}", clientDealsDetails.ClientId));
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            
            return View(clientDealsDetails);
        }

        public ActionResult Edit([System.Web.Http.FromUri] int id)
        {
            IEnumerable<ClientDealsModel> clientDeals;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44384/api/myDeal/");
                var responseTask = client.GetAsync("getDeal?id=" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ClientDealsModel>>();
                    readTask.Wait();
                    clientDeals = readTask.Result;
                }
                else
                {
                    clientDeals = Enumerable.Empty<ClientDealsModel>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(clientDeals.First());
        }

        [HttpPost]
        public ActionResult Edit(ClientDealsModel dealModel)
        {
            if (dealModel == null || dealModel.ClientId == 0)
            {
                ModelState.AddModelError(string.Empty, "Please Enter Valid data.");
                return View();
            }
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri("https://localhost:44384/api/myDeal/");
                var postTask = client.PostAsJsonAsync<ClientDealsModel>("updateDeal", dealModel);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError(string.Empty, "Client Id cannot be changed once registered.");
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            
            return View(dealModel);
        }
        
        public ActionResult Delete([System.Web.Http.FromUri]int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44384/api/myDeal/");
                var responseTask = client.DeleteAsync("deleteDeal?id=" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
