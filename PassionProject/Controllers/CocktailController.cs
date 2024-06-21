using Newtonsoft.Json;
using PassionProject.Models;
using PassionProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PassionProject.Controllers
{
    public class CocktailController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        static CocktailController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,

                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44307/api/");
        }


        // GET: Cocktail/List
        public ActionResult List()
        {
            List<CocktailDto> cocktailDtos = new List<CocktailDto>();

            try
            {
                string url = "cocktaildata/cocktaillist";
                HttpResponseMessage responseMessage = client.GetAsync(url).Result;

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    cocktailDtos = JsonConvert.DeserializeObject<List<CocktailDto>>(responseData);
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to retrieve cocktails from the API.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
            }

            return View(cocktailDtos);
        }

        //GET: Cocktail/Details/id

        public ActionResult Details(int id)
        {
            DetailsCocktail viewModel = new DetailsCocktail();

            try
            {
                string url = "cocktaildata/findcocktail/" + id;
                HttpResponseMessage responseMessage = client.GetAsync(url).Result;

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    CocktailDto selectedCocktail = JsonConvert.DeserializeObject<CocktailDto>(responseData);

                    viewModel.SelectedCocktail = selectedCocktail;
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.ErrorMessage = "Cocktail not found.";
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to retrieve cocktail details.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
            }

            return View(viewModel);
        }


        public ActionResult Error()
        {

            return View();
        }
        //GET: Cocktail/New
        public ActionResult New()
        {
            //information about all bartenders in the system.
            //GET api/bartenderdata/listbartenders
            string url = "bartenderdata/listbartenders";
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

            string url = "cocktaildata/AddCocktail";
            string jsonpayload = JsonConvert.SerializeObject(cocktail);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload, Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = client.PostAsync(url, content).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                // Handle error response
                Debug.WriteLine("Error response: " + responseMessage.StatusCode);
                return RedirectToAction("Error");
            }


        }


        //GET: cocktail/edit/id
        public ActionResult Edit(int id)
        {
            try
            {
                UpdateCocktail viewModel = new UpdateCocktail();

                // Get selected cocktail by ID
                string cocktailUrl = "cocktaildata/findcocktail/" + id;
                HttpResponseMessage cocktailResponse = client.GetAsync(cocktailUrl).Result;
                if (!cocktailResponse.IsSuccessStatusCode)
                {
                    // Handle unsuccessful response
                    Debug.WriteLine("Error fetching cocktail: " + cocktailResponse.StatusCode);
                    return RedirectToAction("Error");
                }

                CocktailDto selectedCocktail = cocktailResponse.Content.ReadAsAsync<CocktailDto>().Result;
                viewModel.SelectedCocktail = selectedCocktail;

                // Get list of bartenders
                string bartendersUrl = "bartenderdata/listbartenders/";
                HttpResponseMessage bartendersResponse = client.GetAsync(bartendersUrl).Result;
                if (!bartendersResponse.IsSuccessStatusCode)
                {
                    // Handle unsuccessful response
                    Debug.WriteLine("Error fetching bartenders: " + bartendersResponse.StatusCode);
                    return RedirectToAction("Error");
                }

                IEnumerable<BartenderDto> bartenderOptions = bartendersResponse.Content.ReadAsAsync<IEnumerable<BartenderDto>>().Result;
                viewModel.BartenderOptions = bartenderOptions;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log exception or handle it accordingly
                Debug.WriteLine("Exception occurred: " + ex.Message);
                return RedirectToAction("Error");
            }
        }

        //POST: Cocktail/Update/id
        [HttpPost]
        [Route("api/cocktaildata/UpdateCocktail/{id}")]
        public ActionResult Update(int id, Cocktail cocktail)
        {
            try
            {
                string url = "cocktaildata/UpdateCocktail/" + id;
                string jsonPayload = JsonConvert.SerializeObject(cocktail);
                HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage responseMessage = client.PostAsync(url, content).Result;

                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    Debug.WriteLine("Error response: " + responseMessage.StatusCode);

                    return RedirectToAction("Error");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occurred: " + ex.Message);

                return RedirectToAction("Error");
            }
        }


        //GET: Cocktail/Delete/id
        public ActionResult DeleteConfirm(int id)
        {
            string url = "cocktaildata/findcocktail/" + id;
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;
            CocktailDto selectedcocktail = responseMessage.Content.ReadAsAsync<CocktailDto>().Result;
            return View(selectedcocktail);
        }

        [HttpPost]
        public ActionResult Delete(int id)
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
