using Codebreak.Framework.Generic;
using Codebreak.Service.World.Game.Fight.Effect.Type;
using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Effect
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EffectManager : Singleton<EffectManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<EffectEnum, EffectBase> _effects;

        /// <summary>
        /// 
        /// </summary>
        public EffectManager()
        {
            _effects = new Dictionary<EffectEnum, EffectBase>();

            // Dégats
            _effects.Add(EffectEnum.DamageEarth, new DamageEffect());
            _effects.Add(EffectEnum.DamageNeutral, new DamageEffect());
            _effects.Add(EffectEnum.DamageFire, new DamageEffect());
            _effects.Add(EffectEnum.DamageWater, new DamageEffect());
            _effects.Add(EffectEnum.DamageAir, new DamageEffect());

            // Vol de statistique
            _effects.Add(EffectEnum.StealNeutral, new LifeStealEffect());
            _effects.Add(EffectEnum.StealEarth, new LifeStealEffect());
            _effects.Add(EffectEnum.StealFire, new LifeStealEffect());
            _effects.Add(EffectEnum.StealWater, new LifeStealEffect());
            _effects.Add(EffectEnum.StealAir, new LifeStealEffect());

            // Soin
            _effects.Add(EffectEnum.Heal, new HealEffect());

            // Teleporation
            _effects.Add(EffectEnum.Teleport, new TeleportEffect());

            // Armure et bouclié feca
            _effects.Add(EffectEnum.AddArmor, new ArmorEffect());
            _effects.Add(EffectEnum.AddArmorBis, new ArmorEffect());

            // Ajout ou reduction AP/MP
            _effects.Add(EffectEnum.AddAP, new StatsEffect());
            _effects.Add(EffectEnum.AddMP, new StatsEffect());
            _effects.Add(EffectEnum.SubAP, new StatsEffect());
            _effects.Add(EffectEnum.SubMP, new StatsEffect());
            _effects.Add(EffectEnum.SubAPDodgeable, new APDodgeSubstractEffect());
            _effects.Add(EffectEnum.SubMPDodgeable, new MPDodgeSubstractEffect());
            _effects.Add(EffectEnum.AddAPDodge, new StatsEffect());
            _effects.Add(EffectEnum.AddMPDodge, new StatsEffect());
            _effects.Add(EffectEnum.SubAPDodge, new StatsEffect());
            _effects.Add(EffectEnum.SubMPDodge, new StatsEffect());

            // Caracteristiques Ajout/Reduction
            _effects.Add(EffectEnum.AddStrength, new StatsEffect());
            _effects.Add(EffectEnum.AddIntelligence, new StatsEffect());
            _effects.Add(EffectEnum.AddAgility, new StatsEffect());
            _effects.Add(EffectEnum.AddChance, new StatsEffect());
            _effects.Add(EffectEnum.AddWisdom, new StatsEffect());
            _effects.Add(EffectEnum.AddLife, new StatsEffect());
            _effects.Add(EffectEnum.AddVitality, new StatsEffect());
            _effects.Add(EffectEnum.SubStrength, new StatsEffect());
            _effects.Add(EffectEnum.SubIntelligence, new StatsEffect());
            _effects.Add(EffectEnum.SubAgility, new StatsEffect());
            _effects.Add(EffectEnum.SubChance, new StatsEffect());
            _effects.Add(EffectEnum.SubWisdom, new StatsEffect());
            _effects.Add(EffectEnum.SubVitality, new StatsEffect());
            _effects.Add(EffectEnum.AddInvocationMax, new StatsEffect());

            // Soins
            _effects.Add(EffectEnum.AddHealCare, new StatsEffect());
            _effects.Add(EffectEnum.SubHealCare, new StatsEffect());

            // Resistances ajout/suppressions
            _effects.Add(EffectEnum.AddReduceDamageAir, new StatsEffect());
            _effects.Add(EffectEnum.AddReduceDamageWater, new StatsEffect());
            _effects.Add(EffectEnum.AddReduceDamageFire, new StatsEffect());
            _effects.Add(EffectEnum.AddReduceDamageNeutral, new StatsEffect());
            _effects.Add(EffectEnum.AddReduceDamageEarth, new StatsEffect());
            _effects.Add(EffectEnum.SubReduceDamageAir, new StatsEffect());
            _effects.Add(EffectEnum.SubReduceDamageWater, new StatsEffect());
            _effects.Add(EffectEnum.SubReduceDamageFire, new StatsEffect());
            _effects.Add(EffectEnum.SubReduceDamageNeutral, new StatsEffect());
            _effects.Add(EffectEnum.SubReduceDamageEarth, new StatsEffect());

            _effects.Add(EffectEnum.AddReduceDamagePercentAir, new StatsEffect());
            _effects.Add(EffectEnum.AddReduceDamagePercentWater, new StatsEffect());
            _effects.Add(EffectEnum.AddReduceDamagePercentFire, new StatsEffect());
            _effects.Add(EffectEnum.AddReduceDamagePercentNeutral, new StatsEffect());
            _effects.Add(EffectEnum.AddReduceDamagePercentEarth, new StatsEffect());
            _effects.Add(EffectEnum.SubReduceDamagePercentAir, new StatsEffect());
            _effects.Add(EffectEnum.SubReduceDamagePercentWater, new StatsEffect());
            _effects.Add(EffectEnum.SubReduceDamagePercentFire, new StatsEffect());
            _effects.Add(EffectEnum.SubReduceDamagePercentNeutral, new StatsEffect());
            _effects.Add(EffectEnum.SubReduceDamagePercentEarth, new StatsEffect());

            // Ajout ou reduction de dommage
            _effects.Add(EffectEnum.AddDamage, new StatsEffect());
            _effects.Add(EffectEnum.AddEchecCritic, new StatsEffect());
            _effects.Add(EffectEnum.AddDamageCritic, new StatsEffect());
            _effects.Add(EffectEnum.AddDamagePercent, new StatsEffect());
            _effects.Add(EffectEnum.SubDamagePercent, new StatsEffect());
            _effects.Add(EffectEnum.SubDamage, new StatsEffect());
            _effects.Add(EffectEnum.SubDamageCritic, new StatsEffect());
            _effects.Add(EffectEnum.AddReflectDamage, new StatsEffect());

            // Chatiment sacris
            _effects.Add(EffectEnum.AddChatiment, new PunishmentEffect());

            // Effet de push back/fear
            _effects.Add(EffectEnum.PushBack, new PushEffect());
            _effects.Add(EffectEnum.PushFront, new PushEffect());
            _effects.Add(EffectEnum.PushFear, new PushFearEffect());

            // Ajout d'un etat / changement de skin
            _effects.Add(EffectEnum.ChangeSkin, new SkinChangeEffect());
            _effects.Add(EffectEnum.AddState, new StateAddEffect());
            _effects.Add(EffectEnum.RemoveState, new StateRemoveEffect());
            _effects.Add(EffectEnum.Stealth, new StateAddEffect());

            // Steal de statistique
            _effects.Add(EffectEnum.StrengthSteal, new StatsStealEffect());
            _effects.Add(EffectEnum.WisdomSteal, new StatsStealEffect());
            _effects.Add(EffectEnum.IntelligenceSteal, new StatsStealEffect());
            _effects.Add(EffectEnum.AgilitySteal, new StatsStealEffect());
            _effects.Add(EffectEnum.ChanceSteal, new StatsStealEffect());
            _effects.Add(EffectEnum.APSteal, new StatsStealEffect());
            _effects.Add(EffectEnum.MPSteal, new StatsStealEffect());
            _effects.Add(EffectEnum.StealPO, new StatsStealEffect());

            // Autres
            _effects.Add(EffectEnum.DamageLifeNeutral, new DamageLifePercentEffect());
            _effects.Add(EffectEnum.EcaflipChance, new EcaflipChanceEffect());
            _effects.Add(EffectEnum.Punition, new PunishmentDamageEffect());

            // Sacrifice
            _effects.Add(EffectEnum.Sacrifice, new SacrificeEffect());
            _effects.Add(EffectEnum.Transpose, new TransposeEffect());

            // Derobade
            _effects.Add(EffectEnum.Evasion, new DamageDodgeEffect());
            
            // Augmente de X les domamges de base du sort Y
            _effects.Add(EffectEnum.IncreaseSpellDamage, new IncreaseSpellJetEffect());


            // Debuff
            _effects.Add(EffectEnum.DeleteAllBonus, new BuffRemoveEffect());

            // Panda
            _effects.Add(EffectEnum.PandaCarrier, new PandaCarrierEffect());
            _effects.Add(EffectEnum.PandaLaunch, new PandaLaunchEffect());

            // ActivableObjects
            _effects.Add(EffectEnum.UseGlyph, new ActivableObjectEffect());
            _effects.Add(EffectEnum.UseTrap, new ActivableObjectEffect());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public FightActionResultEnum TryApplyEffect(CastInfos castInfos)
        {
            if (!_effects.ContainsKey(castInfos.EffectType))
            {
                Logger.Debug("EffectManager::TryApplyEffect unknow effect : " + castInfos.EffectType);
                return FightActionResultEnum.RESULT_NOTHING;
            }
            return _effects[castInfos.EffectType].ApplyEffect(castInfos);
        }
    }
}
