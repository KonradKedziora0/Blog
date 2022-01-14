using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitBlog.Models.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Article = new Article();
    }

        public Article Article { get; set; }
    }

}
