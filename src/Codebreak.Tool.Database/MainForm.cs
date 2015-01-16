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
            SafeExecute(() =>
            {
                ColumnNpcInstanceId.ValueType = typeof(int);

                ColumnNpcInstanceCellId.ValueType = typeof(int);

                ColumnNpcInstanceOrientation.ValueType = typeof(int);

                ColumnNpcInstanceMapId.ValueType = typeof(MapTemplateDAO);
                ColumnNpcInstanceMapId.ValueMember = "This";
                ColumnNpcInstanceMapId.DisplayMember = "DisplayMember";
                ColumnNpcInstanceMapId.DataSource = MapRepository.Instance.GetAll().OrderBy(map => map.Id).ToList();

                ColumnNpcInstanceTemplateId.ValueType = typeof(NpcTemplateDAO);
                ColumnNpcInstanceTemplateId.ValueMember = "This";
                ColumnNpcInstanceTemplateId.DisplayMember = "DisplayMember";
                ColumnNpcInstanceTemplateId.DataSource = NpcTemplateRepository.Instance.GetAll().OrderBy(template => template.Name).ToList();

                ColumnNpcInstanceQuestionId.ValueType = typeof(NpcQuestionDAO);
                ColumnNpcInstanceQuestionId.ValueMember = "This";
                ColumnNpcInstanceQuestionId.DisplayMember = "DisplayMember";
                ColumnNpcInstanceQuestionId.DataSource = NpcQuestionRepository.Instance.GetAll();

                foreach(var npcInstance in NpcInstanceRepository.Instance.GetAll())
                {
                    var row = new DataGridViewRow()
                    {
                        Tag = npcInstance
                    };
                    row.CreateCells(dataGridViewNpcInstance,
                        npcInstance.Id,
                        npcInstance.Map,
                        npcInstance.Template,
                        npcInstance.CellId,
                        npcInstance.Orientation,
                        npcInstance.Question);
                    dataGridViewNpcInstance.Rows.Add(row);
                }
            });

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
            SafeExecute(() => { toolStripStatusLabelState.Text = "Chargement NpcTemplate ..."; toolStripStatusLabelState.ForeColor = Color.Blue; });

            ClearNpcTemplates();
            InitProgressbar(NpcTemplateRepository.Instance.GetAll().Count());
            foreach(var npcTemplate in NpcTemplateRepository.Instance.GetAll())
            {
                AddNpcTemplate(npcTemplate);
                UpdateProgressbar();
            }

            SafeExecute(() => { toolStripStatusLabelState.Text = "Chargé"; toolStripStatusLabelState.ForeColor = Color.DarkGreen; });
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewNpcInstance_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewNpcInstance_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewNpcInstance_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var row = dataGridViewNpcInstance.Rows[e.RowIndex];
                var column = row.Cells[e.ColumnIndex];
                var value = column.Value;
                var npcInstance = row.Tag as NpcInstanceDAO;

                switch(column.OwningColumn.HeaderText)
                {
                    case "Id":
                        break;

                    case "MapId":
                        var map = value as MapTemplateDAO;
                        npcInstance.MapId = map.Id;
                        break;

                    case "Template":
                        var template = value as NpcTemplateDAO;
                        npcInstance.TemplateId = template.Id;
                        break;

                    case "CellId":
                        npcInstance.CellId = (int)value;
                        break;

                    case "Ortientation":
                        npcInstance.Orientation = (int)value;
                        break;

                    case "Question":
                        var question = value as NpcQuestionDAO;
                        npcInstance.QuestionId = question.Id;
                        break;
                }

                if(!npcInstance.Update())
                {
                    MessageBox.Show("Impossible de sauvegarder");
                }
            }
        }
    }
}
