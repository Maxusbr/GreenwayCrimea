using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Tools.fm.DynamicTreeControl
{
    public partial class DynamicTreeControl : System.Web.UI.UserControl
    {
        private TreeNodeCollection _nodeCollection;
        public TreeNodeCollection Nodes
        {
            get
            {
                return _nodeCollection;
            }
            set
            {
                _nodeCollection = value;
            }
        }

        public string SelectedNode
        {
            get
            {
                return SelectedNodeInput.Value;
            }
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            HtmlGenericControl rootList = new HtmlGenericControl("ul");
            container.Controls.Add(rootList);

            foreach (TreeNode node in _nodeCollection)
            {
                ProcessNode(node, rootList);
            }

        }

        private void ProcessNode(TreeNode node, HtmlGenericControl list)
        {
            HtmlGenericControl listItem = new HtmlGenericControl("li");
            list.Controls.Add(listItem);

            if (node.ChildNodes.Count == 0)
            {
                listItem.Attributes.Add("class", "AspNet-TreeView-Leaf");
                HtmlAnchor link = new HtmlAnchor();
                link.InnerText = node.Text;
                link.Attributes.Add("onclick", "javascript: SelectListItem(this, \'" + GetNodePath(node) + "\');");
                listItem.Controls.Add(link);
            }
            else
            {
                listItem.Attributes.Add("class", "AspNet-TreeView-Parent-Closed");

                HtmlGenericControl button = new HtmlGenericControl("span");
                button.Attributes.Add("class", "Net-TreeView-Expand");
                button.Attributes.Add("onclick", "javascript: Expand(this)");
                button.InnerHtml = "&nbsp;";
                listItem.Controls.Add(button);

                HtmlAnchor link = new HtmlAnchor();
                link.InnerText = node.Text;
                link.Attributes.Add("onclick", "javascript: SelectListItem(this, \'" + GetNodePath(node) + "\');");
                listItem.Controls.Add(link);

                HtmlGenericControl subList = new HtmlGenericControl("ul");
                listItem.Controls.Add(subList);

                foreach (TreeNode subnode in node.ChildNodes)
                {
                    ProcessNode(subnode, subList);
                }
            }

        }

        private static string GetNodePath(TreeNode node)
        {
            string result = node.Value;
            if (node.Parent != null)
            {
                result = GetNodePath(node.Parent) + "\\\\" + result;
            }
            return result;
        }
    }
}