﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Data
{
    [Table("Items")]
    public class ItemsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Score { get; set; }
        public string Image { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoriesDTO cat { get; set; }
    }
}