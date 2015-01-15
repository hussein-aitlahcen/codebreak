using Codebreak.Framework.Configuration;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Codebreak.Tool.Database
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainForm : Form
    {
        [Configurable("DbConnection")]
        public static string DbConnection = "Server=localhost;Database=codebreak_world;Uid=root;Pwd=;";
        
        /// <summary>
        /// 
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            tabControl1.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chargerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.Enabled = true;
            chargerToolStripMenuItem.Enabled = false;

            Program.Processor.AddMessage(() => LoadDatabase());
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadDatabase()
        {
            Disable();

            WorldDbMgr.Instance.Initialize(DbConnection);

            LoadNpcTemplates();

            Enable();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Enable()
        {
            SafeExecute(() => Enabled = true);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Disable()
        {
            SafeExecute(() => Enabled = false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        private void SafeExecute(Action action)
        {
            base.Invoke((Action)action);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearNpcTemplates()
        {
            listViewNpcTemplate.Items.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadNpcTemplates()
        {
            ClearNpcTemplates();
            InitProgressbar(NpcTemplateRepository.Instance.GetAll().Count());
            foreach(var npcTemplate in NpcTemplateRepository.Instance.GetAll())
            {
                AddNpcTemplate(npcTemplate);
                UpdateProgressbar();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="npcTemplate"></param>
        private void AddNpcTemplate(NpcTemplateDAO npcTemplate)
        {
            SafeExecute(() =>
                {
                    listViewNpcTemplate.Items.Add(new ListViewItem(new string[]
                    {
                        npcTemplate.Id.ToString(),
                        npcTemplate.Name,
                        npcTemplate.Sell,
                        npcTemplate.Exchange
                    })
                    {
                        Tag = npcTemplate
                    });
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="max"></param>
        private void InitProgressbar(int max)
        {
            SafeExecute(() =>
                {
                    toolStripProgressBar1.Value = 0;
                    toolStripProgressBar1.Maximum = max;
                });
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateProgressbar()
        {
            SafeExecute(() =>
                {
                    toolStripProgressBar1.Value++;
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchNpcTemplate_Click(object sender, EventArgs e)
        {
            Disable();
            
            var id = (int)numericUpDownSearchNpcTemplateId.Value;
            var name = txtBoxSearchNpcTemplateName.Text.ToLower();

            if (id == 0 && string.IsNullOrWhiteSpace(name))
            {
                LoadNpcTemplates();
            }
            else
            {
                ClearNpcTemplates();
                foreach (var npcTemplate in NpcTemplateRepository.Instance.GetAll())
                {
                    if ((id == 0 ? false : npcTemplate.Id == id) || (string.IsNullOrWhiteSpace(name) ? false : npcTemplate.Name.ToLower().Contains(name)))
                    {
                        AddNpcTemplate(npcTemplate);
                    }
                }
            }

            Enable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editerToolStripMenuItemNpcTemplate_Click(object sender, EventArgs e)
        {
            new NpcTemplateEdition(listViewNpcTemplate.SelectedItems[0].Tag as NpcTemplateDAO).ShowDialog();
            LoadNpcTemplates();
        }
    }
}
