using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameOfDrones.WebApi.Models
{
    public class Player
    {
        public int Id { get; set; } 
        
        [Required]
        public string Name { get; set; }

        public int Score { get; set; }
    }
}