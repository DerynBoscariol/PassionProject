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
        private static readonly HttpClient client = new HttpClient();
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        //GET: Cocktail/List
        public ActionResult List()
        {
            HttpClient client = new HttpClient();
            string url = "https://localhost:44307/api/cocktaildata/listcocktails";
            //communicate with cocktail data controller to retrieve list of cocktails
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;


            //deserialize json response content into an IEnumerable<CocktailDTo>
            IEnumerable<CocktailDto> cocktails = responseMessage.Content.ReadAsAsync<IEnumerable<CocktailDto>>().Result;


            //logging the response
            Debug.WriteLine("Response code: ");
            Debug.WriteLine(responseMessage.StatusCode);
            Debug.WriteLine(responseMessage.Content.ReadAsStringAsync().Result);

            Debug.WriteLine("Number of cocktails: ");
            Debug.WriteLine(cocktails.Count());

            return View(cocktails);
        }

        //GET: Cocktail/Details/id

        public ActionResult Details(int id)
        {
            DetailsCocktail ViewModel = new DetailsCocktail();
            //communicate with cocktail data controller to retrieve all information about 1 cocktail

            HttpClient client = new HttpClient();
            string url = "https://localhost:44307/api/cocktaildata/findcocktail/" + id;
            //communicate with cocktail data controller to retrieve list of cocktails
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;


            //deserialize json response content into an IEnumerable<CocktailDTo>
            CocktailDto SelectedCocktail = responseMessage.Content.ReadAsAsync<CocktailDto>().Result;

            Debug.WriteLine("Cocktail received: ");
            Debug.WriteLine(SelectedCocktail.DrinkName);

            ViewModel.SelectedCocktail = SelectedCocktail;

            return View(ViewModel);

        }

        public ActionResult Error()
        {

            return View();
        }

        public ActionResult New()
        {
            //information about all bartenders in the system.
            //GET api/bartenderdata/listbartenders
            string url = "https://localhost:44307/api/bartenderdata/listbartenders";
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;
            IEnumerable<BartenderDto> BartenderOptions = responseMessage.Content.ReadAsAsync<IEnumerable<BartenderDto>>().Result;
            Debug.WriteLine("New method successful");
            return View(BartenderOptions);
        }

        //POST: Cocktail/Create
        [HttpPost]
        public ActionResult Create(Cocktail cocktail)
        {
            Debug.WriteLine("Json payload: ");
            Debug.WriteLine(cocktail.DrinkName);

            string url = "https://localhost:44307/api/cocktaildata/AddCocktail";

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
                Debug.WriteLine("Error: response message: " + responseMessage);
                return RedirectToAction("Error");
            }
        }

        //GET: cocktail/edit/id
        public ActionResult Edit(int id)
        {
            UpdateCocktail ViewModel = new UpdateCocktail();

            string url = "https://localhost:44307/api/cocktaildata/findcocktail/" + id;
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;

            Debug.WriteLine("Response code: ");
            Debug.WriteLine(responseMessage.StatusCode);

            CocktailDto selectedCocktail = responseMessage.Content.ReadAsAsync<CocktailDto>().Result;
            ViewModel.SelectedCocktail = selectedCocktail;

            Debug.WriteLine("Cocktail received: " + selectedCocktail);

            string durl = "https://localhost:44307/api/bartenderdata/listbartenders";
            responseMessage = client.GetAsync(durl).Result;
            IEnumerable<BartenderDto> BartenderOptions = responseMessage.Content.ReadAsAsync<IEnumerable<BartenderDto>>().Result;

            ViewModel.BartenderOptions = BartenderOptions;


            return View(ViewModel);
        }

        //POST: Cocktail/Update/id
        [HttpPost]
        public ActionResult Update(int id, Cocktail cocktail)
        {
            try
            {
                Debug.WriteLine("The new cocktail info is:");
                Debug.WriteLine("Name: " + cocktail.DrinkName);
                Debug.WriteLine("Type: " + cocktail.DrinkType);
                Debug.WriteLine("Recipe: " + cocktail.DrinkRecipe);
                Debug.WriteLine("Alcoholic Ingredients: " + cocktail.LiqIn);
                Debug.WriteLine("Mix Ingredients: " + cocktail.MixIn);

                string url = "https://localhost:44307/api/cocktail/UpdateCocktail/" + id;
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
