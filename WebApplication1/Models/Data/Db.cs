using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Data
{
    public class Db : DbContext
    {
        public DbSet<CategoriesDTO> Categories { get; set; }
        public DbSet<ItemsDTO> Items { get; set; }
        public DbSet<RequestsDTO> Requests { get; set; }
    }
}