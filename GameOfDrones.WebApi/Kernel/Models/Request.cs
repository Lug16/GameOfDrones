using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameOfDrones.WebApi.Kernel.Models
{
    public class Request
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Handshape { get; set; }

        public int Turn { get; set; }

        public int? IdWinner { get; set; }
    }
}