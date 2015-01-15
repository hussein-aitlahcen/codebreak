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
    public partial class NpcTemplateEdition : Form
    {
        /// <summary>
        /// 
        /// </summary>
        private NpcTemplateDAO m_npcTemplate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="npcTemplate"></param>
        public NpcTemplateEdition(NpcTemplateDAO npcTemplate)
        {
            m_npcTemplate = npcTemplate;
            
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NpcTemplateEdition_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            foreach (var item in m_npcTemplate.ShopList)
                AddToSell(item);
            ReloadRewards();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReloadRewards()
        {
            treeView1.Nodes.Clear();
            foreach (var reward in m_npcTemplate.Rewards)
                AddReward(reward);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reward"></param>
        private void AddReward(RewardEntry reward)
        {
            var mainNode = new TreeNode(reward.GetHashCode().ToString())
            {
                Tag = reward
            };

            var requiredNode = new TreeNode("required")
            {
                Tag = reward.RequiredItems
            };

            if(reward.RequiredKamas > 0)
                requiredNode.Nodes.Add(reward.RequiredKamas + " kamas");
            foreach (var item in reward.RequiredItems)
                requiredNode.Nodes.Add(new TreeNode(item.Quantity + "x " + item.Template.Name)
                {
                    Tag = item
                });

            var rewardedNode = new TreeNode("rewarded")
            {
                Tag = reward.RewardedItems
            };

            if (reward.RewardedKamas > 0)
            rewardedNode.Nodes.Add(reward.RewardedKamas + " kamas");
            foreach(var item in reward.RewardedItems)            
                rewardedNode.Nodes.Add(new TreeNode(item.Quantity + "x " + item.Template.Name)
                    {
                        Tag = item
                    });

            mainNode.Nodes.Add(requiredNode);
            mainNode.Nodes.Add(rewardedNode);
            treeView1.Nodes.Add(mainNode);
            treeView1.ExpandAll();
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddToSell(ItemTemplateDAO item)
        {
            listViewSellList.Items.Add(new ListViewItem(new string[] {
                item.Id.ToString(),
                item.Name
            })
            {
                Tag = item
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddToSell_Click(object sender, EventArgs e)
        {
            var itemTemplate = ItemTemplateRepository.Instance.GetById((int)numericUpDown1.Value);
            if (itemTemplate == null)
            {
                MessageBox.Show("Item inéxistant.");
                return;
            }
            else if(m_npcTemplate.ShopList.Any(item => item.Id == itemTemplate.Id))
            {
                MessageBox.Show("Item déjà en vente.");
                return;
            }

            AddToSell(itemTemplate);
            m_npcTemplate.ShopList.Add(itemTemplate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveSellList_Click(object sender, EventArgs e)
        {
            m_npcTemplate.Sell = string.Join(",", m_npcTemplate.ShopList.Select(item => item.Id));
            if(!m_npcTemplate.Save())
            {
                MessageBox.Show("Impossible de sauvegarder.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
             var itemTemplate = ItemTemplateRepository.Instance.GetById((int)numericUpDown1.Value);
            if (itemTemplate == null)
            {
                toolStripStatusLabelCurrentItemName.Text = "Item inéxistant";
                toolStripStatusLabelCurrentItemName.ForeColor = Color.Red;
            }
            else
            {
                toolStripStatusLabelCurrentItemName.Text = itemTemplate.Name;
                toolStripStatusLabelCurrentItemName.ForeColor = Color.DarkGreen;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewSellList_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                foreach(ListViewItem item in listViewSellList.SelectedItems)
                {
                    var template = item.Tag as ItemTemplateDAO;
                    m_npcTemplate.ShopList.Remove(template);
                    listViewSellList.Items.Remove(item);
                }
            }
        }

     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedText == "item")
            {
                var itemTemplate = ItemTemplateRepository.Instance.GetById((int)numericUpDown2.Value);
                if (itemTemplate == null)
                {
                    toolStripStatusLabelCurrentItemName.Text = "Item inéxistant";
                    toolStripStatusLabelCurrentItemName.ForeColor = Color.Red;
                }
                else
                {
                    toolStripStatusLabelCurrentItemName.Text = itemTemplate.Name;
                    toolStripStatusLabelCurrentItemName.ForeColor = Color.DarkGreen;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsertInCurrent_Click(object sender, EventArgs e)
        {            
            var node = treeView1.SelectedNode;
            switch(comboBox1.SelectedItem.ToString())
            {
                case "kamas":
                    if (node == null || (node.Text != "rewarded" && node.Text != "required"))
                    {
                        MessageBox.Show("Selectionnez un noeud 'required' ou 'rewarded' pour ajouter des kamas.");
                        return;
                    }

                    var reward = node.Parent.Tag as RewardEntry;
                    if(node.Text == "rewarded")
                    {
                        reward.RewardedKamas = (long)numericUpDown3.Value;
                    }
                    else if (node.Text == "required")
                    {                        
                        reward.RequiredKamas = (long)numericUpDown3.Value;
                    }                
                    break;


                case "item":
                    if (node == null || node.Tag == null || (node.Tag as List<RewardEntry.ItemEntry>) == null)
                    {
                        MessageBox.Show("Selectionnez un noeud 'required' ou 'rewarded' pour ajouter un item.");
                        return;
                    }

                    var itemTemplate = ItemTemplateRepository.Instance.GetById((int)numericUpDown2.Value);
                    if (itemTemplate == null)
                    {
                        MessageBox.Show("Item inéxistant.");
                        return;
                    }

                    var list = node.Tag as List<RewardEntry.ItemEntry>;
                    reward = node.Parent.Tag as RewardEntry;
                    if(list.Any(item => item.TemplateId == itemTemplate.Id))
                    {
                        MessageBox.Show("Item déjà présent.");
                        return;
                    }

                    list.Add(new RewardEntry.ItemEntry(itemTemplate.Id, (long)numericUpDown3.Value));
                    break;
                    
                case "entry":
                    m_npcTemplate.Rewards.Add(new RewardEntry());
                    break;
            }

            ReloadRewards();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveRewards_Click(object sender, EventArgs e)
        {
            m_npcTemplate.Exchange = string.Join("|", m_npcTemplate.Rewards.Select(reward => reward.Serialize()));
            if (!m_npcTemplate.Save())
            {
                MessageBox.Show("Impossible de sauvegarder.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NpcTemplateEdition_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnSaveSellList_Click(null, null);
            btnSaveRewards_Click(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var node = treeView1.SelectedNode;
                var rewardEntry = node.Tag as RewardEntry;
                if (rewardEntry != null)
                {
                    m_npcTemplate.Rewards.Remove(rewardEntry);
                }
                else
                {
                    rewardEntry = node.Parent.Tag as RewardEntry;
                    if (node.Text == "required")
                    {
                        (node.Tag as List<RewardEntry.ItemEntry>).Clear();
                        rewardEntry.RequiredKamas = 0;
                    }
                    else if (node.Text == "rewarded")
                    {
                        (node.Tag as List<RewardEntry.ItemEntry>).Clear();
                        rewardEntry.RewardedKamas = 0;
                    }
                    else if((node.Tag as RewardEntry.ItemEntry) != null)
                    {
                        (node.Parent.Tag as List<RewardEntry.ItemEntry>).Remove(node.Tag as RewardEntry.ItemEntry);
                    }
                }              

                ReloadRewards();
            }
        }
    }
}
