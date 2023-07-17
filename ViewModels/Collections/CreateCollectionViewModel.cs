using HarzRollerz.MVC.Models.Competitions;
using HarzRollerz.MVC.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Versioning;

namespace HarzRollerz.MVC.ViewModels.Collections
{
    public class CageViewModel
    {
        public int CageNumber { get; set; }
        // public int? CanaryID { get; set; }
        public string? CanaryRingNumber { get; set; }

        public CageViewModel()
        {
        }

        public CageViewModel(int number, Canary? canary = null)
        {
            CageNumber = number;
            // CanaryID = canary?.ID;
            CanaryRingNumber = canary?.RingNumber;
        }
    }

    public class CanaryViewModel
    {
        // public int ID { get; set; }
        public string RingNumber { get; set; } = string.Empty;
        public string? Name { get; set; }

        public CanaryViewModel()
        {
        }

        public CanaryViewModel(Canary canary)
        {
            // ID = canary.ID;
            RingNumber = canary.RingNumber;
            Name = canary.Name;
        }
    }

    public class CreateCollectionViewModel
    {
        public int CompetitionID { get; set; }
        public string CompetitionString { get; set; } = string.Empty;
        public int? OwnerID { get; set; }
        public SelectList AvailableOwners { get; set; } = null!;
        public CageViewModel[] Cages { get; set; } = new CageViewModel[0];
        public CanaryViewModel[] AvailableCanaries { get; set; } = new CanaryViewModel[0];

        public CreateCollectionViewModel()
        {
        }

        public CreateCollectionViewModel(Competition competition, IEnumerable<Breeder> available_owners)
        {
            CompetitionID = competition.ID;
            CompetitionString = competition.String;
            AvailableOwners = new SelectList(available_owners, "ID", "String");
            Cages = new CageViewModel[competition.CollectionSize];
            for (int number = 1; number <= competition.CollectionSize; ++number)
            {
                Cages[number - 1] = new CageViewModel(number);
            }
        }

        public CreateCollectionViewModel(Competition competition, IEnumerable<Breeder> available_owners, Breeder selected_owner, IEnumerable<Canary> owners_canaries)
        {
            CompetitionID = competition.ID;
            CompetitionString = competition.String;
            AvailableOwners = new SelectList(available_owners, "ID", "String", selected_owner.ID);
            OwnerID = selected_owner.ID;
            AvailableCanaries = owners_canaries.Select(c => new CanaryViewModel(c)).ToArray();
            Cages = new CageViewModel[competition.CollectionSize];
            for (int number = 1; number <= competition.CollectionSize; ++number)
            {
                Cages[number - 1] = new CageViewModel(number);
            }
        }

        public CreateCollectionViewModel(Competition competition, IEnumerable<Breeder> available_owners, Breeder selected_owner, IEnumerable<Canary> owners_canaries, IList<Canary?> selected_canaries)
        {
            CompetitionID = competition.ID;
            CompetitionString = competition.String;
            AvailableOwners = new SelectList(available_owners, "ID", "String", selected_owner.ID);
            OwnerID = selected_owner.ID;
            AvailableCanaries = owners_canaries.Where(c => !selected_canaries.Contains(c)).Select(c => new CanaryViewModel(c)).ToArray();
            Cages = new CageViewModel[competition.CollectionSize];
            for (int number = 1; number <= competition.CollectionSize; ++number)
            {
                Cages[number - 1] = new CageViewModel(number, selected_canaries[number - 1]);
            }
        }
    }
}
