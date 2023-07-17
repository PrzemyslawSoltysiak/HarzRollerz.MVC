using HarzRollerz.MVC.Models.Entities;
using HarzRollerz.MVC.Models.Scores;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarzRollerz.MVC.Models.Collections
{
    public class Cage
    {
        [Key]
        public int ID { get; private set; }

        [ForeignKey(nameof(Collection))]
        public int CollectionID { get; private set; }

        [ForeignKey(nameof(Canary))]
        public int CanaryID { get; private set; }

        [Range(1, 4)]
        public int Number { get; set; }


        public Collection Collection { get; set; } = null!;
        public Canary Canary { get; set; } = null!;

        public List<Score> Scores { get; } = new List<Score>();


        public Cage()
        {
        }

        public Cage(Collection collection, Canary canary, int number)
        {
            CollectionID = collection.ID;
            Collection = collection;
            CanaryID = canary.ID;
            Canary = canary;
            Number = number;
        }
    }
}
