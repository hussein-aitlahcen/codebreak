using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.ActionEffect
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StartFightEffect : AbstractActionEffect<StartFightEffect>
    {
        /// <summary>
        /// SHOULD NEVER BE CALLED EXCEPT IF WE CREATE A NEW ITEM WITH THIS EFFECT
        /// </summary>
        /// <param name="character"></param>
        /// <param name="item"></param>
        /// <param name="effect"></param>
        /// <param name="targetId"></param>
        /// <param name="targetCell"></param>
        /// <returns></returns>
        public override bool ProcessItem(Entity.CharacterEntity character, Database.Structure.ItemDAO item, Stats.GenericEffect effect, long targetId, int targetCell)
        {
            var monster = MonsterRepository.Instance.GetById(effect.Value2);
            if(monster == null)
                return false;

            if(!monster.Grades.Any())
                return false;

            var grade = monster.Grades.ElementAt(Util.Next(0, monster.Grades.Count()));

            return Process(character, new Dictionary<string, string> { { "gradeId", grade.Id.ToString() } });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool Process(Entity.CharacterEntity character, Dictionary<string, string> parameters)
        {
            var gradeId = int.Parse(parameters["gradeId"]);
            var grade = MonsterGradeRepository.Instance.GetById(gradeId);
            if(grade == null)
            {
                character.Dispatch(WorldMessage.SERVER_INFO_MESSAGE("Unknow monster grade, cannot start the fight."));
                return false;
            }
            
            return character.Map.StartMonsterFight(character, new List<MonsterGradeDAO>() { grade });
        }
    }
}
