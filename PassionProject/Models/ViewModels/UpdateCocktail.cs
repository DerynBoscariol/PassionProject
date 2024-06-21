using System.Collections.Generic;
using Newtonsoft.Json;

namespace PassionProject.Models.ViewModels
{
    public class UpdateCocktail
    {
        public CocktailDto SelectedCocktail { get; set; }

        public IEnumerable<BartenderDto> BartenderOptions { get; set; }
    }
}