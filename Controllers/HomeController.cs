using MyWebAPIDeal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MyWebAPIDeal.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            IEnumerable<ClientDealsModel> clientDeals;
            using (var httpClientHandler = new HttpClientHandler { Proxy = WebRequest.GetSystemWebProxy() })
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11
           | System.Net.SecurityProtocolType.Tls12;
                var client = new HttpClient(httpClientHandler);
                client.BaseAddress = new Uri("https://localhost:44384/api/");
                //HTTP GET
                var responseTask = client.GetAsync("myDeal/getRecordedClientDeals");
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

        public ActionResult ClientDealView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ClientDealView(ClientDealsModel clientDealsDetails)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44384/api/myDeal/");

                var postTask = client.PostAsJsonAsync<ClientDealsModel>("recordClientDeals", clientDealsDetails);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(clientDealsDetails);
        }
    }
}