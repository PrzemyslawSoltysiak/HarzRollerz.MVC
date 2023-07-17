using HarzRollerz.MVC.Models.Collections;
using HarzRollerz.MVC.Models.SingingFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarzRollerz.MVC.Models.Scores
{
    public class Score
    {
        [Key]
        public int ID { get; private set; }

        [ForeignKey(nameof(Cage))]
        public int CageID { get; set; }

        [ForeignKey(nameof(EvaluatedFeature))]
        public int EvaluatedFeatureID { get; set; }

        public int? Points { get; set; }


        public Cage Cage { get; set; } = null!;
        public EvaluatedFeature EvaluatedFeature { get; set; } = null!;


        public Score()
        {
        }

        public Score(Cage cage, EvaluatedFeature feature, int? points = null)
        {
            CageID = cage.ID;
            Cage = cage;
            EvaluatedFeatureID = feature.ID;
            EvaluatedFeature = feature;
            Points = points;
        }
    }
}
