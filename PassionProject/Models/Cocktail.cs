using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace PassionProject.Models
{
    public class Cocktail
    {
        [Key]
        public int DrinkId { get; set; }
        public string DrinkName { get; set; }
        public string DrinkType { get; set; }
        public string DrinkRecipe { get; set; }
        public string LiqIn { get; set; }
        public string MixIn { get; set; }

        //A cocktail has one bartender, while one bartender can have many cocktails
        [ForeignKey("Bartender")]
        public int BartenderId { get; set; }
        public virtual Bartender Bartender { get; set; }
    }

    //Data transfer object (DTO) - Communicating the bartenders info externally
    public class CocktailDto
    {
        public int DrinkId { get; set; }
        public string DrinkName { get; set; }
        public string DrinkType { get; set; }
        public string DrinkRecipe { get; set; }
        public string LiqIn { get; set; }
        public string MixIn { get; set; }
        public int BartenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}


