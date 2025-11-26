using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiCursos.Context;
using ApiCursos.DTOs;
using ApiCursos.Helpers;
using ApiCursos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<string> LoginAsync(LoginDTO dto, IConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(dto.Login) ||
                string.IsNullOrWhiteSpace(dto.Senha))
            {
                throw new ArgumentException("Login e senha são obrigatórios.");
            }

            var senhaHash = PasswordHelper.Hash(dto.Senha);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Login == dto.Login && u.Senha == senhaHash);

            if (usuario == null)
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

            var key = Encoding.ASCII.GetBytes(config["Jwt:Key"]);

            var claims = new[]
            {
        new Claim("id", usuario.Id.ToString()),
        new Claim(ClaimTypes.Name, usuario.Nome),
        new Claim("login", usuario.Login)
        };

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
