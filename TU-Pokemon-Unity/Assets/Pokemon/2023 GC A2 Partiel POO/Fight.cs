
using System;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    public class Fight
    {
        public Fight(Character character1, Character character2)
        {
            if (character1 == null || character2 == null)
            {
                throw new ArgumentNullException("Even ghost-types somehow manage to exist, this is ridiculous");
            }
            Character1 = character1;
            Character2 = character2;
        }

        public Character Character1 { get; }
        public Character Character2 { get; }
        /// <summary>
        /// Est-ce la condition de victoire/défaite a été rencontré ?
        /// </summary>
        public bool IsFightFinished => !Character1.IsAlive || !Character2.IsAlive;

        /// <summary>
        /// Jouer l'enchainement des attaques. Attention à bien gérer l'ordre des attaques par apport à la speed des personnages
        /// </summary>
        /// <param name="skillFromCharacter1">L'attaque selectionné par le joueur 1</param>
        /// <param name="skillFromCharacter2">L'attaque selectionné par le joueur 2</param>
        /// <exception cref="ArgumentNullException">si une des deux attaques est null</exception>
        public void ExecuteTurnForInitialTests(Skill skillFromCharacter1, Skill skillFromCharacter2)
        {
            if (Character1.CurrentEquipment != null)
            {
                if (Character1.CurrentEquipment.HasPriority)
                {
                    Character2.ReceiveAttackForInitialTests(skillFromCharacter1);
                    if (Character2.IsAlive)
                    {
                        Character1.ReceiveAttackForInitialTests(skillFromCharacter2);
                    }
                    return;
                }
            }
            if (Character2.CurrentEquipment != null)
            {
                if (Character2.CurrentEquipment.HasPriority)
                {
                    Character1.ReceiveAttackForInitialTests(skillFromCharacter2);
                    if (Character1.IsAlive)
                    {
                        Character2.ReceiveAttackForInitialTests(skillFromCharacter1);
                    }
                    return;
                }
            }
            //If both have priority, you're going back to normal calculation
            var randomTurnOrderForSpeedTie = new Random();
            int highestSpeed = Math.Max(Character1.Speed, Character2.Speed);
            if (highestSpeed == Character1.Speed && highestSpeed != Character2.Speed)
            {
                Character2.ReceiveAttackForInitialTests(skillFromCharacter1);
                if (Character2.IsAlive)
                {
                    Character1.ReceiveAttackForInitialTests(skillFromCharacter2);
                }
            }
            else if (highestSpeed == Character2.Speed && highestSpeed != Character1.Speed)
            {
                Character1.ReceiveAttackForInitialTests(skillFromCharacter2);
                if (Character1.IsAlive)
                {
                    Character2.ReceiveAttackForInitialTests(skillFromCharacter1);
                }
            }
            else
            {
                int goingFirst = randomTurnOrderForSpeedTie.Next(0, 2);
                switch (goingFirst)
                {
                    case 0:
                        Character2.ReceiveAttackForInitialTests(skillFromCharacter1);
                        if (Character2.IsAlive)
                        {
                            Character1.ReceiveAttackForInitialTests(skillFromCharacter2);
                        }
                        break;
                    case 1:
                        Character1.ReceiveAttackForInitialTests(skillFromCharacter2);
                        if (Character1.IsAlive)
                        {
                            Character2.ReceiveAttackForInitialTests(skillFromCharacter1);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Une nouvelle méthode pour les fonctionnalités testées aux tests additionnels.
        /// </summary>
        /// <param name="skillFromCharacter1"></param>
        /// <param name="skillFromCharacter2"></param>
        public void ExecuteTurnUpdated(Skill skillFromCharacter1, Skill skillFromCharacter2)
        {
            if (Character1.CurrentStatus != null)
            {
                if (Character1.CurrentStatus is CrazyStatus)
                {
                    Character1.ReceiveAttackUpdated(skillFromCharacter1, Character1);
                    Character1.ReceiveAttackUpdated(skillFromCharacter2, Character2);
                    ManageEndOfTurn();
                    return;
                }
                if (Character1.CurrentStatus is SleepStatus)
                {
                    Character1.ReceiveAttackUpdated(skillFromCharacter2, Character2);
                    ManageEndOfTurn();
                    return;
                }
            }
            if (Character2.CurrentStatus != null)
            {
                if (Character2.CurrentStatus is CrazyStatus)
                {
                    Character2.ReceiveAttackUpdated(skillFromCharacter2, Character2);
                    Character2.ReceiveAttackUpdated(skillFromCharacter1, Character1);
                    ManageEndOfTurn();
                    return;
                }
                if (Character2.CurrentStatus is SleepStatus)
                {
                    Character2.ReceiveAttackUpdated(skillFromCharacter1, Character1);
                    ManageEndOfTurn();
                    return;
                }
            }

            if (Character1.CurrentEquipment != null)
            {
                if (Character1.CurrentEquipment.HasPriority)
                {
                    Character2.ReceiveAttackUpdated(skillFromCharacter1, Character1);
                    if (Character2.IsAlive)
                    {
                        Character1.ReceiveAttackUpdated(skillFromCharacter2, Character2);
                    }
                    ManageEndOfTurn();
                    return;
                }
            }
            if (Character2.CurrentEquipment != null)
            {
                if (Character2.CurrentEquipment.HasPriority)
                {
                    Character1.ReceiveAttackUpdated(skillFromCharacter2, Character2);
                    if (Character1.IsAlive)
                    {
                        Character2.ReceiveAttackUpdated(skillFromCharacter1, Character1);
                    }
                    ManageEndOfTurn();
                    return;
                }
            }
            //If both have priority, you're going back to normal calculation
            var randomTurnOrderForSpeedTie = new Random();
            int highestSpeed = Math.Max(Character1.Speed, Character2.Speed);
            if (highestSpeed == Character1.Speed && highestSpeed != Character2.Speed)
            {
                Character2.ReceiveAttackUpdated(skillFromCharacter1, Character1);
                if (Character2.IsAlive)
                {
                    Character1.ReceiveAttackUpdated(skillFromCharacter2, Character2);
                }
                ManageEndOfTurn();
            }
            else if (highestSpeed == Character2.Speed && highestSpeed != Character1.Speed)
            {
                Character1.ReceiveAttackUpdated(skillFromCharacter2, Character2);
                if (Character1.IsAlive)
                {
                    Character2.ReceiveAttackUpdated(skillFromCharacter1, Character1);
                }
                ManageEndOfTurn();
            }
            else
            {
                int goingFirst = randomTurnOrderForSpeedTie.Next(0, 2);
                switch (goingFirst)
                {
                    case 0:
                        Character2.ReceiveAttackUpdated(skillFromCharacter1, Character1);
                        if (Character2.IsAlive)
                        {
                            Character1.ReceiveAttackUpdated(skillFromCharacter2, Character2);
                        }
                        ManageEndOfTurn();
                        break;
                    case 1:
                        Character1.ReceiveAttackUpdated(skillFromCharacter2, Character2);
                        if (Character1.IsAlive)
                        {
                            Character2.ReceiveAttackUpdated(skillFromCharacter1, Character1);
                        }
                        ManageEndOfTurn();
                        break;
                }
            }
        }

        void ManageEndOfTurn()
        {
            switch (Character1.CurrentStatus)
            {
                case SleepStatus:
                    Character1.CurrentStatus.EndTurn();
                    if(Character1.CurrentStatus.RemainingTurn == 0)
                    {
                        Character1.ClearStatus();
                    }
                    break;
                case BurnStatus:
                    Character1.ReceiveBurn();
                    Character1.CurrentStatus.EndTurn();
                    if (Character1.CurrentStatus.RemainingTurn == 0)
                    {
                        Character1.ClearStatus();
                    }
                    break;
                case CrazyStatus:
                    Character1.CurrentStatus.EndTurn();
                    if (Character1.CurrentStatus.RemainingTurn == 0)
                    {
                        Character1.ClearStatus();
                    }
                    break;
                case null:
                default:
                    break;
            }
            switch (Character2.CurrentStatus)
            {
                case SleepStatus:
                    Character2.CurrentStatus.EndTurn();
                    if (Character2.CurrentStatus.RemainingTurn == 0)
                    {
                        Character2.ClearStatus();
                    }
                    break;
                case BurnStatus:
                    Character2.ReceiveBurn();
                    Character2.CurrentStatus.EndTurn();
                    if (Character2.CurrentStatus.RemainingTurn == 0)
                    {
                        Character2.ClearStatus();
                    }
                    break;
                case CrazyStatus:
                    Character2.CurrentStatus.EndTurn();
                    if (Character2.CurrentStatus.RemainingTurn == 0)
                    {
                        Character2.ClearStatus();
                    }
                    break;
                case null:
                default:
                    break;
            }
        }
    }
}
