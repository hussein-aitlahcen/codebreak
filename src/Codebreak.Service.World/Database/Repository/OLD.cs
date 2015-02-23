using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Database.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MonstersRepository : Repository<MonstersRepository, monsters>
    {
        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void DeleteAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void InsertAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public void Save()
        {
            foreach(var monster in m_dataObjects)
            {
                if (monster.Stats.Contains('|'))
                {
                    var stats = monster.Stats.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    var inits = monster.Inits.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    var grades = monster.Grades.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < Math.Min(Math.Min(grades.Length, stats.Length), inits.Length); i++)
                    {
                        ProcessGrade(monster.Id, i + 1, stats[i], inits[i], grades[i]);
                    }
                }
                else
                {
                    ProcessGrade(monster.Id, 1, monster.Stats, monster.Inits, monster.Grades);
                }
            }
        }

        private void ProcessGrade(int monsterId, int gradeId, string stats, string init, string resistances)
        {
            var monster = MonsterRepository.Instance.GetById(monsterId);
            if (monster != null)
            {
                var grade = monster.GetGrade(gradeId);
                if (grade != null)
                {
                    //22;68;15;-29;25;50;50
                    //400,400,800,400,400
                    var statsData = stats.Split(',');
                    var wisdom = int.Parse(statsData[0]);
                    var strenght = int.Parse(statsData[1]);
                    var intell = int.Parse(statsData[2]);
                    var chance = int.Parse(statsData[3]);
                    var agility = int.Parse(statsData[4]);

                    var initiative = int.Parse(init);

                    var resData = resistances.Split('@')[1].Split(';');
                    var neutralResistance = int.Parse(resData[0]);
                    var earthResistance = int.Parse(resData[1]);
                    var fireResistance = int.Parse(resData[2]);
                    var waterResistance = int.Parse(resData[3]);
                    var airResistance = int.Parse(resData[4]);
                    var apDodge = int.Parse(resData[5]);
                    var mpDodge = int.Parse(resData[6]);

                    grade.Initiative = initiative;
                    grade.Wisdom = wisdom;
                    grade.Strenght = strenght;
                    grade.Intelligence = intell;
                    grade.Chance = chance;
                    grade.Agility = agility;
                    grade.NeutralResistance = neutralResistance;
                    grade.EarthResistance = earthResistance;
                    grade.FireResistance = fireResistance;
                    grade.WaterResistance = waterResistance;
                    grade.AirResistance = airResistance;
                    grade.APDodgePercent = apDodge;
                    grade.MPDodgePercent = mpDodge;

                    grade.Update();
                }
                else
                {
                    Logger.Info("unknow monsterGrade for monsterName=" + monster.Name + " gradeId=" + gradeId);
                }
            }
            else
            {
                Logger.Info("unknow monster monsterId=" + monsterId);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SortsRepository : Repository<SortsRepository, sorts>
    {                
        public override void UpdateAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void DeleteAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }

        public override void InsertAll(MySql.Data.MySqlClient.MySqlConnection connection, MySql.Data.MySqlClient.MySqlTransaction transaction)
        {
        }
    }
}
