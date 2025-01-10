
using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer des features et les TU sur le reste du projet

        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
        // - un heal ne régénère pas plus que les HP Max
        // - si on abaisse les HPMax les HP courant doivent suivre si c'est au dessus de la nouvelle valeur
        // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
        // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir
        // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs)
        // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type
        // - L'utilisation d'objets : Potion, SuperPotion, Vitess+, Attack+ etc.
        // - Gérer les PP (limite du nombre d'utilisation) d'une attaque,
        // si on selectionne une attack qui a 0 PP on inflige 0

        // Choisis ce que tu veux ajouter comme feature et fait en au max.
        // Les nouveaux TU doivent être dans ce fichier.
        // Modifiant des features il est possible que certaines valeurs
        // des TU précédentes ne matchent plus, tu as le droit de réadapter les valeurs
        // de ces anciens TU pour ta nouvelle situation.

        [Test]
        public void OverhealCheck()
        {
            var c = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Assert.That(c.CurrentHealth, Is.EqualTo(c.MaxHealth));
            c.ReceiveHealing(20); //La potion récupérée au PC dans la maison au début du jeu
            Assert.That(c.CurrentHealth, Is.EqualTo(c.MaxHealth));
        }

        [Test]
        public void CurrentHealthAfterUnequipping()
        {
            var c = new Character(100, 50, 30, 20, TYPE.NORMAL);
            var e = new Equipment(100, 90, 70, 12);

            c.Equip(e);
            c.ReceiveHealing(80);
            Assert.That(c.CurrentHealth, Is.EqualTo(180));

            c.Unequip();
            Assert.That(c.CurrentHealth, Is.EqualTo(c.MaxHealth));
        }

        [Test]
        public void RattataUtiliseViveAttaque()
        {
            var a = new Character(20, 50, 30, 20, TYPE.NORMAL);
            var b = new Character(20, 50, 30, 10, TYPE.NORMAL);
            var e = new Equipment(100, 90, 70, 0);
            e.MakeEquipmentBeReallyFast();

            Fight f = new Fight(a, b);
            Punch p = new Punch();

            f.ExecuteTurnForInitialTests(p, p);
            Assert.That(b.IsAlive, Is.False);
            Assert.That(a.IsAlive, Is.True);

            b.ReceiveHealing(20);
            b.Equip(e);
            f.ExecuteTurnForInitialTests(p, p);
            Assert.That(a.IsAlive, Is.False);
            Assert.That(b.IsAlive, Is.True);

            b.Unequip();
            a.ReceiveHealing(20);
            f.ExecuteTurnForInitialTests(p, p);
            Assert.That(b.IsAlive, Is.False);
            Assert.That(a.IsAlive, Is.True);
        }

        [Test]
        public void UneMinuteDexpositionATwitter()
        {
            var a = new Character(1000, 50, 30, 20, TYPE.NORMAL);
            var b = new Character(1000, 50, 30, 20, TYPE.NORMAL);

            var punch = new Punch();
            var megapunch = new MegaPunch();
            var sleep = new MagicalGrass();
            var burn = new FireBall();
            var crazy = new TrendInstagram();

            Fight f = new Fight(a, b);

            f.ExecuteTurnUpdated(punch, crazy);
            Assert.That(a.CurrentStatus is CrazyStatus);

            int tempHealth = b.CurrentHealth;
            f.ExecuteTurnUpdated(punch, punch);
            Assert.That(a.CurrentHealth, Is.EqualTo(tempHealth));
            Assert.That(b.CurrentStatus, Is.Null);
        }
    }
}
