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
    public class BartenderDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns a list of bartenders in the system
        /// </summary>
        /// <returns>An array of bartenders</returns>
        /// <example>
        /// GET: /api/BartenderData/ListBartenders ->
        /// [{"bartenderId" : 3, "firstName" : "Alex", "lastName" : "Turner" , "email" : "alexturner@example.com" , "numDrinks" : 4 "lastDrinkPosted" : 2020-07-21},
        /// {"bartenderId" : 4, "firstName" : "Noah", "lastName" : "Kahan" , "email" : "noahkahan@example.com" , "numDrinks" : 3 "lastDrinkPosted" : 2024-03-18}]
        /// </example>
        [HttpGet]
        [Route("api/BartenderData/ListBartenders")]
        public List<BartenderDto> ListBartenders()
        {
            List<Bartender> Bartenders = db.Bartenders.ToList();

            List<BartenderDto> BartenderDtos = new List<BartenderDto>();

            foreach (Bartender Bartender in Bartenders)
            {
                BartenderDto BartenderDto = new BartenderDto();
                BartenderDto.bartenderId = Bartender.bartenderId;
                BartenderDto.firstName = Bartender.firstName;
                BartenderDto.lastName = Bartender.lastName;
                BartenderDto.email = Bartender.email;
                BartenderDto.numDrinks = Bartender.numDrinks;
                BartenderDto.lastDrink = Bartender.lastDrink;

                BartenderDtos.Add(BartenderDto);
            }
            return BartenderDtos;
        }

        [ResponseType(typeof(Bartender))]
        [HttpGet]
        [Route("api/BartenderData/FindBartender/{id}")]
        public IHttpActionResult FindBartender(int id)
        {
            Bartender Bartender = db.Bartenders.Find(id);
            BartenderDto BartenderDto = new BartenderDto()
            {
                bartenderId = Bartender.bartenderId,
                firstName = Bartender.firstName,
                lastName = Bartender.lastName,
                email = Bartender.email,
                numDrinks = Bartender.numDrinks,
                lastDrink = Bartender.lastDrink,
            };

            if (Bartender == null)
            {
                return NotFound();
            }

            return Ok(BartenderDto);

        }

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateBartender(int id, Bartender bartender)
        {
            Debug.WriteLine("I have succesfully reached update bartender method!");

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != bartender.bartenderId)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("Get parameter" + id);
                Debug.WriteLine("POST parameter" + bartender.bartenderId);
                Debug.WriteLine("POST parameter" + bartender.firstName);
                return BadRequest();
            }

            db.Entry(bartender).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!BartenderExists(id))
                {
                    Debug.WriteLine("Bartender Not Found");
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
    }
}
