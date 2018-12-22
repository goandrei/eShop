using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Data
{
    [Table("Requests")]
    public class RequestsDTO
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
    }
}