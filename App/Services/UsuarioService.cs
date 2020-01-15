using App.Common;
using App.Data;
using App.Data.Domain;
using App.Data.Dto;
using App.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace App.Services
{
    public class UsuarioService
    {
        private readonly ApplicationServiceContext _context;
        //private readonly CriptografarMD5 _common;
        public object ModelState { get; private set; }

        public UsuarioService(ApplicationServiceContext context)
        {
            _context = context;
        }

        public async Task<UsuarioDtoPost> CreateNewUser(UsuarioDto dto)
        {
            try
            {
                var verifyExists = await GetUsuarioByEmail(dto.Email);

                if (verifyExists != null)
                {
                    return verifyExists;
                }

                var user = await ReturnClassUsuario(dto);

                _context.Usuarios.Add(user);
                await _context.SaveChangesAsync();

                var result = await ReturnDtoUsuario(user);

                SendEmail(result);

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UsuarioDtoPost> GetUsuarioById(int idUsuario)
        {
            try
            {
                var user = _context.Usuarios
                                .Where(x => x.Id == idUsuario)
                                .Select(x => new UsuarioDtoPost
                                {
                                    Id = x.Id,
                                    Nome = x.Nome,
                                    Email = x.Email,
                                    Perfil = x.Perfil
                                }).FirstOrDefault();

                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UsuarioDtoPost> GetUsuarioByEmail(string email)
        {
            try
            {
                var user = _context.Usuarios
                                .Where(x => x.Email == email)
                                .Select(x => new UsuarioDtoPost
                                {
                                    Id = x.Id,
                                    Nome = x.Nome,
                                    Email = x.Email,
                                    Perfil = x.Perfil
                                }).FirstOrDefault();

                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        
        #region TRANSFER DTO IN CLASS
        private async Task<Usuario> ReturnClassUsuario(UsuarioDto dto)
        {
            var user = new Usuario()
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

            return user;
        }

        private async Task<UsuarioDtoPost> ReturnDtoUsuario(Usuario dto)
        {
            var user = new UsuarioDtoPost()
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Email = dto.Email,
                Perfil = "user"
            };

            return user;
        }
        #endregion

        #region PRIVATE METHODS
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

        private void SendEmail(UsuarioDtoPost dto)
        {


            string strHtml = "";
            strHtml += "<div style='display: block;margin: 0 auto;width: 620px;'>";
            strHtml += "<div style='height:550px;background-color:#2196F3;border-radius:15px 100px;'>";
            strHtml += "<div style='text-align:center;padding-top:10%;font-size:20px;color:#FFF;'>";
            strHtml += "<p>FATECGRAM:</p>";
            strHtml += "<p>Olá ${params.name}!</p>";
            strHtml += "<p>Precisamos que você confirme seu email para ter acesso a nossa rede.</p>";
            strHtml += "<p>Clique no link a baixo para confirmar</p>";
            strHtml += "<p></p>";
            strHtml += "<p><a href='https://cleberspirlandeli.github.io/portfolio/' target='_blank'  style='text-decoration: none;cursor:pointer;'>CONFIRMAR EMAIL</a></p>";
            strHtml += "<p></p>";
            strHtml += "<p>Obs: Se não receber o email, verifique a caixa de lixo eletrônico.</p>";
            strHtml += "<p style='font-size:12px;color:#000;bottom:0px;margin-top:170px'>Desenvolvido por Cleber Rezende</p>";
            strHtml += "</div></div></div>";

            //body += "<div>";
            //body += "<td class='esd-stripe' esd-custom-block-id='7388' align='center'>";
            //body += "    <table class='es-content-body' style='background-color: rgb(51, 51, 51);' width='600' cellspacing='0' cellpadding='0' bgcolor='#333333' align='center'>";
            //body += "        <tbody>";
            //body += "            <tr>";
            //body += "                <td class='esd-structure esd-checked es-p40t es-p40b es-p40r es-p40l' style='background-image: url('https://etnfxo.stripocdn.email/content/guids/CABINET_85e4431b39e3c4492fca561009cef9b5/images/93491522393929597.png'); background-repeat: no-repeat;' align='left'>";
            //body += "                    <table width='100%' cellspacing='0' cellpadding='0'>";
            //body += "                        <tbody>";
            //body += "                            <tr>";
            //body += "                                <td class='esd-container-frame' width='520' valign='top' align='center'>";
            //body += "                                    <table width='100%' cellspacing='0' cellpadding='0'>";
            //body += "                                        <tbody>";
            //body += "                                            <tr>";
            //body += "                                                <td align='center' class='esd-block-text es-p40t es-p10b'>";
            //body += "                                                    <h1 style='color: #ffffff;'>BEM VINDO AO TESTE</h1>";
            //body += "                                                </td>";
            //body += "                                            </tr>";
            //body += "                                            <tr>";
            //body += "                                                <td esdev-links-color='#757575' class='esd-block-text es-p10t es-p20b es-p30r es-p30l' align='center'>";
            //body += "                                                    <p style='color: #757575;'>Se hoje é o dia das crianças... Ontem eu disse: o dia da criança é o dia da mãe, dos pais, das professoras, mas também é o dia dos animais, sempre que você olha uma criança, há sempre uma figura oculta, que é um cachorro atrás. O que é algo muito importante! incididunt ut labore.</p>";
            //body += "                                                </td>";
            //body += "                                            </tr>";
            //body += "                                            <tr>";
            //body += "                                                <td class='esd-block-button es-p10t es-p20b' align='center'>";
            //body += "                                                    <span class='es-button-border' style='border-width: 0px; border-style: solid; background: none 0% 0% repeat scroll rgb(38, 164, 211); border-color: rgb(38, 164, 211);'><a href='https://cleberspirlandeli.github.io/portfolio/' class='es-button' target='_blank' style='background: rgb(38, 164, 211) none repeat scroll 0% 0%; border-color: rgb(38, 164, 211);'>VALIDAR EMAIL</a></span></td>";
            //body += "                                            </tr>";
            //body += "                                        </tbody>";
            //body += "                                    </table>";
            //body += "                                </td>";
            //body += "                            </tr>";
            //body += "                        </tbody>";
            //body += "                    </table>";
            //body += "                </td>";
            //body += "            </tr>";
            //body += "        </tbody>";
            //body += "    </table>";
            //body += "</td>";

            //body += "</div>";



            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("xxxxxx@gmail.com"); // De
            mail.To.Add(dto.Email); // Para
            mail.Subject = "Teste API Dotnet"; // Assunto
            mail.Body = strHtml; // Mensagem


            // Anexos
            //mail.Attachments.Add(new Attachment(@"C:\teste.txt"));

            using (var smtp = new SmtpClient("smtp.gmail.com"))
            {
                smtp.EnableSsl = true; // GMail requer SSL
                smtp.Port = 587;       // porta para SSL
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
                smtp.UseDefaultCredentials = false; // vamos utilizar credencias especificas

                // seu usuário e senha para autenticação
                smtp.Credentials = new NetworkCredential("xxxxxx@gmail.com", "xxxxx");

                // envia o e-mail
                smtp.Send(mail);
            }
        }
        #endregion

    }
}
