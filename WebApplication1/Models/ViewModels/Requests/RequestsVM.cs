using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models.Data;

namespace WebApplication1.Models.ViewModels.Requests
{
    public class RequestsVM
    {
        public RequestsVM()
        {

        }

        public RequestsVM(RequestsDTO req)
        {
            Id = req.Id;
            UserId = req.UserId;
            ItemId = req.ItemId;
        }

        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ItemId { get; set; }
    }
}