using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetBlog.Model
{
    /// <summary>
    /// Category table
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate { set; get; }
        /// <summary>
        /// CaName
        /// </summary>
        public string CaName { set; get; }
        /// <summary>
        /// Bh 
        /// </summary>
        public string Num { set; get; }
        /// <summary>
        /// ParNum
        /// </summary>
        public string ParNum { set; get; }
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { set; get; }
    }
}
