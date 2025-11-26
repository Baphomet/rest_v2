using ApiCursos.Context;
using ApiCursos.DTOs;
using ApiCursos.Helpers;
using ApiCursos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCursos.Services
{
    public class UsuarioService
    {
        private readonly Db _context;

        public UsuarioService(Db context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> GetAllUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task AddUsuario(UsuarioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome) ||
                string.IsNullOrWhiteSpace(dto.Login) ||
                string.IsNullOrWhiteSpace(dto.Senha))
            {
                throw new ArgumentException("Todos os campos são obrigatórios.");
            }

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Login = dto.Login,
                Senha = PasswordHelper.Hash(dto.Senha)   
            };

            try
            {
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Erro ao inserir usuário no banco de dados.");
            }
        }

        public async Task UpdateUsuario(Guid id, UsuarioDTO dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            if (string.IsNullOrWhiteSpace(dto.Nome) ||
                string.IsNullOrWhiteSpace(dto.Login) ||
                string.IsNullOrWhiteSpace(dto.Senha))
            {
                throw new ArgumentException("Todos os campos são obrigatórios.");
            }

            usuario.Nome = dto.Nome;
            usuario.Login = dto.Login;
            usuario.Senha = PasswordHelper.Hash(dto.Senha);   

            try
            {
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Erro ao atualizar usuário no banco de dados.");
            }
        }

        public async Task DeleteUsuario(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Erro de integridade ao deletar usuário.");
            }
        }
    }
}
