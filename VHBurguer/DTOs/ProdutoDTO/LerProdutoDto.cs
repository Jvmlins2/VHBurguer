using VHBurguer.Domains;

namespace VHBurguer.DTOs.ProdutoDTO
{
    public class LerProdutoDto
    {
        public int ProdutoID { get; set; }

        public string Nome { get; set; } = null!;

        public decimal Preco { get; set; }

        public string Descricao { get; set; } = null!;

        public byte[] Imagem { get; set; } = null!;

        public bool? StatusProduto { get; set; }

        public List<int> CategoriaIds { get; set; } = new();

        public List<string> Categorias { get; set; } = new();

        public int? UsuarioID { get; set; }

        public string UsuarioNome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public byte[] Senha { get; set; } = null!;

        public bool? StatusUsuario { get; set; }

        public virtual ICollection<Produto> Produto { get; set; } = new List<Produto>();

    }
}
