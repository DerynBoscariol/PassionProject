using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class DetailsCocktail
    {
        public CocktailDto SelectedCocktail { get; set; }
        public IEnumerable<BartenderDto> BartenderCreated { get; set; }
    }
}