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

        private bool BartenderExists(int id)
        {
            return db.Bartenders.Count(b => b.BartenderId == id) > 0;
        }
    }
}