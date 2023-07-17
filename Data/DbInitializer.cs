using HarzRollerz.MVC.Models.Collections;
using HarzRollerz.MVC.Models.Competitions;
using HarzRollerz.MVC.Models.Entities;
using HarzRollerz.MVC.Models.Scores;
using HarzRollerz.MVC.Models.SingingFeatures;

namespace HarzRollerz.MVC.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            if (context.SingingFeatures.Any())
            {
                return;
            }

            var features = SeedSingingFeatures();
            context.SingingFeatures.AddRange(features);
            context.SaveChanges();

            var competitions = SeedCompetitions(features);
            context.Competitions.AddRange(competitions);
            context.SaveChanges();

            var breeders = SeedBreedersWithCanaries();
            context.Breeders.AddRange(breeders);
            context.SaveChanges();

            var collections = SeedCollections(competitions, breeders);
            context.Collections.AddRange(collections);
            context.SaveChanges();

            var scores = SeedScores(competitions.SkipLast(1).SelectMany(c => c.Collections).ToList());
            scores.AddRange(SeedScores(competitions.Last().Collections.SkipLast(2).ToList()));
            context.Scores.AddRange(scores);
            context.SaveChanges();
        }


        private static List<SingingFeature> SeedSingingFeatures()
        {
            var features = new List<SingingFeature>();

            features.Add(new SingingFeature("Rumble", FeatureWeight.Positive, 27, "A deep and continuous sound that resembles the sound of a motor or a pigeon’s cooing."));
            features.Add(new SingingFeature("Bass rumble", FeatureWeight.Positive, 27, "A low-pitched rumble that sounds like a bass guitar."));
            features.Add(new SingingFeature("Water pipe", FeatureWeight.Positive, 27, "A series of rapid and clear notes that sound like water flowing or bubbling."));
            features.Add(new SingingFeature("Brass bell", FeatureWeight.Positive, 18, "A bell that has a metallic quality and sounds like a brass instrument."));
            features.Add(new SingingFeature("Flute", FeatureWeight.Positive, 18, "A high-pitched and clear sound that resembles the sound of a flute."));
            features.Add(new SingingFeature("Shocker", FeatureWeight.Positive, 18, "A sudden and loud sound that resembles the sound of a whip or a firecracker."));
            features.Add(new SingingFeature("Ticking", FeatureWeight.Positive, 18, "A series of short and rhythmic sounds that resemble the sound of a clock or a metronome."));
            features.Add(new SingingFeature("Pearl bell", FeatureWeight.Positive, 3, "A bell that has a delicate and soft quality and sounds like a pearl necklace."));
            features.Add(new SingingFeature("Regular bell", FeatureWeight.Positive, 3, "A bell that has no distinctive quality and sounds like an ordinary bell."));
            features.Add(new SingingFeature("Impression", FeatureWeight.Positive, 3, "The overall effect and quality of the canary’s singing, taking into account its melody, harmony, variety, and expression."));

            features.Add(new SingingFeature("Faulty water pipe", FeatureWeight.Negative, 3, "A water pipe that is interrupted, unclear, or mixed with other sounds."));
            features.Add(new SingingFeature("Faulty ticking", FeatureWeight.Negative, 3, "Ticking that is irregular, unclear, or mixed with other sounds."));
            features.Add(new SingingFeature("Faulty flutes", FeatureWeight.Negative, 3, "Flutes that are interrupted, unclear, or mixed with other sounds."));
            features.Add(new SingingFeature("Faulty bells", FeatureWeight.Negative, 3, "Bells that are distorted, unclear, or mixed with other sounds."));

            return features;
        }


        private static List<Breeder> SeedBreedersWithCanaries()
        {
            var breeders = new List<Breeder>();

            var david = new Breeder("David", "Smith", "S001",
                "A British breeder who lives in London and has a fascination for rumbles and ticking. He is also a writer and a journalist who covers topics related to Harcen Canaries and other birds.",
                new DateTime(2018, 5, 24));

            david.Canaries.Add(new Canary(david, 1, 2.7, "John"));
            david.Canaries.Add(new Canary(david, 2, 2.2, "Paul"));
            david.Canaries.Add(new Canary(david, 3, 3.0, "George"));
            david.Canaries.Add(new Canary(david, 4, 3.1, "Ringo"));

            breeders.Add(david);
            

            var elena = new Breeder("Elena", "García", "G001",
                "A Spanish breeder who lives in Madrid and has a talent for impression and expression. She is also a singer and a composer who creates songs inspired by her canaries.",
                new DateTime(2019, 3, 16));

            elena.Canaries.Add(new Canary(elena, 1, 2.4, "Picasso"));
            elena.Canaries.Add(new Canary(elena, 2, 2.3, "Dalí"));
            elena.Canaries.Add(new Canary(elena, 3, 3.1, "Miró"));
            elena.Canaries.Add(new Canary(elena, 4, 3.4, "Goya"));
            elena.Canaries.Add(new Canary(elena, 5, 2.0, "Botero"));

            breeders.Add(elena);


            var francois = new Breeder("François", "Dubois", "D001",
                "A French breeder who lives in Paris and has a preference for bells and flutes. He is also a painter and a sculptor who creates artworks based on his canaries.",
                new DateTime(2019, 4, 12));

            francois.Canaries.Add(new Canary(francois, 1, 3.5, "Descartes"));
            francois.Canaries.Add(new Canary(francois, 2, 2.7, "Voltaire"));
            francois.Canaries.Add(new Canary(francois, 3, 3.4, "Rousseau"));
            francois.Canaries.Add(new Canary(francois, 4, 3.1, "Camus"));
            francois.Canaries.Add(new Canary(francois, 5, 2.3, "Beauvoir"));

            breeders.Add(francois);


            var giuseppe = new Breeder("Giuseppe", "Rossi", "R001",
                "An Italian breeder who lives in Rome and has a passion for water pipes and shockers. He is also a chef and a restaurateur who prepares dishes with canary ingredients.",
                new DateTime(2019, 6, 20));

            giuseppe.Canaries.Add(new Canary(giuseppe, 1, 4.7, "Pavarotti"));

            breeders.Add(giuseppe);


            var adam = new Breeder("Adam", "Kowalski", "K001",
                "A well-known breeder from Warsaw who has won several national and international awards for his canaries. He specializes in brass bells and flutes.",
                new DateTime(2019, 8, 3));

            adam.Canaries.Add(new Canary(adam, 1, 3.0, "Chopin"));
            adam.Canaries.Add(new Canary(adam, 2, 3.1, "Szymanowski"));
            adam.Canaries.Add(new Canary(adam, 3, 2.7, "Penderecki"));
            adam.Canaries.Add(new Canary(adam, 4, 2.5, "Górecki"));
            adam.Canaries.Add(new Canary(adam, 5, 3.2, "Lutosławski"));

            breeders.Add(adam);


            var julia = new Breeder("Julia", "Santos", "S002",
                "A Brazilian breeder who lives in Rio de Janeiro and has a flair for bells and flutes. She is also a model and an actress who appears in commercials and movies featuring canaries.",
                new DateTime(2020, 1, 2));

            julia.Canaries.Add(new Canary(julia, 1, 2.3, "Pelé"));
            julia.Canaries.Add(new Canary(julia, 2, 2.5, "Ronaldo"));
            julia.Canaries.Add(new Canary(julia, 3, 2.7, "Neymar"));

            breeders.Add(julia);


            var kevin = new Breeder("Kevin", "Lee", "L001",
                "An American breeder who lives in New York and has a love for water pipes and shockers. He is also a lawyer and an activist who defends the rights of canaries and other animals.",
                new DateTime(2020, 2, 17));

            kevin.Canaries.Add(new Canary(kevin, 1, 3.3, "Washington"));
            kevin.Canaries.Add(new Canary(kevin, 2, 3.1, "Lincoln"));

            breeders.Add(kevin);


            var li = new Breeder("Li", "Wei", "W001",
                "A Chinese breeder who lives in Beijing and has a gift for rumbles and ticking. He is also a doctor and an inventor who develops devices and treatments for canaries.",
                new DateTime(2020, 4, 1));

            li.Canaries.Add(new Canary(li, 1, 2.3, "Laozi"));
            li.Canaries.Add(new Canary(li, 2, 2.5, "Confucius"));
            li.Canaries.Add(new Canary(li, 3, 2.2, "Wang Wei"));
            li.Canaries.Add(new Canary(li, 4, 2.1, "Li Bai"));

            breeders.Add(li);


            var barbara = new Breeder("Barbara", "Nowak", "N001",
                "A young and talented breeder from Krakow who has a passion for water pipes and shockers. She is also a member of the Polish Harcen Canary Association.",
                new DateTime(2020, 7, 10));

            barbara.Canaries.Add(new Canary(barbara, 1, 2.7, "Słowacki"));
            barbara.Canaries.Add(new Canary(barbara, 2, 2.2, "Mickiewicz"));
            barbara.Canaries.Add(new Canary(barbara, 3, 2.9, "Norwid"));
            barbara.Canaries.Add(new Canary(barbara, 4, 2.3, "Szymborska"));
            barbara.Canaries.Add(new Canary(barbara, 5, 2.5, "Miłosz"));

            breeders.Add(barbara);


            var maria = new Breeder("Maria", "Andersson", "A001",
                "A Swedish breeder who lives in Stockholm and has a talent for impression and expression. She is also a designer and an entrepreneur who creates products and services for canaries.",
                new DateTime(2021, 3, 28));

            maria.Canaries.Add(new Canary(maria, 1, 2.6, "Agnetha"));
            maria.Canaries.Add(new Canary(maria, 2, 3.0, "Björn"));
            maria.Canaries.Add(new Canary(maria, 3, 3.6, "Benny"));
            maria.Canaries.Add(new Canary(maria, 4, 2.1, "Anni-Frid"));

            breeders.Add(maria);


            return breeders;
        }


        private static List<Competition> SeedCompetitions(List<SingingFeature> features)
        {
            var competitions = new List<Competition>();

            competitions.Add(new Competition("The Harcen Canary Song Festival", new DateTime(2020, 3, 16), null, null, 3));
            competitions.Add(new Competition("The Harcen Canary Melody Show", new DateTime(2021, 4, 20), null, null, 4));
            competitions.Add(new Competition("The Harcen Canary Harmony Contest", new DateTime(2022, 6, 8), null, null, 4));
            competitions.Add(new Competition("The Harcen Canary Sound Spectacular", new DateTime(2023, 10, 13), null, null, 4));

            foreach (var competition in competitions)
            {
                foreach (var feature in features)
                {
                    competition.EvaluatedFeatures.Add(new EvaluatedFeature(competition, feature));
                }
            }

            return competitions;
        }


        private static List<Collection> SeedCollections(List<Competition> competitions, List<Breeder> breeders)
        {
            var collections = new List<Collection>();
            foreach (var competition in competitions)
            {
                foreach (var breeder in breeders.Where(b => b.RegistrationDate <= competition.Date))
                {
                    if (breeder.Canaries.Count < competition.CollectionSize)
                    {
                        continue;
                    }
                    var collection = new Collection(competition, breeder);
                    for (int cage_number = 1; cage_number <= competition.CollectionSize; ++cage_number)
                    {
                        collection.Cages.Add(new Cage(collection, breeder.Canaries[cage_number - 1], cage_number));
                    }
                    collections.Add(collection);
                }
            }
            return collections;
        }


        private static List<Score> SeedScores(List<Collection> collections)
        {
            Random rng = new Random();
            var scores = new List<Score>();
            foreach (var collection in collections)
            {
                foreach (var feature in collection.Competition.EvaluatedFeatures)
                {
                    foreach (var cage in collection.Cages)
                    {
                        if ((feature.IsPositive() && rng.Next(100) < 10) || (feature.IsNegative() && rng.Next(100) < 70))
                        {
                            scores.Add(new Score(cage, feature));
                        }
                        else
                        {
                            int points = Math.Min(feature.MaxPoints, (int)Math.Round(rng.Next(100) * (double)feature.MaxPoints / 100));
                            scores.Add(new Score(cage, feature, points));
                        }
                    }
                }
            }
            return scores;
        }
    }
}
