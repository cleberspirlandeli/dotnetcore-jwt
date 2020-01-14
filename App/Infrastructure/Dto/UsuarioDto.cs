namespace App.Infrastructure.Dto
{
    public class UsuarioDto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string Perfil { get; set; }

        public int? Ativo { get; set; }

        public int? EmailAtivo { get; set; }
    }
}
