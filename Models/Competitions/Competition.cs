using HarzRollerz.MVC.Models.Collections;
using HarzRollerz.MVC.Models.SingingFeatures;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HarzRollerz.MVC.Models.Competitions
{
    public class Competition
    {
        [Key]
        public int ID { get; private set; }

        [Required, StringLength(128)]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [StringLength(32)]
        public string? Place { get; set; }

        [StringLength(32)]
        public string? Rank { get; set; }

        [Range(1, 4)]
        public int CollectionSize { get; set; } = 4;


        public List<EvaluatedFeature> EvaluatedFeatures { get; } = new List<EvaluatedFeature>();
        public List<Collection> Collections { get; } = new List<Collection>();


        public Competition()
        {
        }

        public Competition(string name, DateTime? date, string? place, string? rank, int collection_size)
        {
            Name = name;
            Date = date;
            Place = place;
            Rank = rank;
            CollectionSize = collection_size;
        }


		public string String => ToString();


		public override string ToString()
        {
            var bob = new StringBuilder(Name);

            if (!Place.IsNullOrEmpty())
            {
                bob.Append(", " + Place);
            }

            if (Date.HasValue)
            {
                bob.Append(", " + Date.Value.ToShortDateString());
            }

            if (!Rank.IsNullOrEmpty())
            {
                bob.Append(", " + Rank);
            }

            return bob.ToString();
        }
	}
}
