using System;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition d'un personnage
    /// </summary>
    public class Character
    {
        /// <summary>
        /// Stat de base, HP
        /// </summary>
        int _baseHealth;
        /// <summary>
        /// Stat de base, ATK
        /// </summary>
        int _baseAttack;
        /// <summary>
        /// Stat de base, DEF
        /// </summary>
        int _baseDefense;
        /// <summary>
        /// Stat de base, SPE
        /// </summary>
        int _baseSpeed;
        /// <summary>
        /// Type de base
        /// </summary>
        TYPE _baseType;

        public Character(int baseHealth, int baseAttack, int baseDefense, int baseSpeed, TYPE baseType)
        {
            _baseHealth = baseHealth;
            _baseAttack = baseAttack;
            _baseDefense = baseDefense;
            _baseSpeed = baseSpeed;
            _baseType = baseType;

            CurrentHealth = baseHealth;
        }
        /// <summary>
        /// HP actuel du personnage
        /// </summary>
        public int CurrentHealth { get; private set; }
        public TYPE BaseType { get => _baseType;}
        /// <summary>
        /// HPMax, prendre en compte base et equipement potentiel
        /// </summary>
        public int MaxHealth
        {
            get
            {
                if (CurrentEquipment == null)
                {
                    return _baseHealth;
                }
                return _baseHealth + CurrentEquipment.BonusHealth;
            }
        }
        /// <summary>
        /// ATK, prendre en compte base et equipement potentiel
        /// </summary>
        public int Attack
        {
            get
            {
                if (CurrentEquipment == null)
                {
                    return _baseAttack;
                }
                return _baseAttack + CurrentEquipment.BonusAttack;
            }
        }
        /// <summary>
        /// DEF, prendre en compte base et equipement potentiel
        /// </summary>
        public int Defense
        {
            get
            {
                if (CurrentEquipment == null)
                {
                    return _baseDefense;
                }
                return _baseDefense + CurrentEquipment.BonusDefense;
            }
        }
        /// <summary>
        /// SPE, prendre en compte base et equipement potentiel
        /// </summary>
        public int Speed
        {
            get
            {
                if (CurrentEquipment == null)
                {
                    return _baseSpeed;
                }
                return _baseSpeed + CurrentEquipment.BonusSpeed;
            }
        }
        /// <summary>
        /// Equipement unique du personnage
        /// </summary>
        public Equipment CurrentEquipment { get; private set; }

        /// <summary>
        /// null si pas de status
        /// </summary>
        public StatusEffect CurrentStatus { get; private set; }

        public bool IsAlive => CurrentHealth > 0;


        /// <summary>
        /// Application d'un skill contre le personnage
        /// On pourrait potentiellement avoir besoin de connaitre le personnage attaquant,
        /// Vous pouvez adapter au besoin
        /// </summary>
        /// <param name="s">skill attaquant</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ReceiveAttackForInitialTests(Skill s)
        {
            float factor = TypeResolver.GetFactor(s.Type, _baseType);
            float damage = factor * (s.Power - Defense);
            CurrentHealth = Math.Clamp(CurrentHealth - (int)damage, 0, MaxHealth);
        }

        /// <summary>
        /// Version mise à jour, avec utilisation de l'attaque du combattant adverse, et des capacités de statut.
        /// J'ai préféré séparer cette version de l'autre, pour éviter d'avoir à modifier une bonne partie de tests de base.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        public void ReceiveAttackUpdated(Skill s, Character c)
        {
            float typeFactor = TypeResolver.GetFactor(s.Type, _baseType);
            float crazyFactor = 1f;
            if (c.CurrentStatus != null)
            {
                if (c.CurrentStatus is CrazyStatus && c == this)
                {
                    crazyFactor = CurrentStatus.DamageOnAttack;
                }
            }
            float damage = crazyFactor * typeFactor * (s.Power + c.Attack - Defense);
            CurrentHealth = Math.Clamp(CurrentHealth - (int)damage, 0, MaxHealth);

            if (CurrentStatus == null )
            {
                CurrentStatus = StatusEffect.GetNewStatusEffect(s.Status);
            }
        }
        /// <summary>
        /// Applique les dégats de la brûlure, de façon directe
        /// </summary>
        public void ReceiveBurn()
        {
            if (CurrentStatus is BurnStatus) { CurrentHealth = Math.Clamp(CurrentHealth - CurrentStatus.DamageEachTurn, 0, MaxHealth); }
        }
        /// <summary>
        /// Application directe d'un soin au personnage (mettons que le dresseur aie sorti une potion)
        /// </summary>
        /// <param name="amount"></param>
        public void ReceiveHealing(int amount)
        {
            CurrentHealth = Math.Clamp(CurrentHealth + amount, 0, MaxHealth);
        }
        /// <summary>
        /// Sert à enlever le statut du pokémon, au besoin ou à la fin de son tour
        /// </summary>
        public void ClearStatus()
        {
            CurrentStatus = null;
        }
        /// <summary>
        /// Equipe un objet au personnage
        /// </summary>
        /// <param name="newEquipment">equipement a appliquer</param>
        /// <exception cref="ArgumentNullException">Si equipement est null</exception>
        public void Equip(Equipment newEquipment)
        {
            if (newEquipment == null) { throw new ArgumentNullException("The player is not an imaginary number"); }
            CurrentEquipment = newEquipment;
        }
        /// <summary>
        /// Desequipe l'objet en cours au personnage
        /// </summary>
        public void Unequip()
        {
            CurrentEquipment = null;
            CurrentHealth = Math.Clamp(CurrentHealth, 0, MaxHealth);
        }
    }
}
