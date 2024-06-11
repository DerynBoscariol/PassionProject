using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class UpdateCocktail
    {
        public CocktailDto SelectedCocktail { get; set; }

        public IEnumerable<BartenderDto> BartenderCreated { get; set; }
    }
}