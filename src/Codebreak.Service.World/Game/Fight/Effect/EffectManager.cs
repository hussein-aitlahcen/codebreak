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
        private Dictionary<EffectEnum, AbstractSpellEffect> m_effects;

        /// <summary>
        /// 
        /// </summary>
        public EffectManager()
        {
            m_effects = new Dictionary<EffectEnum, AbstractSpellEffect>();

            // Dégats
            m_effects.Add(EffectEnum.SelfDamage, new SelfDamageEffect());
            m_effects.Add(EffectEnum.DamageEarth, new DamageEffect());
            m_effects.Add(EffectEnum.DamageNeutral, new DamageEffect());
            m_effects.Add(EffectEnum.DamageFire, new DamageEffect());
            m_effects.Add(EffectEnum.DamageWater, new DamageEffect());
            m_effects.Add(EffectEnum.DamageAir, new DamageEffect());
            m_effects.Add(EffectEnum.DamageLifeNeutral, new DamageLifePercentEffect(EffectEnum.DamageBrut));
            m_effects.Add(EffectEnum.DamageLifeAir, new DamageLifePercentEffect(EffectEnum.DamageAir));
            m_effects.Add(EffectEnum.DamageLifeEarth, new DamageLifePercentEffect(EffectEnum.DamageEarth));
            m_effects.Add(EffectEnum.DamageLifeFire, new DamageLifePercentEffect(EffectEnum.DamageFire));
            m_effects.Add(EffectEnum.DamageLifeWater, new DamageLifePercentEffect(EffectEnum.DamageWater));
            m_effects.Add(EffectEnum.DamageDropLife, new DropLifeEffect());
            m_effects.Add(EffectEnum.Punition, new PunishmentDamageEffect());
            m_effects.Add(EffectEnum.ReflectSpell, new ReflectSpellEffect());
            m_effects.Add(EffectEnum.LifeSteal, new PureLifeStealEffect());
            m_effects.Add(EffectEnum.DamagePerAP, new DamagePerAPEffect());

            // Vol de statistique
            m_effects.Add(EffectEnum.StealNeutral, new LifeStealEffect());
            m_effects.Add(EffectEnum.StealEarth, new LifeStealEffect());
            m_effects.Add(EffectEnum.StealFire, new LifeStealEffect());
            m_effects.Add(EffectEnum.StealWater, new LifeStealEffect());
            m_effects.Add(EffectEnum.StealAir, new LifeStealEffect());

            // Soin
            m_effects.Add(EffectEnum.Heal, new HealEffect());

            // Teleporation
            m_effects.Add(EffectEnum.Teleport, new TeleportEffect());

            // Armure et bouclié feca
            m_effects.Add(EffectEnum.AddArmor, new ArmorEffect());
            m_effects.Add(EffectEnum.AddArmorBis, new ArmorEffect());

            // Ajout ou reduction AP/MP
            m_effects.Add(EffectEnum.AddAP, new StatsEffect());
            m_effects.Add(EffectEnum.AddMP, new StatsEffect());
            m_effects.Add(EffectEnum.SubAP, new StatsEffect());
            m_effects.Add(EffectEnum.SubMP, new StatsEffect());
            m_effects.Add(EffectEnum.SubAPDodgeable, new APDodgeSubstractEffect());
            m_effects.Add(EffectEnum.SubMPDodgeable, new MPDodgeSubstractEffect());
            m_effects.Add(EffectEnum.AddAPDodge, new StatsEffect());
            m_effects.Add(EffectEnum.AddMPDodge, new StatsEffect());
            m_effects.Add(EffectEnum.SubAPDodge, new StatsEffect());
            m_effects.Add(EffectEnum.SubMPDodge, new StatsEffect());

            // Caracteristiques Ajout/Reduction
            m_effects.Add(EffectEnum.AddReduceDamagePhysic, new StatsEffect());
            m_effects.Add(EffectEnum.AddReduceDamageMagic, new StatsEffect());
            m_effects.Add(EffectEnum.AddPO, new StatsEffect());
            m_effects.Add(EffectEnum.SubPO, new StatsEffect());
            m_effects.Add(EffectEnum.AddStrength, new StatsEffect());
            m_effects.Add(EffectEnum.AddIntelligence, new StatsEffect());
            m_effects.Add(EffectEnum.AddAgility, new StatsEffect());
            m_effects.Add(EffectEnum.AddChance, new StatsEffect());
            m_effects.Add(EffectEnum.AddWisdom, new StatsEffect());
            m_effects.Add(EffectEnum.AddLife, new StatsEffect());
            m_effects.Add(EffectEnum.AddVitality, new StatsEffect());
            m_effects.Add(EffectEnum.SubStrength, new StatsEffect());
            m_effects.Add(EffectEnum.SubIntelligence, new StatsEffect());
            m_effects.Add(EffectEnum.SubAgility, new StatsEffect());
            m_effects.Add(EffectEnum.SubChance, new StatsEffect());
            m_effects.Add(EffectEnum.SubWisdom, new StatsEffect());
            m_effects.Add(EffectEnum.SubVitality, new StatsEffect());
            m_effects.Add(EffectEnum.AddInvocationMax, new StatsEffect());
            m_effects.Add(EffectEnum.AddProspection, new StatsEffect());

            // Soins
            m_effects.Add(EffectEnum.AddHealCare, new StatsEffect());
            m_effects.Add(EffectEnum.SubHealCare, new StatsEffect());

            // Resistances ajout/suppressions
            m_effects.Add(EffectEnum.AddReduceDamageAir, new StatsEffect());
            m_effects.Add(EffectEnum.AddReduceDamageWater, new StatsEffect());
            m_effects.Add(EffectEnum.AddReduceDamageFire, new StatsEffect());
            m_effects.Add(EffectEnum.AddReduceDamageNeutral, new StatsEffect());
            m_effects.Add(EffectEnum.AddReduceDamageEarth, new StatsEffect());
            m_effects.Add(EffectEnum.SubReduceDamageAir, new StatsEffect());
            m_effects.Add(EffectEnum.SubReduceDamageWater, new StatsEffect());
            m_effects.Add(EffectEnum.SubReduceDamageFire, new StatsEffect());
            m_effects.Add(EffectEnum.SubReduceDamageNeutral, new StatsEffect());
            m_effects.Add(EffectEnum.SubReduceDamageEarth, new StatsEffect());

            m_effects.Add(EffectEnum.AddReduceDamagePercentAir, new StatsEffect());
            m_effects.Add(EffectEnum.AddReduceDamagePercentWater, new StatsEffect());
            m_effects.Add(EffectEnum.AddReduceDamagePercentFire, new StatsEffect());
            m_effects.Add(EffectEnum.AddReduceDamagePercentNeutral, new StatsEffect());
            m_effects.Add(EffectEnum.AddReduceDamagePercentEarth, new StatsEffect());
            m_effects.Add(EffectEnum.SubReduceDamagePercentAir, new StatsEffect());
            m_effects.Add(EffectEnum.SubReduceDamagePercentWater, new StatsEffect());
            m_effects.Add(EffectEnum.SubReduceDamagePercentFire, new StatsEffect());
            m_effects.Add(EffectEnum.SubReduceDamagePercentNeutral, new StatsEffect());
            m_effects.Add(EffectEnum.SubReduceDamagePercentEarth, new StatsEffect());

            // Ajout ou reduction de dommage
            m_effects.Add(EffectEnum.AddDamage, new StatsEffect());
            m_effects.Add(EffectEnum.AddDamagePhysic, new StatsEffect());
            m_effects.Add(EffectEnum.AddDamageMagic, new StatsEffect());
            m_effects.Add(EffectEnum.AddEchecCritic, new StatsEffect());
            m_effects.Add(EffectEnum.AddDamageCritic, new StatsEffect());

            m_effects.Add(EffectEnum.AddDamagePercent, new StatsEffect());
            m_effects.Add(EffectEnum.SubDamagePercent, new StatsEffect());
            m_effects.Add(EffectEnum.SubDamage, new StatsEffect());
            m_effects.Add(EffectEnum.SubDamageCritic, new StatsEffect());
            m_effects.Add(EffectEnum.SubDamageMagic, new StatsEffect());
            m_effects.Add(EffectEnum.SubDamagePhysic, new StatsEffect());

            m_effects.Add(EffectEnum.AddReflectDamage, new StatsEffect());
            m_effects.Add(EffectEnum.AddReflectDamageItem, new StatsEffect());

            // Chatiment sacris
            m_effects.Add(EffectEnum.AddChatiment, new PunishmentEffect());

            // Effet de push back/fear
            m_effects.Add(EffectEnum.PushBack, new PushEffect());
            m_effects.Add(EffectEnum.PushFront, new PushEffect());
            m_effects.Add(EffectEnum.PushFear, new PushFearEffect());

            // Ajout d'un etat / changement de skin
            m_effects.Add(EffectEnum.ChangeSkin, new SkinChangeEffect());
            m_effects.Add(EffectEnum.AddState, new StateAddEffect());
            m_effects.Add(EffectEnum.RemoveState, new StateRemoveEffect());
            m_effects.Add(EffectEnum.Stealth, new StateAddEffect());

            // Steal de statistique
            m_effects.Add(EffectEnum.StrengthSteal, new StatsStealEffect());
            m_effects.Add(EffectEnum.WisdomSteal, new StatsStealEffect());
            m_effects.Add(EffectEnum.IntelligenceSteal, new StatsStealEffect());
            m_effects.Add(EffectEnum.AgilitySteal, new StatsStealEffect());
            m_effects.Add(EffectEnum.ChanceSteal, new StatsStealEffect());
            m_effects.Add(EffectEnum.APSteal, new StatsStealEffect());
            m_effects.Add(EffectEnum.MPSteal, new StatsStealEffect());
            m_effects.Add(EffectEnum.POSteal, new StatsStealEffect());

            // Autres
            m_effects.Add(EffectEnum.EcaflipChance, new EcaflipChanceEffect());
            m_effects.Add(EffectEnum.Perception, new PerceptionEffect());

            // Sacrifice
            m_effects.Add(EffectEnum.Sacrifice, new SacrificeEffect());
            m_effects.Add(EffectEnum.Transpose, new TransposeEffect());

            // Derobade
            m_effects.Add(EffectEnum.Evasion, new DamageDodgeEffect());
            
            // Augmente de X les domamges de base du sort Y
            m_effects.Add(EffectEnum.IncreaseSpellDamage, new IncreaseSpellJetEffect());

            // Invocation
            m_effects.Add(EffectEnum.Invocation, new SummoningEffect());
            m_effects.Add(EffectEnum.InvocationStatic, new SummoningEffect(true));

            // Debuff
            m_effects.Add(EffectEnum.DeleteAllBonus, new BuffRemoveEffect());

            // Panda
            m_effects.Add(EffectEnum.PandaCarrier, new PandaCarrierEffect());
            m_effects.Add(EffectEnum.PandaLaunch, new PandaLaunchEffect());

            // ActivableObjects
            m_effects.Add(EffectEnum.UseGlyph, new ActivableObjectEffect());
            m_effects.Add(EffectEnum.UseTrap, new ActivableObjectEffect());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public FightActionResultEnum TryApplyEffect(CastInfos castInfos)
        {
            if (!m_effects.ContainsKey(castInfos.EffectType))
            {
                Logger.Debug("EffectManager::TryApplyEffect unknow effect : " + castInfos.EffectType);
                return FightActionResultEnum.RESULT_NOTHING;
            }
            return m_effects[castInfos.EffectType].ApplyEffect(castInfos);
        }
    }
}
