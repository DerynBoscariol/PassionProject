using PassionProject.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace PassionProject.Controllers
{
    public class CocktailDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns a list of cocktails in the system
        /// </summary>
        /// <returns>Header 200 (OK)
        /// Content: all cocktails in the database</returns>
        /// <example>
        /// GET: /api/CocktailData/ListCocktails ->
        /// [{"drinkId" : 3, "firstName" : "Alex", "lastName" : "Turner" , "email" : "alexturner@example.com" , "numDrinks" : 4 "lastDrinkPosted" : 2020-07-21},
        /// {"bartenderId" : 4, "firstName" : "Noah", "lastName" : "Kahan" , "email" : "noahkahan@example.com" , "numDrinks" : 3 "lastDrinkPosted" : 2024-03-18}]
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CocktailDto))]
        public IHttpActionResult ListCocktails()
        {
            //fetch cocktails from database and store them in a list
            List<Cocktail> cocktails = db.Cocktails.ToList();
            //create a list of cocktails as data tranferable objects
            List<CocktailDto> cocktailDtos = new List<CocktailDto>();

            //convert each cocktail entity into a cocktaildto and add to the list
            cocktails.ForEach(c => cocktailDtos.Add(new CocktailDto()
            {
                DrinkId = c.DrinkId,
                DrinkName = c.DrinkName,
                DrinkType = c.DrinkType,
                DrinkRecipe = c.DrinkRecipe,
                LiqIn = c.LiqIn,
                MixIn = c.MixIn,
                BartenderId = c.BartenderId,
                FirstName =c.Bartender.FirstName,
                LastName =c.Bartender.LastName
            }));
            Debug.WriteLine(cocktailDtos);
            return Ok(cocktailDtos);
        }

        /// <summary>
        /// Gathers information about all Cocktails related to a specific bartenderId
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all cocktails in database, including their bartenders matched with a specific bartenderId
        /// </returns>
        /// <param name="id">bartenderId</param>
        /// <example>
        /// GET: api/CocktailData/ListCocktailsByBartender/2
        /// </example>

        [HttpGet]
        [ResponseType(typeof(CocktailDto))]
        [Route("api/cocktaildata/listcocktailsbybartender/{id}")]
        public IHttpActionResult ListCocktailsByBartender(int id)
        {
            List<Cocktail> Cocktails = db.Cocktails.Where(c => c.BartenderId == id).ToList();
            List<CocktailDto> CocktailDtos = new List<CocktailDto>();

            Cocktails.ForEach(c => CocktailDtos.Add(new CocktailDto()
            {
                DrinkId = c.DrinkId,
                DrinkName = c.DrinkName,
                DrinkType = c.DrinkType,
                DrinkRecipe = c.DrinkRecipe,
                LiqIn = c.LiqIn,
                MixIn = c.MixIn,
                BartenderId = c.BartenderId,
                FirstName = c.Bartender.FirstName,
                LastName = c.Bartender.LastName
            }));

            return Ok(CocktailDtos);
        }

        [ResponseType(typeof(CocktailDto))]
        [HttpGet]
        [Route("api/CocktailData/FindCocktail/{id}")]
        public IHttpActionResult FindCocktail(int id)
        {
            Cocktail Cocktail = db.Cocktails.Find(id);

            if (Cocktail == null)
            {
                return NotFound();
            }
            CocktailDto CocktailDto = new CocktailDto()
            {
                DrinkId = Cocktail.DrinkId,
                DrinkName = Cocktail.DrinkName,
                DrinkType = Cocktail.DrinkType,
                DrinkRecipe = Cocktail.DrinkRecipe,
                LiqIn = Cocktail.LiqIn,
                MixIn = Cocktail.MixIn,
                BartenderId = Cocktail.BartenderId,
                FirstName = Cocktail.Bartender.FirstName,
                LastName = Cocktail.Bartender.LastName
            };

            return Ok(CocktailDto);

        }

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCocktail(int id, CocktailDto cocktail)
        {
            Debug.WriteLine("I have succesfully reached update cocktail method!");

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != cocktail.DrinkId)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("Get parameter" + id);
                Debug.WriteLine("POST parameter" + cocktail.DrinkId);
                Debug.WriteLine("POST parameter" + cocktail.DrinkName);
                return BadRequest();
            }

            db.Entry(cocktail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)

            {
                if (!CocktailExists(id))
                {
                    Debug.WriteLine("Cocktail Not Found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("No conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CocktailData/AddCocktail
        [ResponseType(typeof(Cocktail))]
        [HttpPost]
        public IHttpActionResult AddCocktail(Cocktail cocktail)
        {
            Debug.WriteLine(cocktail);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(ModelState);
            }

            db.Cocktails.Add(cocktail);
            Debug.WriteLine("Added cocktail:" + cocktail);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cocktail.DrinkId }, cocktail);
        }

        // POST: api/CocktailData/DeleteCocktail/id

        [ResponseType(typeof(Cocktail))]
        [HttpPost]
        public IHttpActionResult DeleteCocktail(int id)
        {
            Cocktail cocktail = db.Cocktails.Find(id);

            if (cocktail == null)
            {
                return NotFound();
            }

            db.Cocktails.Remove(cocktail);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CocktailExists(int id)
        {
            return db.Cocktails.Count(e => e.DrinkId == id) > 0;
        }

    }
}
