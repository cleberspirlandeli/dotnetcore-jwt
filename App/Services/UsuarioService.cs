using App.Data;
using App.Data.Domain;
using App.Infrastructure.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace App.Services
{
    public class UsuarioService
    {
        private readonly ApplicationServiceContext _context;
        public object ModelState { get; private set; }
        public IConfiguration _configuration { get; }


        public UsuarioService(ApplicationServiceContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        public async void ConfirmEmail(string codigo)
        {
            try
            {
                var user = _context.Usuarios
                                .Where(x => x.CodigoGuid == codigo)
                                .FirstOrDefault();

                if(user == null)
                    throw new Exception("Os dados informados não estão cadastrados no sistema.");

                if (user.Ativo == 0)
                    throw new Exception("Usuário bloqueado.");

                if(user.EmailAtivo == 0)
                {
                    _context.Entry(user).State = EntityState.Modified;
                    user.EmailAtivo = 1;
                    _context.SaveChanges();
                }

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
                EmailAtivo = 0,
                CodigoGuid = Guid.NewGuid().ToString()
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
                Perfil = "user",
                Codigo = dto.CodigoGuid
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

            #region STRING HTML

            string strHtml = @"
<!doctype html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"">
  <head>
    <title>
    </title>
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <style type=""text/css"">
      #outlook a{padding: 0;}
      			.ReadMsgBody{width: 100%;}
      			.ExternalClass{width: 100%;}
      			.ExternalClass *{line-height: 100%;}
      			body{margin: 0; padding: 0; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%;}
      			table, td{border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt;}
      			img{border: 0; height: auto; line-height: 100%; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic;}
      			p{display: block; margin: 13px 0;}
    </style>
    <!--[if !mso]><!-->
    <style type=""text/css"">
      @media only screen and (max-width:480px) {
      			  		@-ms-viewport {width: 320px;}
      			  		@viewport {	width: 320px; }
      				}
    </style>
    <!--<![endif]-->
    <!--[if mso]> 
		<xml> 
			<o:OfficeDocumentSettings> 
				<o:AllowPNG/> 
				<o:PixelsPerInch>96</o:PixelsPerInch> 
			</o:OfficeDocumentSettings> 
		</xml>
		<![endif]-->
    <!--[if lte mso 11]> 
		<style type=""text/css""> 
			.outlook-group-fix{width:100% !important;}
		</style>
		<![endif]-->
    <style type=""text/css"">
      @media only screen and (max-width:480px) {
      
      			  table.full-width-mobile { width: 100% !important; }
      				td.full-width-mobile { width: auto !important; }
      
      }
      @media only screen and (min-width:480px) {
      .dys-column-per-100 {
      	width: 100.000000% !important;
      	max-width: 100.000000%;
      }
      }
      @media only screen and (min-width:480px) {
      .dys-column-per-100 {
      	width: 100.000000% !important;
      	max-width: 100.000000%;
      }
      }
      @media only screen and (max-width:480px) {
      
      			  table.full-width-mobile { width: 100% !important; }
      				td.full-width-mobile { width: auto !important; }
      
      }
      @media only screen and (min-width:480px) {
      .dys-column-per-100 {
      	width: 100.000000% !important;
      	max-width: 100.000000%;
      }
      }
      @media only screen and (min-width:480px) {
      .dys-column-per-100 {
      	width: 100.000000% !important;
      	max-width: 100.000000%;
      }
      }
      @media only screen and (min-width:480px) {
      .dys-column-per-100 {
      	width: 100.000000% !important;
      	max-width: 100.000000%;
      }
      }
      @media only screen and (max-width:480px) {
      
      			  table.full-width-mobile { width: 100% !important; }
      				td.full-width-mobile { width: auto !important; }
      
      }
      @media only screen and (min-width:480px) {
      .dys-column-per-100 {
      	width: 100.000000% !important;
      	max-width: 100.000000%;
      }
      }
      @media only screen and (max-width:480px) {
      
      			  table.full-width-mobile { width: 100% !important; }
      				td.full-width-mobile { width: auto !important; }
      
      }
      @media only screen and (min-width:480px) {
      .dys-column-per-100 {
      	width: 100.000000% !important;
      	max-width: 100.000000%;
      }
      }
    </style>
  </head>
  <body>
    <div>
      <!--[if mso | IE]>
<table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:600px;"" width=""600""><tr><td style=""line-height:0px;font-size:0px;mso-line-height-rule:exactly;"">
<![endif]-->
      <div style='margin:0px auto;max-width:600px;'>
        <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%;'>
          <tbody>
            <tr>
              <td style='direction:ltr;font-size:0px;padding:40px;text-align:center;vertical-align:top;'>
                <!--[if mso | IE]>
<table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td style=""vertical-align:top;width:600px;"">
<![endif]-->
                <div class='dys-column-per-100 outlook-group-fix' style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>
                  <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top;' width='100%'>
                    <tr>
                      <td align='left' style='font-size:0px;padding:0px;word-break:break-word;'>
                        <table border='0' cellpadding='0' cellspacing='0' style='cellpadding:0;cellspacing:0;color:#000000;font-family:Helvetica, Arial, sans-serif;font-size:13px;line-height:22px;table-layout:auto;width:100%;' width='100%'>
                          <tbody>
                            <tr>
                              <td align='left'>
                                <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px;'>
                                  <tbody>
                                    <tr>
                                      <td style='width:250px;'>
                                        <img alt='Descriptive Alt Text' height='auto' src='https://assets.opensourceemails.com/imgs/lifestyle/cdykUp9mR0aRosKeEEcs_logo%20product%20company.png' style='border:none;display:block;font-size:13px;height:auto;outline:none;text-decoration:none;width:100%;' width='250' />
                                      </td>
                                    </tr>
                                  </tbody>
                                </table>
                              </td>
                              <td align='right' style='vertical-align:bottom;' width='34px'>
                                <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px;'>
                                  <tbody>
                                    <tr>
                                      <td style='width:30px; cursor: pointer;'>
                                        <a href='https://www.facebook.com/profile.php?id=100009535411121' target='_blank'>
                                            <img alt='Nossa página no Facebook' height='auto' src='https://assets.opensourceemails.com/imgs/lifestyle/Fct0c2xMRUKPHBdMCcnf_icon-facebook.png' style='border:none;display:block;font-size:13px;height:auto;outline:none;text-decoration:none;width:100%;' width='30' />
                                        </a>
                                      </td>
                                    </tr>
                                  </tbody>
                                </table>
                              </td>
                              <td align='right' style='vertical-align:bottom;' width='34px'>
                                <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px;'>
                                  <tbody>
                                    <tr>
                                      <td style='width:30px; cursor: pointer;'>
                                        <a href='https://www.linkedin.com/in/cleber-rezende-0abb81117' target='_blank'>
                                            <img alt='Nossa página no LinkedIn' height='auto' src='https://assets.opensourceemails.com/imgs/lifestyle/BHraIlyShi7koHdeMEbD_icon-linkedin.png' style='border:none;display:block;font-size:13px;height:auto;outline:none;text-decoration:none;width:100%;' width='30' />
                                        </a>
                                      </td>
                                    </tr>
                                  </tbody>
                                </table>
                              </td>
                              <td align='right' style='vertical-align:bottom;' width='34px'>
                                <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px;'>
                                  <tbody>
                                    <tr>
                                      <td style='width:30px; cursor: pointer;'>
                                        <a href='https://cleberspirlandeli.github.io/portfolio' target='_blank'></a>
                                        <img alt='Nosso site' height='auto' src='https://assets.opensourceemails.com/imgs/lifestyle/Rc7jq7J2ToyH0IGSptTY_icon-twitter.png' style='border:none;display:block;font-size:13px;height:auto;outline:none;text-decoration:none;width:100%;' width='30' />
                                      </td>
                                    </tr>
                                  </tbody>
                                </table>
                              </td>
                            </tr>
                          </tbody>
                        </table>
                      </td>
                    </tr>
                  </table>
                </div>
                <!--[if mso | IE]>
</td></tr></table>
<![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <!--[if mso | IE]>
</td></tr></table>
<![endif]-->
      <table align='center' background='https://assets.opensourceemails.com/imgs/lifestyle/binu6fnar8unlra4bqse_osetu-product-header.jpg' border='0' cellpadding='0' cellspacing='0' role='presentation' style='background:#f0f0f0 url(https://assets.opensourceemails.com/imgs/lifestyle/binu6fnar8unlra4bqse_osetu-product-header.jpg) top center / cover no-repeat;width:100%;'>
        <tbody>
          <tr>
            <td>
              <!--[if mso | IE]>
<v:rect style=""mso-width-percent:1000;"" xmlns:v=""urn:schemas-microsoft-com:vml"" fill=""true"" stroke=""false""><v:fill src=""https://assets.opensourceemails.com/imgs/lifestyle/binu6fnar8unlra4bqse_osetu-product-header.jpg"" color=""#f0f0f0"" origin=""0.5, 0"" position=""0.5, 0"" type=""tile"" /><v:textbox style=""mso-fit-shape-to-text:true"" inset=""0,0,0,0"">
<![endif]-->
              <div style='margin:0px auto;max-width:600px;'>
                <div style='font-size:0;line-height:0;'>
                  <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%;'>
                    <tbody>
                      <tr>
                        <td style='direction:ltr;font-size:0px;padding:0px;text-align:center;vertical-align:top;'>
                          <!--[if mso | IE]>
<table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td style=""vertical-align:top;width:600px;"">
<![endif]-->
                          <div class='dys-column-per-100 outlook-group-fix' style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>
                            <table border='0' cellpadding='0' cellspacing='0' role='presentation' width='100%'>
                              <tbody>
                                <tr>
                                  <td style='padding:40px 20px;vertical-align:top;'>
                                    <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='' width='100%'>
                                      <tr>
                                        <td align='left' style='font-size:0px;padding:10px 25px;padding-bottom:0px;word-break:break-word;'>
                                          <div style='color:#000000;font-family:Martel;font-size:22px;font-weight:900;line-height:72px;text-align:left;'>
                                            YOUR COMPANY
                                          </div>
                                        </td>
                                      </tr>
                                      <tr>
                                        <td align='left' style='font-size:0px;padding:10px 25px;padding-bottom:50px;padding-top:00px;word-break:break-word;'>
                                          <div style='color:#000000;font-family:Martel;font-size:56px;font-weight:900;line-height:72px;text-align:left;'>
                                            CONFIRMAÇÃO DE EMAIL
                                          </div>
                                        </td>
                                      </tr>
                                    </table>
                                  </td>
                                </tr>
                              </tbody>
                            </table>
                          </div>
                          <!--[if mso | IE]>
</td></tr></table>
<![endif]-->
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
              <!--[if mso | IE]>
</v:textbox></v:rect>
<![endif]-->
            </td>
          </tr>
        </tbody>
      </table>
      <!--[if mso | IE]>
<table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:600px;"" width=""600""><tr><td style=""line-height:0px;font-size:0px;mso-line-height-rule:exactly;"">
<![endif]-->
      <div style='margin:0px auto;max-width:600px;'>
        <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%;'>
          <tbody>
            <tr>
              <td style='direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;'>
                <!--[if mso | IE]>
<table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td style=""vertical-align:top;width:600px;"">
<![endif]-->
                <div class='dys-column-per-100 outlook-group-fix' style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>
                  <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top;' width='100%'>
                    <tr>
                      <td align='center' style='font-size:0px;padding:10px 25px;word-break:break-word;'>
                        <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px;'>
                          <tbody>
                            <tr>
                              <td style='width:300px;'>
                                <img height='auto' src='https://assets.opensourceemails.com/imgs/lifestyle/rQdM7LSiRUqNh29a8Mo8_divider%20product%20lifestyle.png' style='border:none;display:block;font-size:13px;height:auto;outline:none;text-decoration:none;width:100%;' width='300' />
                              </td>
                            </tr>
                          </tbody>
                        </table>
                      </td>
                    </tr>
                  </table>
                </div>
                <!--[if mso | IE]>
</td></tr></table>
<![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <!--[if mso | IE]>
</td></tr></table>
<![endif]-->
      <!--[if mso | IE]>
<table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:600px;"" width=""600""><tr><td style=""line-height:0px;font-size:0px;mso-line-height-rule:exactly;"">
<![endif]-->
      <div style='margin:0px auto;max-width:600px;'>
        <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%;'>
          <tbody>
            <tr>
              <td style='direction:ltr;font-size:0px;padding:10px 20px;text-align:center;vertical-align:top;'>
                <!--[if mso | IE]>
<table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td style=""vertical-align:top;width:600px;"">
<![endif]-->
                <div class='dys-column-per-100 outlook-group-fix' style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>
                  <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top;' width='100%'>
                    <tr>
                      <td align='left' style='font-size:0px;padding:10px 25px;word-break:break-word;'>
                        <div style='color:#000000;font-family:Helvetica, Arial, sans-serif;font-size:18px;font-weight:light;line-height:28px;text-align:left;'>
                          Olá #[001]!
                          <br />
                          Obrigado por se cadastrar na plataforma YOUR COMPANY!
                          <br />
                          <br />
                          Clique no botão abaixo para validar sua conta e confirmar se temos o endereço de e-mail correto.
                        </div>
                      </td>
                    </tr>
                  </table>
                </div>
                <!--[if mso | IE]>
</td></tr></table>
<![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
      </div>
        <!--[if mso | IE]>
        </td></tr></table>
        <![endif]-->
            <!--[if mso | IE]>
        <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:600px;"" width=""600""><tr><td style=""line-height:0px;font-size:0px;mso-line-height-rule:exactly;"">
        <![endif]-->
      <div style='margin:0px auto;max-width:600px;'>
        <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%;'>
          <tbody>
            <tr>
              <td style='direction:ltr;font-size:0px;padding:20px;text-align:center;vertical-align:top;'>
                <!--[if mso | IE]>
                <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td style=""vertical-align:top;width:600px;"">
                <![endif]-->
                <div class='dys-column-per-100 outlook-group-fix' style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>
                  <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top;' width='100%'>
                    <tr>
                      <td align='left' style='font-size:0px;padding:10px 25px;word-break:break-word;' vertical-align='middle'>
                        <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:separate;line-height:100%;'>
                          <tr>
                            <td align='center' bgcolor='BLACK' role='presentation' style='background-color:BLACK;border:none;border-radius:0px;cursor:auto;padding:10px 25px;' valign='middle'>
                              <a href='#[002]' style='background:BLACK;color:#ffffff;font-family:Helvetica, Arial, sans-serif;font-size:22px;font-weight:normal;line-height:120%;margin:0;text-decoration:none;text-transform:none;' target='_blank'>
                                CONFIRMAR EMAIL
                              </a>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </div>
                <!--[if mso | IE]>
                </td></tr></table>
                <![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
      </div>
        <!--[if mso | IE]>
        </td></tr></table>
        <![endif]-->
            <!--[if mso | IE]>
        <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:600px;"" width=""600""><tr><td style=""line-height:0px;font-size:0px;mso-line-height-rule:exactly;"">
        <![endif]-->
      <div style='margin:0px auto;max-width:600px;'>
        <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%;'>
          <tbody>
            <tr>
              <td style='direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;'>
                <!--[if mso | IE]>
                <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td style=""vertical-align:top;width:600px;"">
                <![endif]-->
                <div class='dys-column-per-100 outlook-group-fix' style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>
                  <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top;' width='100%'>
                    <tr>
                      <td align='center' style='font-size:0px;padding:10px 25px;word-break:break-word;'>
                        <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px;'>
                          <tbody>
                            <tr>
                              <td style='width:300px;'>
                                <img height='auto' src='https://assets.opensourceemails.com/imgs/lifestyle/rQdM7LSiRUqNh29a8Mo8_divider%20product%20lifestyle.png' style='border:none;display:block;font-size:13px;height:auto;outline:none;text-decoration:none;width:100%;' width='300' />
                              </td>
                            </tr>
                          </tbody>
                        </table>
                      </td>
                    </tr>
                  </table>
                </div>
                <!--[if mso | IE]>
                </td></tr></table>
                <![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <!--[if mso | IE]>
</td></tr></table>
<![endif]-->
      <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='background:black;background-color:black;width:100%;'>
        <tbody>
          <tr>
            <td>
              <div style='margin:0px auto;max-width:600px;'>
                <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%;'>
                  <tbody>
                    <tr>
                      <td style='direction:ltr;font-size:0px;padding:50px;text-align:center;vertical-align:top;'>
                        <!--[if mso | IE]>
<table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td style=""vertical-align:top;width:600px;"">
<![endif]-->
                        <div class='dys-column-per-100 outlook-group-fix' style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>
                          <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top;' width='100%'>
                            <tr>
                              <td align='left' style='font-size:0px;padding:0px;word-break:break-word;'>
                                <table border='0' cellpadding='0' cellspacing='0' style='cellpadding:0;cellspacing:0;color:#000000;font-family:Helvetica, Arial, sans-serif;font-size:13px;line-height:22px;table-layout:auto;width:100%;' width='100%'>
                                  <tr>
                                    <td align='left'>
                                      <div style='color:#666666;font-family:Helvetica, Arial, sans-serif;font-size:12px;line-height:28px;text-align:left;'>
                                        <p>
                                          © Copyright 2019 YOUR COMPANY
                                          <br />
                                          <a href='#' style='color: #666;'>
                                            Privacy Policy
                                          </a>
                                          <br />
                                          <a href='#LINKPrivacyPolicy#011' style='color: #666;'>
                                            Terms of Use
                                          </a>
                                          <br />
                                          <a href='#LINKTermsOfUse#013' style='color: #666;'>
                                            Unsubscribe
                                          </a>
                                        </p>
                                      </div>
                                    </td>
                                    <td align='right' style='vertical-align:top; opacity: 0.35;' width='34px'>
                                      <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px;'>
                                        <tbody>
                                          <tr>
                                            <td style='width:30px;'>
                                                <a href='https://www.facebook.com/profile.php?id=100009535411121' target='_blank'></a>
                                                    <img alt='Nossa página no Facebook' height='auto' src='https://assets.opensourceemails.com/imgs/lifestyle/Fct0c2xMRUKPHBdMCcnf_icon-facebook.png' style='border:none;display:block;font-size:13px;height:auto;outline:none;text-decoration:none;width:100%;' width='30' />
                                                </a>
                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>
                                    </td>
                                    <td align='right' style='vertical-align:top; opacity: 0.35;' width='34px'>
                                      <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px;'>
                                        <tbody>
                                          <tr>
                                            <td style='width:30px;'>
                                                <a href='https://www.linkedin.com/in/cleber-rezende-0abb81117' target='_blank'>
                                                    <img alt='Nossa página no Linked In' height='auto' src='https://assets.opensourceemails.com/imgs/lifestyle/BHraIlyShi7koHdeMEbD_icon-linkedin.png' style='border:none;display:block;font-size:13px;height:auto;outline:none;text-decoration:none;width:100%;' width='30' />
                                                </a>    
                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>
                                    </td>
                                    <td align='right' style='vertical-align:top; opacity: 0.35;' width='34px'>
                                      <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px;'>
                                        <tbody>
                                          <tr>
                                            <td style='width:30px;'>
                                                <a href='https://cleberspirlandeli.github.io/portfolio' target='_blank'></a>
                                                    <img alt='Nosso site' height='auto' src='https://assets.opensourceemails.com/imgs/lifestyle/Rc7jq7J2ToyH0IGSptTY_icon-twitter.png' style='border:none;display:block;font-size:13px;height:auto;outline:none;text-decoration:none;width:100%;' width='30' />
                                                </a>
                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>
                                    </td>
                                  </tr>
                                </table>
                              </td>
                            </tr>
                          </table>
                        </div>
                        <!--[if mso | IE]>
                        </td></tr></table>
                        <![endif]-->
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </body>
</html>

            ";

            #endregion


            string url = _configuration["UrlBackend"] + "usuario/confirm-email?codigo=" + dto.Codigo;

            strHtml = strHtml.Replace("#[001]", dto.Nome);
            strHtml = strHtml.Replace("#[002]", url);

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(_configuration["EmailAddress"]); // De
            mail.To.Add(dto.Email); // Para
            mail.Subject = "YOUR COMPANY - Confirmação de E-mail"; // Assunto
            mail.IsBodyHtml = true;
            mail.Body = strHtml; // Mensagem

            // Incluir Anexos
            //mail.Attachments.Add(new Attachment(@"C:\teste.txt"));

            using (var smtp = new SmtpClient("smtp.gmail.com"))
            {
                smtp.EnableSsl = true; // GMail requer SSL
                smtp.Port = 587;       // porta para SSL
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
                smtp.UseDefaultCredentials = false; // vamos utilizar credencias especificas

                // seu usuário e senha para autenticação
                smtp.Credentials = new NetworkCredential(_configuration["EmailAddress"], _configuration["EmailPassword"]);

                // envia o e-mail
                smtp.Send(mail);
            }
        }
        #endregion

    }
}
