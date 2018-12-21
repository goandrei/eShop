using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models.Data;

namespace WebApplication1.Models.ViewModels.Categories
{
    public class CategoryVM
    {

        public CategoryVM()
        {

        }

        public CategoryVM(CategoriesDTO cat)
        {
            Id = cat.Id;
            Name = cat.Name;
            Description = cat.Description;
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}