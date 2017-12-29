﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public class Grafik
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GrafikID { get; set; }
        public string OsobaID { get; set; }
        public int LiczbaGodzin { get; set; }
        [DisplayFormat(DataFormatString = "{HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime GodzinaRozpoczecia { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        [ForeignKey("OsobaID")]
        public virtual Pracownik Pracownicy { get; set; }
    }
}
