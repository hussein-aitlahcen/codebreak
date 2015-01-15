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
        /// <summary>
        /// 
        /// </summary>
        private BasicTaskProcessor m_processor;

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
            m_processor = new BasicTaskProcessor("MainForm_Action");
            m_processor.Start();

            tabControl1.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chargerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearViews();
            tabControl1.Enabled = true;
            chargerToolStripMenuItem.Enabled = false;
            m_processor.AddMessage(() => LoadDatabase());
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearViews()
        {
            ClearNpcTemplates();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadDatabase()
        {
            Disable();

            WorldDbMgr.Instance.Initialize();

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
                    }));
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

            ClearNpcTemplates();

            var id = (int)numericUpDownSearchNpcTemplateId.Value;
            var name = txtBoxSearchNpcTemplateName.Text.ToLower();

            if (id == 0 && string.IsNullOrWhiteSpace(name))
            {
                LoadNpcTemplates();
            }
            else
            {
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

        }
    }
}
