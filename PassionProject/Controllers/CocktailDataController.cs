using PassionProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics;

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
            List<Cocktail> Cocktails = db.Cocktails.ToList();
            //create a list of cocktails as data tranferable objects
            List<CocktailDto> CocktailDtos = new List<CocktailDto>();

            //convert each cocktail entity into a cocktaildto and add to the list
            Cocktails.ForEach(c => CocktailDtos.Add(new CocktailDto()
            {
                drinkId = c.drinkId,
                drinkName = c.drinkName,
                drinkType = c.drinkType,
                drinkRecipe = c.drinkRecipe,
                liqIn = c.liqIn,
                mixIn = c.mixIn,
                datePosted = c.datePosted,
                bartenderId = c.bartenderId
            }));
            Debug.WriteLine(CocktailDtos);
            return Ok(CocktailDtos);
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
        public IHttpActionResult ListCocktailsByBartender(int id)
        {
            List<Cocktail> Cocktails = db.Cocktails.Where(c => c.bartenderId == id).ToList();
            List<CocktailDto> CocktailDtos = new List<CocktailDto>();

            Cocktails.ForEach(c => CocktailDtos.Add(new CocktailDto()
            {
                drinkId = c.drinkId,
                drinkName = c.drinkName,
                drinkType = c.drinkType,
                drinkRecipe = c.drinkRecipe,
                liqIn = c.liqIn,
                mixIn = c.mixIn,
                datePosted = c.datePosted,
                bartenderId = c.bartenderId
            }));

            return Ok(CocktailDtos);
        }

        [ResponseType(typeof(Cocktail))]
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
                drinkId = Cocktail.drinkId,
                drinkName = Cocktail.drinkName,
                drinkType = Cocktail.drinkType,
                drinkRecipe = Cocktail.drinkRecipe,
                liqIn = Cocktail.liqIn,
                mixIn = Cocktail.mixIn,
                datePosted = Cocktail.datePosted,
                bartenderId = Cocktail.bartenderId
            };

            return Ok(CocktailDto);

        }

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCocktail(int id, Cocktail cocktail)
        {
            Debug.WriteLine("I have succesfully reached update cocktail method!");

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != cocktail.drinkId)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("Get parameter" + id);
                Debug.WriteLine("POST parameter" + cocktail.drinkId);
                Debug.WriteLine("POST parameter" + cocktail.drinkName);
                return BadRequest();
            }

            db.Entry(cocktail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)

            {
                if(!CocktailExists(id))
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cocktails.Add(cocktail);
            db.SaveChanges();

            return Ok();
        }

        // POST: api/CocktailData/DeleteCocktail

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
            return db.Cocktails.Count(e => e.drinkId == id) > 0;
        }

    }
}
