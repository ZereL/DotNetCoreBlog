using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetBlog.Model
{
    /// <summary>
    /// Admin Table
    /// </summary>
    public class Admin
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate { set; get; }
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// Password 
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { set; get; }
    }
}
