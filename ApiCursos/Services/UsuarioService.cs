using ApiCursos.Context;
using ApiCursos.DTOs;
using ApiCursos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCursos.Services
{
    public class UsuarioService
    {
        public readonly Db _context;

        public UsuarioService(Db context) 
        {
            _context = context;
        }

        public async Task<List<Usuario>> GetAllUsuarios() 
        {
            return await _context.Usuarios
                .ToListAsync();
        }

        public async Task<string> AddUsuario(UsuarioDTO dto) 
        {
            Usuario usuario = new Usuario();
            usuario.Nome = dto.Nome;
            usuario.Login = dto.Login;
            usuario.Senha = dto.Senha;

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return "Usuario adicionado com sucesso";

        }

        public async Task<string> UpdateUsuario(Guid id, UsuarioDTO dto) 
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id.Equals(id));
            if (usuario == null) return "Usuario não encontrado";

            usuario.Nome = dto.Nome;
            usuario.Login = dto.Login;
            usuario.Senha = dto.Senha;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return "Usuario atualizado com sucesso";
        }

        public async Task<string> DeleteUsuario (Guid id) 
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return "Usuario não encontrado";

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return "Usuario deletado com sucesso";
        }
    }
}
