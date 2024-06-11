using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject.Models
{
    public class Cocktail
    {
        [Key]
        public int drinkId { get; set; }
        public string drinkName { get; set; }
        public string drinkType { get; set; }
        public string drinkRecipe { get; set; }
        public string liqIn { get; set; }
        public string mixIn { get; set; }
        public DateTime datePosted { get; set; }

        //A cocktail has one bartender, while one bartender can have many cocktails
        [ForeignKey("Bartender")]
        public int bartenderId { get; set; }
        public virtual Bartender Bartender { get; set; }
    }

    //Data transfer object (DTO) - Communicating the bartenders info externally
    public class CocktailDto
    {
        public int drinkId { get; set; }
        public string drinkName { get; set; }
        public string drinkType { get; set; }
        public string drinkRecipe { get; set; }
        public string liqIn { get; set; }
        public string mixIn { get; set; }
        public DateTime datePosted { get; set; }
        public int bartenderId { get; set; }

    }
}


