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
using PassionProject.Models.ViewModels;
using Newtonsoft.Json;

namespace PassionProject.Controllers
{
    public class CocktailController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        static CocktailController()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44307/api/")
            };
        }

        //GET: Cocktail/List
        public ActionResult List()
        {
            //communicate with cocktail data controller to retrieve list of cocktails

            //Send GET request to api
            string url = "CocktailData/ListCocktails";
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;

            //logging the response
            Debug.WriteLine("Response code: ");
            Debug.WriteLine(responseMessage.StatusCode);
            Debug.WriteLine(responseMessage.Content.ReadAsStringAsync().Result);

            //deserialize json response content into an IEnumerable<CocktailDTo>
            string jsonResponse = responseMessage.Content.ReadAsStringAsync().Result;
            IEnumerable<CocktailDto> cocktails = JsonConvert.DeserializeObject<IEnumerable<CocktailDto>>(jsonResponse);
            Debug.WriteLine("Number of cocktails: ");
            Debug.WriteLine(cocktails.Count());

            return View(cocktails);
        }

        //GET: Cocktail/Details/id

        public ActionResult Details(int id)
        {
            DetailsCocktail ViewModel = new DetailsCocktail();
            //communicate with cocktail data controller to retrieve all information about 1 cocktail

            string url = "CocktailData/FindCocktail/" + id;
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;

            Debug.WriteLine("Response code: ");
            Debug.WriteLine(responseMessage.StatusCode);

            CocktailDto SelectedCocktail = responseMessage.Content.ReadAsAsync<CocktailDto>().Result;
            Debug.WriteLine("Cocktail received: ");
            Debug.WriteLine(SelectedCocktail.drinkName);

            ViewModel.SelectedCocktail = SelectedCocktail;

            //Show bartender who made the drink

            url = "BartenderData/FindBartender/" + id;
            responseMessage = client.GetAsync(url).Result;
            IEnumerable<BartenderDto> BartenderCreated = responseMessage.Content.ReadAsAsync<IEnumerable<BartenderDto>>().Result;

            ViewModel.BartenderCreated = BartenderCreated;

            return View(ViewModel);

        }

        //POST: drink/Associate
        [HttpPost]
        public ActionResult Associate(int id, int bartenderId)
        {
            string url = "cocktaildata/Associate/"+ id + "/" +bartenderId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage responseMessage = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error()
        {

            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        //POST: Cocktail/Create
        [HttpPost]
        public ActionResult Create(Cocktail cocktail)
        {
            Debug.WriteLine("Json payload: ");
            Debug.WriteLine(cocktail.drinkId + " " + cocktail.drinkName);

            string url = "AddCocktail";

            string jsonpayload = serializer.Serialize(cocktail);

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

        //GET: cocktail/edit/id
        public ActionResult Edit(int id)
        {
            string url = "findCocktail/" + id;
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;

            Debug.WriteLine("Response code: ");
            Debug.WriteLine(responseMessage.StatusCode);

            CocktailDto selectedCocktail = responseMessage.Content.ReadAsAsync<CocktailDto>().Result;

            Debug.WriteLine("Cocktail received: ");
            Debug.WriteLine(selectedCocktail.drinkId + " " + selectedCocktail.drinkName);

            return View(selectedCocktail);
        }

        //POST: Cocktail/Update/id
        [HttpPost]
        public ActionResult Update(int id, Cocktail cocktail)
        {
            try
            {
                Debug.WriteLine("The new cocktail info is:");
                Debug.WriteLine("Name: " + cocktail.drinkName);
                Debug.WriteLine("Type: " + cocktail.drinkType);
                Debug.WriteLine("Recipe: " + cocktail.drinkRecipe);
                Debug.WriteLine("Alcoholic Ingredients: " + cocktail.liqIn);
                Debug.WriteLine("Mix Ingredients: " + cocktail.mixIn);

                string url = "UpdateCocktail/" + id;
                string jsonpayload = serializer.Serialize(cocktail);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/CocktailData/UpdateCocktail/{id}
                //Header : Content-Type: application/json
                //
                HttpResponseMessage responseMessage = client.PostAsync(url, content).Result;

                return RedirectToAction("Details/" + id);

            }
            catch
            {
                return View();
            }
        }
        //GET: Cocktail/Delete/id
        public ActionResult Delete(int id, FormCollection collection) 
        {
           string url = "cocktailData/DeleteCocktail/" + id;
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
