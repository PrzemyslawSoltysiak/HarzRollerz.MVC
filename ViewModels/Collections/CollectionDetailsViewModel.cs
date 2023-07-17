using HarzRollerz.MVC.Models.Competitions;
using HarzRollerz.MVC.Models.Entities;

namespace HarzRollerz.MVC.ViewModels.Collections
{
    public class CageDetailsViewModel
    {
        public int ID { get; set; }
        public int CanaryID { get; set; }
        public int Number { get; set; }
        public string RingNumber { get; set; } = string.Empty;
        public bool IsEvaluated { get; set; }
        public int? TotalPositivePoints { get; set; }
        public int? TotalNegativePoints { get; set; }
        public int? TotalPoints { get; set; }
        public double? TotalScore { get; set; }
        public int? Rank { get; set; }
    }

    public class CollectionDetailsViewModel
    {
        public int ID { get; set; }
        public Competition Competition { get; set; } = null!;
        public Breeder Owner { get; set; } = null!;
        public bool IsEvaluated { get; set; }
        public int? TotalPositivePoints { get; set; }
        public int? TotalNegativePoints { get; set; }
        public int? TotalPoints { get; set; }
        public double? TotalScore { get; set; }
        public int? Rank { get; set; }
        public List<CageDetailsViewModel> Cages { get; set; } = new List<CageDetailsViewModel>();
    }
}
