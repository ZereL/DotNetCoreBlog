using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetBlog.Model
{
    /// <summary>
    /// 博客表
    /// </summary>
    public class Blog
    {
        /// <summary>
        /// id primary key
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate { set; get; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// Body 
        /// </summary>
        public string Body { set; get; }
        /// <summary>
        /// Body-markdown
        /// </summary>
        public string Body_md { set; get; }
        /// <summary>
        /// VisitNum
        /// </summary>
        public int VisitNum { set; get; }
        /// <summary>
        /// Category number
        /// </summary>
        public string CaNum { set; get; }
        /// <summary>
        /// Category name
        /// </summary>
        public string CaName { set; get; }
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// sort
        /// </summary>
        public int Sort { set; get; }
    }
}
