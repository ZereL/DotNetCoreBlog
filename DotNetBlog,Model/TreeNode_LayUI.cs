using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetBlog.Model
{
    public class TreeNode_LayUI
    {
        public int id { set; get; }
        public string name { set; get; }
        public bool spread { set; get; }
        public List<TreeNode_LayUI> children { set; get; }
        public int pid { set; get; }
    }
}
