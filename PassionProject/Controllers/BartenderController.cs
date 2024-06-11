using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using PassionProject.Models;
using PassionProject.Models.ViewModels;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace PassionProject.Controllers
{
    public class BartenderController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        static BartenderController()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44307/api/")
            };
        }

        // GET: bartender/List
        public ActionResult List()
        {
            string url = "BartenderData/ListBartenders";
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;
            Debug.WriteLine(responseMessage);

            if (!responseMessage.IsSuccessStatusCode)
            {
                // Log error details
                string error = responseMessage.Content.ReadAsStringAsync().Result;
                Debug.WriteLine(error);
                return RedirectToAction("Error", new { message = error });
            }

            // Deserialize the response as a list of BartenderDto
            IEnumerable<BartenderDto> bartenders = responseMessage.Content.ReadAsAsync<IEnumerable<BartenderDto>>().Result;

            return View(bartenders);
        }

        //Get: Bartender/Details/id
        public ActionResult Details(int id)
        {
            DetailsBartender ViewModel = new DetailsBartender();

            string url = "bartenderData/FindBartender/" + id;
            HttpResponseMessage ResponseMessage = client.GetAsync(url).Result;

            BartenderDto SelectedBartender = ResponseMessage.Content.ReadAsAsync<BartenderDto>().Result;

            ViewModel.SelectedBartender = SelectedBartender;

            url = "cocktaildata/ListCocktailsByBartender/" + id;
            ResponseMessage = client.GetAsync(url).Result;

            IEnumerable<CocktailDto> CocktailsMade = ResponseMessage.Content.ReadAsAsync<IEnumerable<CocktailDto>>().Result;

            ViewModel.CocktailsMade = CocktailsMade;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult New()
        {

            return View(); 
        }

        //POST: bartenders/create
        [HttpPost]
        public ActionResult Create(Bartender Bartender)
        {
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(Bartender.firstName + Bartender.lastName);

            string url = "bartenderdata/addBartender";

            string jsonpayload = serializer.Serialize(Bartender);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage responseMessage = client.PostAsync(url, content).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: bartender/edit/id
        public ActionResult Edit(int id)
        {
            string url = "bartenderdata/findbartender/" + id;
            HttpResponseMessage ResponseMessage = client.GetAsync(url).Result;
            BartenderDto SelectedBartender = ResponseMessage.Content.ReadAsAsync<BartenderDto>().Result;
            return View(SelectedBartender);
        }

        //POST: bartender/update/id
        public ActionResult Update(int id, Bartender Bartender) 
        {
            string url = "bartenderdata/updateBartender/" + id;
            string jsonpayload = serializer.Serialize(Bartender);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage responseMessage = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: bartender/delete/id
        public ActionResult DeleteConfirm(int id)
        {
            string url = "bartenderData/findbartender/" + id;
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;
            BartenderDto SelectedBartender = responseMessage.Content.ReadAsAsync<BartenderDto>().Result;
            return View(SelectedBartender);
        }
        // POST: Bartender/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "bartenderdata/deleteBartender/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage responseMessage = client.PostAsync(url, content).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}