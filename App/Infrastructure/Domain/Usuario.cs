using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(100)]
        public string Sobrenome { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(150)]
        public string Senha { get; set; }

        [Required]
        [StringLength(30)]
        public string Perfil { get; set; }

        public int? Ativo { get; set; }

        public int? EmailAtivo { get; set; }

        [Required]
        [StringLength(200)]
        public string CodigoGuid { get; set; }
    }
}
