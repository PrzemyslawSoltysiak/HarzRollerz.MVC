# HarzRollerz.MVC

**HarzRollerz.MVC** is a web-based database application that allows you to conduct singing competitions of Harz roller canaries. 
It is a re-work of the **KanarkiHarcenskie** project, implemented using the Model-View-Controller design pattern (instead of Razor Pages).

<br />

## Functionality overview:

#### Singing Features:
- read saved Singing Features, along with the best percentage score achieved by Canary for a given Feature,
- add new Singing Features to be evaluated in future Competitions,
- edit saved Singing Features (without affecting the already saved Evaluated Singing Features, assigned to Competitions).

#### Breeders (with Canaries):
- read registered Breeders and access their Breeder's profiles, containing a list of Canaries owned by the Breeder, and the best rank achieved by the Breeder and his Canary,
- register new Breeders and Canaries (via the Breeder profile),
- from the Breeder's profile you can also view the Canary details, where you can find a list of Collections that the Canary belongs to.

#### Competitions (with Collections):
- read saved Competitions, including Evaluated Singing Features (with the best percentage score achieved by the Canary in a given Competition),
  and registered Collections (with a summary of scores and rank in the Competition - more detailed scores are available in Collection details and in Evaluation Card),
- add new Competitions and register new Collections for existing Competitions,
- access the Evaluation Cards, allowing for rating Collections registered for the Competitions (or viewing detailed scores).