using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimeStudio.GUI.Projects
{
    public class TreeNodeJSON
    {
        TreeNodeDTO Convert(TreeNode node) => new TreeNodeDTO
        {
            Text = node.Text,
            Children = node.Nodes.Cast<TreeNode>().Select(Convert).ToList()
        };

        public List<TreeNodeDTO> ConvertAll(List<TreeNode> nodes) =>
            nodes.Select(Convert).ToList();
    }

    public class TreeNodeDTO
    {
        public string Text { get; set; }
        public List<TreeNodeDTO> Children { get; set; }
    }
}
