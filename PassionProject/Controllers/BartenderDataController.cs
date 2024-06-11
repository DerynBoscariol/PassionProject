using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProject.Models;
using System.Diagnostics;


namespace PassionProject.Controllers
{
    public class BartenderDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //GET: api/bartenderdata/listbartenders
        [HttpGet]
        [ResponseType(typeof(BartenderDto))]
        public IHttpActionResult ListBartenders()
        {
            List<Bartender> Bartenders = db.Bartenders.ToList();
            List<BartenderDto> BartenderDtos = new List<BartenderDto>();

            Bartenders.ForEach(b => BartenderDtos.Add(new BartenderDto()
            {
                bartenderId = b.bartenderId,
                firstName = b.firstName,
                lastName = b.lastName,
                email = b.email,
                numDrinks = b.numDrinks,
                lastDrink = b.lastDrink
            }));

            return Ok(BartenderDtos);
        }

        //GET: api/BartenderData/FindBartender/id
        [HttpGet]
        [ResponseType(typeof(BartenderDto))]
        public IHttpActionResult FindBartender(int id) 
        {
            Bartender Bartender = db.Bartenders.Find(id);

            if (Bartender == null)
            {
                return NotFound();
            }

            BartenderDto BartenderDto = new BartenderDto()
            {
                bartenderId = Bartender.bartenderId,
                firstName = Bartender.firstName,
                lastName = Bartender.lastName,
                email = Bartender.email,
                numDrinks = Bartender.numDrinks,
                lastDrink = Bartender.lastDrink
            };

            return Ok(BartenderDto);
        }

        //POST: api/bartenderdata/updateBartender/id
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateBartender(int id, Bartender b)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != b.bartenderId)
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

        //POST: api/bartenderdata/addbartender
        //FORM DATA: Bartender JSON object
        [HttpPost]
        [ResponseType(typeof(Bartender))]
        public IHttpActionResult AddBartender(Bartender Bartender)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bartenders.Add(Bartender);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = Bartender.bartenderId }, Bartender);
        }

        //POST: api/BartenderData/DeleteBartender/id
        //FORM DATA (empty)
        [HttpPost]
        [ResponseType(typeof(Bartender))]
        public IHttpActionResult DeleteBartender(int id)
        {
            Bartender Bartender = db.Bartenders.Find(id);
            if(Bartender == null)
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

        private bool BartenderExists(int id)
        {
            return db.Bartenders.Count(b => b.bartenderId == id) > 0;
        }
    }
}