using System.ComponentModel.DataAnnotations;

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