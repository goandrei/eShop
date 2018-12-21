using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.Data;

namespace WebApplication1.Models.ViewModels.Items
{
    public class ItemsVM
    {

        public ItemsVM()
        {

        }

        public ItemsVM(ItemsDTO item)
        {
            Id = item.Id;
            Title = item.Title;
            Description = item.Description;
            Price = item.Price;
            CategoryId = item.CategoryId;
            CategoryName = item.CategoryName;
            Score = item.Score;
            Status = false;
            Image = item.Image;
        }

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Score { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}