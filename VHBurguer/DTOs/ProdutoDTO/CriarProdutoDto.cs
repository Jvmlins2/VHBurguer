namespace VHBurguer.DTOs.ProdutoDTO
{
    public class CriarProdutoDto
    {
        public string Nome { get; set; } = null!;
        public decimal Preco { get; set; }
        public string Descricao { get; set; } = null!;

        public IFormFile Imagem { get; set; } = null;

        public List<int> CategoriaIds { get; set; } = new();
    }
}
