using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitBlog.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
