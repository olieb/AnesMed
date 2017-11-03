using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Przuchodnia_Medyczna_Inz.Models
{
    public enum StanowiskoNazwa
    {
        Administrator,
        Lekarz,
        Pielęgniarka,
        Sanitariusz,
        Recepcja
    };
    public class Stanowisko
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StanowiskoID { get; set; }
        public string PracownikID { get; set; }
        public StanowiskoNazwa Nazwa { get; set; }
        [ForeignKey("PracownikID")]
        public virtual Pracownik Pracownik { get; set; }
        [InverseProperty("Stanowisko")]
        public virtual ICollection<Pracownik> Pracownicy { get; set; }
    }
}
