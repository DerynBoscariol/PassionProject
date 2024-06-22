﻿using Microsoft.AspNet.Identity;
using PassionProject.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;


namespace PassionProject.Controllers
{
    public class BartenderDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// recieves a GET request and returns an http response and a list object of all the bartenders in the system
        /// </summary>
        /// <example>
        /// curl https://localhost:44307/api/bartenderdata/listbartenders -->
        /// [{"BartenderId":3,"FirstName":"Alex","LastName":"Turner","Email":"alexturner@example.com","NumDrinks":4},{"BartenderId":4,"FirstName":"Noah","LastName":"Kahan","Email":"noahkahan@example.com","NumDrinks":3},{ "BartenderId":5,"FirstName":"Max","LastName":"Kerman","Email":"maxKerman@example.com","NumDrinks":5},{ "BartenderId":6,"FirstName":"Hannah","LastName":"Wicklund","Email":"hannahw@gmail.com","NumDrinks":2}]
        /// </example>
        // GET: api/bartenderdata/listbartenders
        [HttpGet]
        [ResponseType(typeof(IEnumerable<BartenderDto>))]
        public IHttpActionResult ListBartenders()
        {
            List<Bartender> Bartenders = db.Bartenders.ToList();
            List<BartenderDto> BartenderDtos = new List<BartenderDto>();

            Bartenders.ForEach(b => BartenderDtos.Add(new BartenderDto()
            {
                BartenderId = b.BartenderId,
                FirstName = b.FirstName,
                LastName = b.LastName,
                Email = b.Email,
                NumDrinks = b.NumDrinks
            }));

            return Ok(BartenderDtos);
        }
        /// <summary>
        /// Receives  BartenderId and returns a http response with all information about said bartender
        /// </summary>
        /// <param name="BartenderId">Unique integer to differentiate bartenders</param>
        /// <returns>
        /// Ok Http response and BartenderDto
        /// </returns>
        /// <example>
        /// curl https://localhost:44307/api/bartenderdata/findbartender/3  --->
        /// {"BartenderId":3,"FirstName":"Alex","LastName":"Turner","Email":"alexturner@example.com","NumDrinks":4}
        /// </example>
        // GET: api/BartenderData/FindBartender/id
        [HttpGet]
        [Route("api/bartenderdata/findbartender/{id}")]
        [ResponseType(typeof(BartenderDto))]
        public IHttpActionResult FindBartender(int id)
        {
            Bartender Bartender = db.Bartenders.Find(id);
            Debug.WriteLine(Bartender);

            if (Bartender == null)
            {
                return NotFound();
            }

            BartenderDto BartenderDto = new BartenderDto()
            {
                BartenderId = Bartender.BartenderId,
                FirstName = Bartender.FirstName,
                LastName = Bartender.LastName,
                Email = Bartender.Email,
                NumDrinks = Bartender.NumDrinks
            };

            return Ok(BartenderDto);
        }
        /// <summary>
        /// Recieves a BartenderId and sends updated data to a database then returns and http response 
        /// </summary>
        /// <param name="id"> Id of Bartender user is searching for</param>
        /// <param name="b"> alias for the bartenderobject in the body of the Http response</param>
        /// <returns>HttpStatus code</returns>
        // POST: api/bartenderdata/updateBartender/id
        [HttpPost]
        [Route("api/bartenderdata/updatebartender/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateBartender(int id, [FromBody] Bartender b)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != b.BartenderId)
            {
                return BadRequest();
            }

            db.Entry(b).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BartenderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
        /// <summary>
        /// Recieves a bartender object and sends it to the database
        /// </summary>
        /// <param name="Bartender">The bartender being added to the database</param>
        /// <returns>
        /// A new bartender id for the new bartender object
        /// </returns>
        // POST: api/bartenderdata/addbartender
        [HttpPost]
        [Route("api/bartenderdata/addbartender")]
        [ResponseType(typeof(Bartender))]
        public IHttpActionResult AddBartender([FromBody] Bartender Bartender)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bartenders.Add(Bartender);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = Bartender.BartenderId }, Bartender);
        }
        /// <summary>
        /// Recieves a bartenderId and sends a post request to delete that bartender from the database
        /// </summary>
        /// <param name="id">Id of bartender being deleted</param>
        /// <returns>an Ok Http response</returns>
        // POST: api/BartenderData/DeleteBartender/id
        [HttpPost]
        [Route("api/bartenderdata/deletebartender/{id}")]
        [ResponseType(typeof(Bartender))]
        public IHttpActionResult DeleteBartender(int id)
        {
            Bartender Bartender = db.Bartenders.Find(id);
            if (Bartender == null)
            {
                return NotFound();
            }
            db.Bartenders.Remove(Bartender);
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
        //Checking to see if a given bartender exists in the database
        private bool BartenderExists(int id)
        {
            return db.Bartenders.Count(b => b.BartenderId == id) > 0;
        }
    }
}