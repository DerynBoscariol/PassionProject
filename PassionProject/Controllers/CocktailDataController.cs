using PassionProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public IHttpActionResult CocktailList()
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
                FirstName = c.Bartender.FirstName,
                LastName = c.Bartender.LastName
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
/*
        [HttpGet]
        [ResponseType(typeof(CocktailDto))]
        //[Route("api/cocktaildata/listcocktailsbybartender/{id}")]
        public IHttpActionResult ListCocktailsByBartender(id)
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
        } */

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
        [Route("api/cocktaildata/UpdateCocktail/{id}")]
        public IHttpActionResult UpdateCocktail(int id, [FromBody] Cocktail cocktail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cocktail.DrinkId)
            {
                return BadRequest("ID mismatch");
            }

            // Example of updating the database entity
            var existingCocktail = db.Cocktails.FirstOrDefault(c => c.DrinkId == id);
            if (existingCocktail == null)
            {
                return NotFound();
            }

            existingCocktail.DrinkName = cocktail.DrinkName;
            existingCocktail.DrinkType = cocktail.DrinkType;
            existingCocktail.DrinkRecipe = cocktail.DrinkRecipe;
            existingCocktail.LiqIn = cocktail.LiqIn;
            existingCocktail.MixIn = cocktail.MixIn;
            existingCocktail.BartenderId = cocktail.BartenderId;

            try
            {
                db.SaveChanges();
                return Ok(); // Or return any appropriate success response
            }
            catch (Exception ex)
            {
                // Log the exception
                Debug.WriteLine($"Error updating cocktail: {ex.Message}");
                return InternalServerError(ex);
            }
        }



        // POST: api/CocktailData/AddCocktail
        [ResponseType(typeof(Cocktail))]
        [HttpPost]
        [Route("api/cocktaildata/AddCocktail")]
        public IHttpActionResult AddCocktail([FromBody] Cocktail cocktail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Cocktails.Add(cocktail);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception details
                Debug.WriteLine("Exception occurred: " + ex.Message);
                // You can also log this to a file, database, etc.
                return InternalServerError(ex);
            }

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
            return db.Cocktails.Count(c => c.DrinkId == id) > 0;
        }

    }
}

