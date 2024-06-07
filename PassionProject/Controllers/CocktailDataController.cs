using PassionProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PassionProject.Controllers
{
    public class CocktailDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("api/CocktailData/ListCocktails")]

        public List<CocktailDto> ListCocktails()
        {
            List<Cocktail> Cocktails = db.Cocktails.ToList();

            List<CocktailDto> cocktailDtos = new List<CocktailDto>();

            foreach (Cocktail Cocktail in Cocktails)
            {
                CocktailDto CocktailDto = new CocktailDto();
                CocktailDto.drinkId = Cocktail.drinkId;
                CocktailDto.drinkName = Cocktail.drinkName;
                CocktailDto.drinkType = Cocktail.drinkType;
                CocktailDto.drinkRecipe = Cocktail.drinkRecipe;
                CocktailDto.liqIn = Cocktail.liqIn;
                CocktailDto.mixIn = Cocktail.mixIn;
                CocktailDto.datePosted = Cocktail.datePosted;
                CocktailDto.firstName = Cocktail.Bartender.firstName;
                CocktailDto.lastName = Cocktail.Bartender.lastName;

                cocktailDtos.Add(CocktailDto);
            }
            return cocktailDtos;
        }
    }
}
