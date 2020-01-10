using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Models
{
    [Table("Produto")]
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(100)]
        public string Descricao { get; set; }

        [Column(TypeName = "decimal(9, 2)")]
        public Decimal? ValorPago { get; set; }

        [Column(TypeName = "decimal(9, 2)")]
        public Decimal ValorVenda { get; set; }

        public int Quantidade { get; set; }

        public DateTime? DataCompra { get; set; }

        public int Ativo { get; set; }

        public DateTime CriadoEm { get; set; }

        public DateTime EditadoEm { get; set; }

        [MaxLength(50)]
        public string Usuario { get; set; }
    }
}
