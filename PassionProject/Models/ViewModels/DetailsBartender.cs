using System.Collections.Generic;

namespace PassionProject.Models.ViewModels
{
    public class DetailsBartender
    {
        public BartenderDto SelectedBartender { get; set; }
        public IEnumerable<CocktailDto> CocktailsMade { get; set; }
    }
}