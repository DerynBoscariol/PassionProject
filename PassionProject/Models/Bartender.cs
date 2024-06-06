using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject.Models
{
    public class Bartender
    {
        //Describing bartender 
        [Key]
        public int bartenderId { get; set; }
        public string firstName { get; set; }   
        public string lastName { get; set; }
        public string email { get; set; }
        public int numDrinks { get; set; }
        public DateTime lastDrink { get; set; }
    }

    //Representing bartender in database
}