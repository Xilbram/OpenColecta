using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenColectaAPI.Models
{
    [Table("review")]
    public class Review
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }  // FK + PK referencing materia(id)

        [Column("laboriedade")]
        public int? Laboriedade { get; set; }

        [Column("dificuldade")]
        public int? Dificuldade { get; set; }

        [Column("didatica")]
        public int? Didatica { get; set; }

        [Column("interesse_pessoal")]
        public int? InteressePessoal { get; set; }

        [Column("respondente")]
        [StringLength(200)]
        public string? Respondente { get; set; }

        // Navigation property for Materia
        public Materia? Materia { get; set; }
    }
}