using HarzRollerz.MVC.Models.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarzRollerz.MVC.Models.Entities
{
    public class Canary
    {
        [Key]
        public int ID { get; private set; }

        [ForeignKey(nameof(Owner))]
        public int OwnerID { get; set; }

        public int OrdinalNumber { get; set; }

        [Range(0, 5)]
        public double InnerRingDiameter { get; set; }

        [StringLength(32)]
        public string? Name { get; set; }


        public Breeder Owner { get; set; } = null!;

        public List<Cage> Cages { get; } = new List<Cage>();

        
        public Canary()
        {
        }

        public Canary(Breeder owner, int no, double diameter = 2.5, string? name = null)
        {
            OwnerID = owner.ID;
            Owner = owner;
            OrdinalNumber = no;
            InnerRingDiameter = diameter;
            Name = name;
        }


        public string OrdinalNumberString => OrdinalNumber.ToString("D3");

        public string InnerRingDiameterString => InnerRingDiameter.ToString("F1");

        public string RingNumber => $"[ HR | {Owner?.Signature ?? "A000"} | {InnerRingDiameterString} | {OrdinalNumberString} ]";

    }
}
