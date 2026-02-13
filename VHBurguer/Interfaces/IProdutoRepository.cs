using VHBurguer.Domains;

namespace VHBurguer.Interfaces
{
    public interface IProdutoRepository
    {
        List<Produto> Listar();
        Produto ObterPorId(int id);

        Byte[] ObterImagem(int id);
        bool NomeExiste(string nome, int? produtoIdAtual = null);

        void Adicionar(Produto produto, List<int> categoriaId);
        void Atualizar(Produto produto, List<int> categoriaId);
        void Remover(int id);


    }
}
