using GitBlog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GitBlog.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This field cannot be empty")]
        [MinLength(10, ErrorMessage = "Title must be longer than 10 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field cannot be empty")]
        [Display(Name = "Subtitle")]
        [MaxLength(300, ErrorMessage = "Subtitle cannot be longer than 10 characters")]
        [MinLength(10, ErrorMessage = "Subtitle must be longer than 10 characters")]
        public string ShortDesccription { get; set; }

        [Required(ErrorMessage = "This field cannot be empty")]
        [Display(Name = "Content")]
        [MinLength(20, ErrorMessage = "Article must be longer than 20 characters")]
        public string ContentOfArticle { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "This field cannot be empty")]
        public string Author { get; set; }

        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public DateTime Created { get; set; }
    }
}
