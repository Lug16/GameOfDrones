namespace GameOfDrones.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Round")]
    public partial class Round
    {
        [Key]
        public int IdRound { get; set; }

        public int IdPlayer { get; set; }

        public int IdGame { get; set; }

        public int Score { get; set; }

        public virtual Game Game { get; set; }

        public virtual Player Player { get; set; }
    }
}
