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
        public int BartenderId { get; set; }
        public string FirstName { get; set; }   
        public string LastName { get; set; }
        public string Email { get; set; }
        public int NumDrinks { get; set; }
    }

    //Data transfer object (DTO) - Communicating the bartenders info externally
    public class BartenderDto
    {
        public int BartenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int NumDrinks { get; set; }

    }
}