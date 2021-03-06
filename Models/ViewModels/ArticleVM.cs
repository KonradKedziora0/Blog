using GitBlog.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitBlog.Models.ViewModels
{
    public class ArticleVM
    {
        public Article Article { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
    }
}
