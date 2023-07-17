using HarzRollerz.MVC.Models.Collections;
using System.ComponentModel.DataAnnotations;

namespace HarzRollerz.MVC.Models.Entities
{
    public class Breeder
    {
        [Key]
        public int ID { get; private set; }

        [RegularExpression(@"[A-Z][0-9]{3}")]
        public string Signature { get; set; } = string.Empty;

        [RegularExpression(@"^[A-Z][a-z]+", ErrorMessage = "Invalid format.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [RegularExpression(@"^[A-z][a-z]+", ErrorMessage = "Invalid format.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }

        public string? About { get; set; }


        public List<Canary> Canaries { get; set; } = new List<Canary>();
        public List<Collection> Collections { get; set; } = new List<Collection>();


        public Breeder()
        {
        }

        public Breeder(string first_name, string last_name, string signature, string? about = null, DateTime? registration_date = null)
        {
            FirstName = first_name;
            LastName = last_name;
            Signature = signature;
            About = about;
            RegistrationDate = registration_date ?? DateTime.UtcNow;
        }


        public string FirstLastName => FirstName + " " + LastName;

        public string String => FirstLastName + " (" + Signature + ")";
    }
}
