using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenColectaAPI.Models
{
    [Table("materias")]
    public class Materia
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome_materia")]
        [StringLength(60)]
        public string? NomeMateria { get; set; }

        [Column("professor")]
        [StringLength(100)]
        public string? Professor { get; set; }
    }
}
