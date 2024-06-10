using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class UpdateBartender
    {
        public BartenderDto SelectedBartender { get; set; }

        public IEnumerable<CocktailDto> CocktailsOptions { get; set; }
    }
}