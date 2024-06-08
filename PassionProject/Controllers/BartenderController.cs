using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Script.Serialization;
using PassionProject.Models;

namespace PassionProject.Controllers
{
    public class BartenderController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        static BartenderController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44307/api/bartenderdata");
        }

        //GET: Bartender/List
        public ActionResult List()
        {
            //communicate with bartender data controller to retrieve list of bartenders

            string url = "ListBartenders";
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;

            Debug.WriteLine("Response code: ");
            Debug.WriteLine(responseMessage.StatusCode);

            IEnumerable<BartenderDto> bartenders = responseMessage.Content.ReadAsAsync<IEnumerable<BartenderDto>>().Result;
            Debug.WriteLine("Number of bartenders: ");
            Debug.WriteLine(bartenders.Count());

            return View(bartenders);
        }

        //GET: Bartender/Info/id

        public ActionResult Info(int id)
        {
            //communicate with bartender data controller to retrieve all information about 1 bartender

            string url = "FindBartender/" + id;
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;

            Debug.WriteLine("Response code: ");
            Debug.WriteLine(responseMessage.StatusCode);

            BartenderDto selectedBartender = responseMessage.Content.ReadAsAsync<BartenderDto>().Result;
            Debug.WriteLine("Bartender received: ");
            Debug.WriteLine(selectedBartender.firstName + " " + selectedBartender.lastName);

            return View(selectedBartender);

        }

        public ActionResult Error()
        {

            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        //POST: Bartender/Create
        [HttpPost]
        public ActionResult Create(Bartender bartender)
        {
            Debug.WriteLine("Json payload: ");
            Debug.WriteLine(bartender.firstName + " " + bartender.lastName);

            string url = "AddBartender";

            string jsonpayload = serializer.Serialize(bartender);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);

        }
    }
}
