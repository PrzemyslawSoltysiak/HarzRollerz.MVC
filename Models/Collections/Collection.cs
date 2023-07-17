using HarzRollerz.MVC.Models.Competitions;
using HarzRollerz.MVC.Models.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarzRollerz.MVC.Models.Collections
{
    public class Collection
    {
        [Key]
        public int ID { get; private set; }

        [ForeignKey(nameof(Competition))]
        public int CompetitionID { get; private set; }

        [ForeignKey(nameof(Owner))]
        public int OwnerID { get; private set; }


        public Competition Competition { get; set; } = null!;
        public Breeder Owner { get; set; } = null!;

        public List<Cage> Cages { get; } = new List<Cage>();


        public Collection() { }

        public Collection(Competition competition, Breeder owner)
        {
            CompetitionID = competition.ID;
            Competition = competition;
            OwnerID = owner.ID;
            Owner = owner;
        }

    }
}
