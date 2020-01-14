using App.Common;
using App.Data;
using App.Data.Domain;
using App.Data.Dto;
using App.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Services
{
    public class UsuarioService
    {
        private readonly ApplicationServiceContext _context;
        private readonly CriptografarMD5 _common;

        public UsuarioService(ApplicationServiceContext context)
        {
            _context = context;
        }

        public object ModelState { get; private set; }

        public async Task<Usuario> Insert(UsuarioDto dto)
        {
            try
            {

                var usuario = await Create(dto);

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return usuario;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        //private string Criptografar(string password)
        //{
        //    password = "|240&c0-bb@43-6e3$81=" + password + "=MaP_Ew#Td@x3";

        //    System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        //    byte[] data = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(password));
        //    System.Text.StringBuilder sbString = new System.Text.StringBuilder();
        //    for (int i = 0; i < data.Length; i++)
        //        sbString.Append(data[i].ToString("x2"));
        //    return sbString.ToString();
        //}


        private async Task<Usuario> Create(UsuarioDto dto)
        {

            var usuario = new Usuario()
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Sobrenome = dto.Sobrenome,
                Email = dto.Email,
                Senha = Criptografar(dto.Senha),
                Perfil = "user",
                Ativo = 1,
                EmailAtivo = 0
            };

            return usuario;
        }

        private string Criptografar(string password)
        {
            if (password == null || !password.Any())
                return null;


            password = "|240&c0-bb@43-6e3$81=" + password + "=MaP_Ew#Td@x3";

            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(password));
            System.Text.StringBuilder sbString = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sbString.Append(data[i].ToString("x2"));
            return sbString.ToString();
        }

    }
}
