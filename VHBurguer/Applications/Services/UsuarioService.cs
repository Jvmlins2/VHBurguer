using System.Security.Cryptography;
using System.Text;
using VHBurguer.Domains;
using VHBurguer.DTOs;
using VHBurguer.Exceptions;
using VHBurguer.Interfaces;

namespace VHBurguer.Applications.Services
{
    // service concentra o "como fazer"
    public class UsuarioService
    {
        // _repository é o canal para acessar os dados.
        private readonly IUsuarioRepository _repository;

        // injeção de dependencias
        // implementamos o repositório e o service só depende da interface
        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        // Por que private?
        // pq o método não é regra de negócio e não faz sentido existir fora do UsuarioService
        private static LerUsuarioDto LerDto(Usuario usuario) // pega a entidade usuario e gera um DTO
        {
            LerUsuarioDto lerUsuario = new LerUsuarioDto
            {
                UsuarioID = usuario.UsuarioID,
                Nome = usuario.Nome,
                Email = usuario.Email,
                StatusUsuario = usuario.StatusUsuario ?? true // se não tiver status no banco, deixa como true
            };

            return lerUsuario;
        }

        public List<LerUsuarioDto> Listar()
        {
            List<Usuario> usuarios = _repository.Listar();

            List<LerUsuarioDto> usuariosDto = usuarios
                .Select(usuarioBanco => LerDto(usuarioBanco)) // SELECT que percorre cada Usuario e LerDto(usuario)
                .ToList(); // ToList() -> devolve uma lista de DTOs

            return usuariosDto;
        }

        private static void ValidarEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new DomainException("Email inválido");
            }
        }

        private static byte[] HashSenha(string senha)
        {
            if(string.IsNullOrWhiteSpace(senha))
            {
                throw new DomainException("Senha é obrigatória");
            }

            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));

        }

        public LerUsuarioDto ObterPorID(int id)
        {
            Usuario?usuario = _repository.ObterPorID(id);
            if(usuario == null)
            {
                throw new DomainException("Usuario não existe.");
            }

            return LerDto(usuario);
        }

        public LerUsuarioDto ObterPorEmail(string email)
        {
            Usuario?usuario = _repository.ObterPorEmail(email);
            if (usuario == null)
            {
                throw new DomainException("Usuario não existe.");
            }

            return LerDto(usuario);
        }

        public LerUsuarioDto Adicionar(CriarUsuarioDto usuarioDto)
        {
            ValidarEmail(usuarioDto.Email);
            if (_repository.EmailExiste(usuarioDto.Email))
            {
                throw new DomainException("Já existe um usuário com esse email.");
            }

            Usuario usuario = new Usuario
            {
              Nome = usuarioDto.Nome,
              Email = usuarioDto.Email,
              Senha = HashSenha(usuarioDto.Senha),
              StatusUsuario = true
            };

            _repository.Adicionar(usuario);

            return LerDto(usuario);
        }

        public LerUsuarioDto Atualizar(int id, CriarUsuarioDto usuarioDto)
        {
            ValidarEmail(usuarioDto.Email);

            Usuario usuarioBanco = _repository.ObterPorID(id);

            if(usuarioBanco == null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            ValidarEmail(usuarioDto.Email);

            Usuario usuarioComMesmoEmail = _repository.ObterPorEmail(usuarioDto.Email);

            if(usuarioComMesmoEmail != null && usuarioComMesmoEmail.UsuarioID != id)
            {
                throw new DomainException("Já existe um usuário com este e-mail.");
            }

            usuarioBanco.Nome = usuarioDto.Nome;
            usuarioBanco.Email = usuarioDto.Email;
            usuarioBanco.Senha = HashSenha(usuarioDto.Senha);

            _repository.Atualizar(usuarioBanco);

            return LerDto(usuarioBanco);
        }

        public void Remover(int id)
        {
            Usuario usuario = _repository.ObterPorID(id);

            if(usuario == null)
            {
                throw new DomainException("Usuário não encontrado");
            }

            _repository.Remover(id);
        }
    }
}
