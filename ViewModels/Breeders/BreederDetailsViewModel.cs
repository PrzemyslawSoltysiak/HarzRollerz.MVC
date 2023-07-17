using HarzRollerz.MVC.Models.Entities;
using HarzRollerz.MVC.Services;

namespace HarzRollerz.MVC.ViewModels.Breeders
{
    public class CanaryViewModel
    {
        public int ID { get; set; }
        public int OrdinalNumber { get; set; }
        public string? Name { get; set; }
        public string RingNumber { get; set; } = string.Empty;

        public CanaryViewModel() { }

        public CanaryViewModel(Canary canary)
        {
            ID = canary.ID;
            OrdinalNumber = canary.OrdinalNumber;
            Name = canary.Name;
            RingNumber = canary.RingNumber;
        }
    }

    public class BreederDetailsViewModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
        public string String { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public int? BestCollectionRank { get; set; }
        public int? BestCanaryRank { get; set; }
        public List<CanaryViewModel> Canaries { get; } = new List<CanaryViewModel>();

        public BreederDetailsViewModel() { }

        public BreederDetailsViewModel(Breeder breeder, IEnumerable<Canary> canaries, ScoresService scores_service)
        {
            ID = breeder.ID;
            FirstName = breeder.FirstName;
            LastName = breeder.LastName;
            String = breeder.String;
            Signature = breeder.Signature;
            RegistrationDate = breeder.RegistrationDate;
            BestCollectionRank = scores_service.GetBestBreedersCollectionRank(breeder.ID);
            BestCanaryRank = scores_service.GetBestBreedersCanaryRank(breeder.ID);
            foreach (var canary in canaries.OrderBy(c => c.OrdinalNumber))
            {
                Canaries.Add(new CanaryViewModel(canary));
            }
        }
    }
}
