using System;

namespace App.Data.Dto
{
    public class ProdutoDto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public Decimal? ValorPago { get; set; }

        public Decimal ValorVenda { get; set; }

        public int Quantidade { get; set; }

        public DateTime? DataCompra { get; set; }

        public int Ativo { get; set; }

        public DateTime CriadoEm { get; set; }

        public DateTime EditadoEm { get; set; }

        public string Usuario { get; set; }
    }
}
