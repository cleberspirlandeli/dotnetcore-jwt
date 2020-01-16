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


            string strHtml = @"
                <html>
                <body>
                <div style=""display: block;margin: 0 auto;width: 620px;"">
                <div style=""height:550px;background-color:#2196F3;border-radius:15px 100px;"">
                <div style=""text-align:center;padding-top:10%;font-size:20px;color:#FFF;"">
                <p>SLOGAN EMPRESA</p>
                <p>Olá ${0}!</p>
                <p>Precisamos que você confirme seu email para ter acesso a nossa rede.</p>
                <p>Clique no link a baixo para confirmar</p>
                <p></p>
                <p><a href=""https://cleberspirlandeli.github.io/portfolio/"" target=""_blank""  style=""text-decoration: none;cursor:pointer;"">CONFIRMAR EMAIL</a></p>
                <p></p>
                <p>Obs: Se não receber o email, verifique a caixa de lixo eletrônico.</p>
                <p style=""font-size:12px;color:#000;bottom:0px;margin-top:170px"">Desenvolvido por Cleber Rezende</p>
                </div></div></div>
                </body>
                </html>";

            strHtml.Replace("${0}", dto.Nome);


//            string strHtml = @"
//            <html>
//            <head>
//                <style>
//                #outlook a {
//                    padding: 0;
//                }

//                .ExternalClass {
//                    width: 100%;
//                }

//                .ExternalClass,
//                .ExternalClass p,
//                .ExternalClass span,
//                .ExternalClass font,
//                .ExternalClass td,
//                .ExternalClass div
//                {
//                    line-height: 100%;
//                }

//                .es-button {
//                    mso-style-priority: 100 !important;
//                    text-decoration: none !important;
//                    transition: all 100ms ease-in;
//                }

//                a[x - apple - data - detectors] {
//                    color: inherit !important;
//                    text-decoration: none !important;
//                    font-size: inherit !important;
//                    font-family: inherit !important;
//                    font-weight: inherit !important;
//                    line-height: inherit !important;
//                }

//                .es-button:hover {
//                    background: #555555 !important;
//                    border-color: #555555 !important;
//                }

//                .es-desk-hidden {
//                    display: none;
//                    float: left;
//                    overflow: hidden;
//                    width: 0;
//                    max-height: 0;
//                    line-height: 0;
//                    mso-hide: all;
//                }


//                s {
//                    text-decoration: line-through;
//                }

//                html,
//                body {
//                    width: 100%;
//                    font-family: helvetica, 'helvetica neue', arial, verdana, sans-serif;
//                    -webkit-text-size-adjust: 100%;
//                    -ms-text-size-adjust: 100%;
//                }

//                table {
//                    mso-table-lspace: 0pt;
//                    mso-table-rspace: 0pt;
//                    border-collapse: collapse;
//                    border-spacing: 0px;
//                }

//                table td,
//                html,
//                body,
//                .es-wrapper {
//                    padding: 0;
//                    Margin: 0;
//                }

//                .es-content,
//                .es-header,
//                .es-footer {
//                    table-layout: fixed !important;
//                width: 100%;
//                }

//                img {
//                    display: block;
//                    border: 0;
//                    outline: none;
//                    text-decoration: none;
//                    -ms-interpolation-mode: bicubic;
//                }

//                table tr
//                {
//                    border-collapse: collapse;
//                }

//                p,
//                hr {
//                    Margin: 0;
//                }

//                h1,
//                h2,
//                h3,
//                h4,
//                h5 {
//                    Margin: 0;
//                    line-height: 120%;
//                    mso-line-height-rule: exactly;
//                    font-family: lato, 'helvetica neue', helvetica, arial, sans-serif;
//                }

//                p,
//                ul li,
//                ol li,
//                a {
//                    -webkit-text-size-adjust: none;
//                    -ms-text-size-adjust: none;
//                    mso-line-height-rule: exactly;
//                }

//                .es-left {
//                    float: left;
//                }

//                .es-right {
//                    float: right;
//                }

//                .es-p5 {
//                    padding: 5px;
//                }

//                .es-p5t {
//                    padding-top: 5px;
//                }

//                .es-p5b {
//                    padding-bottom: 5px;
//                }

//                .es-p5l {
//                    padding-left: 5px;
//                }

//                .es-p5r {
//                    padding-right: 5px;
//                }

//                .es-p10 {
//                    padding: 10px;
//                }

//                .es-p10t {
//                    padding-top: 10px;
//                }

//                .es-p10b {
//                    padding-bottom: 10px;
//                }

//                .es-p10l {
//                    padding-left: 10px;
//                }

//                .es-p10r {
//                    padding-right: 10px;
//                }

//                .es-p15 {
//                    padding: 15px;
//                }

//                .es-p15t {
//                    padding-top: 15px;
//                }

//                .es-p15b {
//                    padding-bottom: 15px;
//                }

//                .es-p15l {
//                    padding-left: 15px;
//                }

//                .es-p15r {
//                    padding-right: 15px;
//                }

//                .es-p20 {
//                    padding: 20px;
//                }

//                .es-p20t {
//                    padding-top: 20px;
//                }

//                .es-p20b {
//                    padding-bottom: 20px;
//                }

//                .es-p20l {
//                    padding-left: 20px;
//                }

//                .es-p20r {
//                    padding-right: 20px;
//                }

//                .es-p25 {
//                    padding: 25px;
//                }

//                .es-p25t {
//                    padding-top: 25px;
//                }

//                .es-p25b {
//                    padding-bottom: 25px;
//                }

//                .es-p25l {
//                    padding-left: 25px;
//                }

//                .es-p25r {
//                    padding-right: 25px;
//                }

//                .es-p30 {
//                    padding: 30px;
//                }

//                .es-p30t {
//                    padding-top: 30px;
//                }

//                .es-p30b {
//                    padding-bottom: 30px;
//                }

//                .es-p30l {
//                    padding-left: 30px;
//                }

//                .es-p30r {
//                    padding-right: 30px;
//                }

//                .es-p35 {
//                    padding: 35px;
//                }

//                .es-p35t {
//                    padding-top: 35px;
//                }

//                .es-p35b {
//                    padding-bottom: 35px;
//                }

//                .es-p35l {
//                    padding-left: 35px;
//                }

//                .es-p35r {
//                    padding-right: 35px;
//                }

//                .es-p40 {
//                    padding: 40px;
//                }

//                .es-p40t {
//                    padding-top: 40px;
//                }

//                .es-p40b {
//                    padding-bottom: 40px;
//                }

//                .es-p40l {
//                    padding-left: 40px;
//                }

//                .es-p40r {
//                    padding-right: 40px;
//                }

//                .es-menu td
//                {
//                    border: 0;
//                }

//                .es-menu td a img
//                {
//                    display: inline-block !important;
//                }


//                a {
//                    font-family: helvetica, 'helvetica neue', arial, verdana, sans-serif;
//                    font-size: 15px;
//                    text-decoration: underline;
//                }

//                h1 {
//                    font-size: 30px;
//                    font-style: normal;
//                    font-weight: bold;
//                    color: #333333;
//                }

//                h1 a
//                {
//                    font-size: 30px;
//                    text-align: center;
//                }

//                h2 {
//                    font-size: 20px;
//                    font-style: normal;
//                    font-weight: bold;
//                    color: #333333;
//                }

//                h2 a
//                {
//                    font-size: 20px;
//                    text-align: left;
//                }

//                h3 {
//                    font-size: 18px;
//                    font-style: normal;
//                    font-weight: normal;
//                    color: #333333;
//                }

//                h3 a
//                {
//                    font-size: 18px;
//                    text-align: left;
//                }

//                p,
//                ul li,
//                ol li {
//                    font-size: 15px;
//                    font-family: helvetica, 'helvetica neue', arial, verdana, sans-serif;
//                    line-height: 150%;
//                }

//                ul li,
//                ol li {
//                    Margin-bottom: 15px;
//                }

//                .es-menu td a {
//                    text-decoration: none;
//                    display: block;
//                }

//                .es-wrapper {
//                    width: 100%;
//                    height: 100%;
//                    background-image: ;
//                    background-repeat: repeat;
//                    background-position: center top;
//                }

//                .es-wrapper-color {
//                    background-color: #f1f1f1;
//                }

//                .es-content-body {
//                    background-color: #ffffff;
//                }

//                .es-content-body p,
//                .es-content-body ul li,
//                .es-content-body ol li {
//                    color: #555555;
//                }

//                .es-content-body a
//                {
//                    color: #26a4d3;
//                }

//                .es-header {
//                    background-color: transparent;
//                    background-image: ;
//                    background-repeat: repeat;
//                    background-position: center top;
//                }

//                .es-header-body {
//                    background-color: #333333;
//                }

//                .es-header-body p,
//                .es-header-body ul li,
//                .es-header-body ol li {
//                    color: #ffffff;
//                    font-size: 14px;
//                }

//                .es-header-body a
//                {
//                    color: #ffffff;
//                    font-size: 14px;
//                }

//                .es-footer {
//                    background-color: transparent;
//                    background-image: ;
//                    background-repeat: repeat;
//                    background-position: center top;
//                }

//                .es-footer-body {
//                    background-color: #ffffff;
//                }

//                .es-footer-body p,
//                .es-footer-body ul li,
//                .es-footer-body ol li {
//                    color: #666666;
//                    font-size: 12px;
//                }

//                .es-footer-body a
//                {
//                    color: #666666;
//                    font-size: 12px;
//                }

//                .es-infoblock,
//                .es-infoblock p,
//                .es-infoblock ul li,
//                .es-infoblock ol li {
//                    line-height: 120%;
//                    font-size: 12px;
//                    color: #cccccc;
//                }

//                .es-infoblock a
//                {
//                    font-size: 12px;
//                    color: #cccccc;
//                }

//                a.es-button {
//                    border-style: solid;
//                    border-color: #26a4d3;
//                    border-width: 15px 30px 15px 30px;
//                    display: inline-block;
//                    background: #26a4d3;
//                    border-radius: 50px;
//                    font-size: 14px;
//                    font-family: arial, 'helvetica neue', helvetica, sans-serif;
//                    font-weight: bold;
//                    font-style: normal;
//                    line-height: 120%;
//                    color: #ffffff;
//                    text-decoration: none;
//                    width: auto;
//                    text-align: center;
//                }

//                .es-button-border {
//                    border-style: solid solid solid solid;
//                border-color: #26a4d3 #26a4d3 #26a4d3 #26a4d3;
//                    background: #26a4d3;
//                    border-width: 0px 0px 0px 0px;
//                    display: inline-block;
//                    border-radius: 50px;
//                    width: auto;
//                }


//                @media only screen and(max-width: 600px)
//                {

//                    p,
//                    ul li,
//                    ol li,
//                    a {
//                        font-size: 17px !important;
//                        line-height: 150 % !important;
//                    }

//                    h1 {
//                        font-size: 30px !important;
//                        text-align: center;
//                        line-height: 120 % !important;
//                    }

//                    h2 {
//                        font-size: 26px !important;
//                        text-align: left;
//                        line-height: 120 % !important;
//                    }

//                    h3 {
//                        font-size: 20px !important;
//                        text-align: left;
//                        line-height: 120 % !important;
//                    }

//                    h1 a {
//                        font-size: 30px !important;
//                        text-align: center;
//                    }

//                    h2 a {
//                        font-size: 20px !important;
//                        text-align: left;
//                    }

//                    h3 a {
//                        font-size: 20px !important;
//                        text-align: left;
//                    }

//                    .es-menu td a {
//                        font-size: 16px !important;
//                    }

//                    .es-header-body p,
//                    .es-header-body ul li,
//                        .es-header-body ol li,
//                            .es-header-body a {
//                        font-size: 16px !important;
//                    }

//                    .es-footer-body p,
//                    .es-footer-body ul li,
//                        .es-footer-body ol li,
//                            .es-footer-body a {
//                        font-size: 17px !important;
//                    }

//                    .es-infoblock p,
//                    .es-infoblock ul li,
//                      .es-infoblock ol li,
//                        .es-infoblock a {
//                        font-size: 12px !important;
//                    }

//                    *[class=""gmail-fix""] {
//                        display: none !important;
//                    }

//                    .es-m-txt-c,
//                    .es-m-txt-c h1,
//                    .es-m-txt-c h2,
//                    .es-m-txt-c h3
//                {
//                    text-align: center !important;
//                }

//                    .es-m-txt-r,
//                    .es-m-txt-r h1,
//                    .es-m-txt-r h2,
//                    .es-m-txt-r h3
//                {
//                    text-align: right !important;
//                }

//                    .es-m-txt-l,
//                    .es-m-txt-l h1,
//                    .es-m-txt-l h2,
//                    .es-m-txt-l h3
//                {
//                    text-align: left !important;
//                }

//                    .es-m-txt-r img,
//                    .es-m-txt-c img,
//                    .es-m-txt-l img
//                {
//                    display: inline !important;
//                }

//                    .es-button-border {
//                        display: inline-block !important;
//                    }

//                    a.es-button {
//                        font-size: 14px !important;
//                        display: inline-block !important;
//                        border-width: 15px 25px 15px 25px !important;
//                    }

//                    .es-btn-fw {
//                        border-width: 10px 0px !important;
//                        text-align: center !important;
//                    }

//                    .es-adaptive table,
//                    .es-btn-fw,
//                    .es-btn-fw-brdr,
//                    .es-left,
//                    .es-right {
//                        width: 100% !important;
//                    }

//                    .es-content table,
//                    .es-header table,
//                    .es-footer table,
//                    .es-content,
//                    .es-footer,
//                    .es-header {
//                        width: 100% !important;
//                        max-width: 600px !important;
//                    }

//                    .es-adapt-td {
//                        display: block !important;
//                        width: 100% !important;
//                    }

//                    .adapt-img {
//                        width: 100% !important;
//                        height: auto !important;
//                    }

//                    .es-m-p0 {
//                        padding: 0px !important;
//                    }

//                    .es-m-p0r {
//                        padding-right: 0px !important;
//                    }

//                    .es-m-p0l {
//                        padding-left: 0px !important;
//                    }

//                    .es-m-p0t {
//                        padding-top: 0px !important;
//                    }

//                    .es-m-p0b {
//                        padding-bottom: 0 !important;
//                    }

//                    .es-m-p20b {
//                        padding-bottom: 20px !important;
//                    }

//                    .es-mobile-hidden,
//                    .es-hidden {
//                        display: none !important;
//                    }

//                    .es-desk-hidden {
//                        display: table-row !important;
//                        width: auto !important;
//                        overflow: visible !important;
//                        float: none !important;
//                        max-height: inherit !important;
//                        line-height: inherit !important;
//                    }

//                    .es-desk-menu-hidden {
//                        display: table-cell !important;
//                    }

//                    table.es-table-not-adapt,
//                    .esd-block-html table
//                    {
//                        width: auto !important;
//                    }

//                table.es-social {
//                        display: inline-block !important;
//                    }

//                    table.es-social td
//                    {
//                        display: inline-block !important;
//                    }
//                }


//                .es-p-default {
//                    padding-top: 20px;
//                    padding-right: 40px;
//                    padding-bottom: 0px;
//                    padding-left: 40px;
//                }

//                .es-p-all-default {
//                    padding: 0px;
//                }
//                </style>    
//            </head>

//<body>

//<td class=""esd-structure es-p30t es-p30b es-p40r es-p40l"" style=""background-color: rgb(51, 51, 51);"" bgcolor=""#333333"" align=""left"">
//    <table width = ""100%"" cellspacing=""0"" cellpadding=""0"">
//        <tbody>
//            <tr>
//                <td class=""esd-container-frame"" width=""520"" valign=""top"" align=""center"">
//                    <table width = ""100%"" cellspacing=""0"" cellpadding=""0"">
//                        <tbody>
//                            <tr>
//                                <td class=""esd-block-image"" align=""center"">
//                                    <a href = ""https://viewstripo.email/"" target=""_blank"">
//                                        <img src = ""https://etnfxo.stripocdn.email/content/guids/CABINET_85e4431b39e3c4492fca561009cef9b5/images/51621521206431413.png"" alt style=""display: block;"" width=""57""></a></td>
//                            </tr>
//                        </tbody>
//                    </table>
//                </td>
//            </tr>
//        </tbody>
//    </table>
//</td>
//<td class=""esd-stripe"" esd-custom-block-id=""7388"" align=""center"">
//    <table class=""es-content-body"" style=""background-color: rgb(51, 51, 51);"" width=""600"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#333333"" align=""center"">
//        <tbody>
//            <tr>
//                <td class=""esd-structure esd-checked es-p40t es-p40b es-p40r es-p40l"" style=""background-image: url('https://etnfxo.stripocdn.email/content/guids/CABINET_85e4431b39e3c4492fca561009cef9b5/images/93491522393929597.png'); background-repeat: no-repeat;"" align=""left"">
//                    <table width = ""100%"" cellspacing=""0"" cellpadding=""0"">
//                        <tbody>
//                            <tr>
//                                <td class=""esd-container-frame"" width=""520"" valign=""top"" align=""center"">
//                                    <table width = ""100%"" cellspacing=""0"" cellpadding=""0"">
//                                        <tbody>
//                                            <tr>
//                                                <td align = ""center"" class=""esd-block-text es-p40t es-p10b"">
//                                                    <h1 style = ""color: #ffffff;"">BEM VINDO AO TESTE</h1>
//                                                </td>
//                                            </tr>
//                                            <tr>
//                                                <td esdev-links-color=""#757575"" class=""esd-block-text es-p10t es-p20b es-p30r es-p30l"" align=""center"">
//                                                    <p style = ""color: #757575;"">Se hoje é o dia das crianças... Ontem eu disse: o dia da criança é o dia da mãe, dos pais, das professoras, mas também é o dia dos animais, sempre que você olha uma criança, há sempre uma figura oculta, que é um cachorro atrás. O que é algo muito importante! incididunt ut labore.</p>
//                                                </td>
//                                            </tr>
//                                            <tr>
//                                                <td class=""esd-block-button es-p10t es-p20b"" align=""center"">
//                                                    <span class=""es-button-border"" style=""border-width: 0px; border-style: solid; background: none 0% 0% repeat scroll rgb(38, 164, 211); border-color: rgb(38, 164, 211);""><a href = ""https://cleberspirlandeli.github.io/portfolio/"" class=""es-button"" target=""_blank"" style=""background: rgb(38, 164, 211) none repeat scroll 0% 0%; border-color: rgb(38, 164, 211);"">VALIDAR EMAIL</a></span></td>
//                                            </tr>
//                                        </tbody>
//                                    </table>
//                                </td>
//                            </tr>
//                        </tbody>
//                    </table>
//                </td>
//            </tr>
//        </tbody>
//    </table>
//</td>
//<td class=""esd-stripe"" align=""center"">
//    <table class=""es-content-body"" width=""600"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"">
//        <tbody>
//            <tr>
//                <td class=""esd-structure es-p40t es-p40r es-p40l"" esd-custom-block-id=""7389"" align=""left"">
//                    <table width = ""100%"" cellspacing=""0"" cellpadding=""0"">
//                        <tbody>
//                            <tr>
//                                <td class=""esd-container-frame"" width=""520"" valign=""top"" align=""center"">
//                                    <table width = ""100%"" cellspacing=""0"" cellpadding=""0"">
//                                        <tbody>
//                                            <tr>
//                                                <td class=""esd-block-text es-p5t es-p15b es-m-txt-c"" align=""left"">
//                                                    <h2>YOUR ACCOUNT IS NOW ACTIVE</h2>
//                                                </td>
//                                            </tr>
//                                            <tr>
//                                                <td class=""esd-block-text es-p10b"" align=""left"">
//                                                    <p><strong>A única área que eu acho, que vai exigir muita atenção nossa, e aí eu já aventei a hipótese de até criar um ministério.É na área de...Na área... Eu diria assim, como uma espécie de analogia com o que acontece na área agrícola.</strong></p>
//                                                </td>
//                                            </tr>
//                                            <tr>
//                                                <td class=""esd-block-text es-p10t es-p10b"" align=""left"">
//                                                    <p>Todos as descrições das pessoas são sobre a humanidade do atendimento, a pessoa pega no pulso, examina, olha com carinho.Então eu acho que vai ter outra coisa, que os médicos cubanos trouxeram pro brasil, um alto grau de humanidade. </p>
//                                                </td>
//                                            </tr>
//                                            <tr>
//                                                <td class=""esd-block-text es-p10t es-p10b"" align=""left"">
//                                                    <p>Yours sincerely,</p>
//                                                </td>
//                                            </tr>
//                                        </tbody>
//                                    </table>
//                                </td>
//                            </tr>
//                        </tbody>
//                    </table>
//                </td>
//            </tr>
//            <tr>
//                <td class=""esd-structure es-p10t es-p40b es-p40r es-p40l"" esd-custom-block-id=""7390"" align=""left"">
//                    <!--[if mso]><table width = ""520"" cellpadding=""0""
//                            cellspacing=""0""><tr><td width = ""40"" valign=""top""><![endif]-->
//                    <table class=""es-left"" cellspacing=""0"" cellpadding=""0"" align=""left"">
//                        <tbody>
//                            <tr>
//                                <td class=""es-m-p0r es-m-p20b esd-container-frame"" width=""40"" valign=""top"" align=""center"">
//                                    <table width = ""100%"" cellspacing=""0"" cellpadding=""0"">
//                                        <tbody>
//                                            <tr>
//                                                <td class=""esd-block-image"" align=""left"">
//                                                    <a target = ""_blank""><img src = ""https://etnfxo.stripocdn.email/content/guids/CABINET_85e4431b39e3c4492fca561009cef9b5/images/29241521207598269.jpg"" alt style=""display: block;"" width=""40""></a></td>
//                                            </tr>
//                                        </tbody>
//                                    </table>
//                                </td>
//                            </tr>
//                        </tbody>
//                    </table>
//                    <!--[if mso]></td><td width = ""20""></td><td width = ""460"" valign=""top""><![endif]-->
//                    <table cellspacing = ""0"" cellpadding=""0"" align=""right"">
//                        <tbody>
//                            <tr>
//                                <td class=""esd-container-frame"" width=""460"" align=""left"">
//                                    <table width = ""100%"" cellspacing=""0"" cellpadding=""0"">
//                                        <tbody>
//                                            <tr>
//                                                <td class=""esd-block-text es-p10t"" align=""left"">
//                                                    <p style = ""color: #222222; font-size: 14px;""><strong>Anna Bella</strong><br></p>
//                                                </td>
//                                            </tr>
//                                            <tr>
//                                                <td class=""esd-block-text"" align=""left"">
//                                                    <p style = ""color: #666666; font-size: 14px;"">CEO | Vision</p>
//                                                </td>
//                                            </tr>
//                                        </tbody>
//                                    </table>
//                                </td>
//                            </tr>
//                        </tbody>
//                    </table>
//                </td>
//            </tr>
//        </tbody>
//    </table>
//</td>
//<td class=""esd-stripe"" esd-custom-block-id=""7392"" align=""center"">
//    <table class=""es-content-body"" style=""background-color: rgb(38, 164, 211);"" width=""600"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#26a4d3"" align=""center"">
//        <tbody>
//            <tr>
//                <td class=""esd-structure es-p40t es-p20b es-p40r es-p40l"" style=""background-color: rgb(38, 164, 211);"" bgcolor=""#26a4d3"" align=""left"">
//                    <table width = ""100%"" cellspacing=""0"" cellpadding=""0"">
//                        <tbody>
//                            <tr>
//                                <td class=""esd-container-frame"" width=""520"" valign=""top"" align=""center"">
//                                    <table width = ""100%"" cellspacing=""0"" cellpadding=""0"">
//                                        <tbody>
//                                            <tr>
//                                                <td class=""esd-block-text es-m-txt-c"" align=""center"">
//                                                    <h2 style = ""color: #ffffff;"">YOUR FEEDBACK IS IMPORTANT<br></h2>
//                                                </td>
//                                            </tr>
//                                            <tr>
//                                                <td class=""esd-block-text es-p5t es-p10b"" align=""center"">
//                                                    <p style = ""color: #aad4ea; font-size: 17px;"">Let us know what you think of our latest updates<br></p>
//                                                </td>
//                                            </tr>
//                                            <tr>
//                                                <td class=""esd-block-button es-p10"" align=""center"">
//                                                    <span class=""es-button-border"" style=""background: rgb(255, 255, 255) none repeat scroll 0% 0%;""><a href = ""https://cleberspirlandeli.github.io/portfolio/"" class=""es-button"" target=""_blank"" style=""background: rgb(255, 255, 255) none repeat scroll 0% 0%; border-color: rgb(255, 255, 255); color: rgb(38, 164, 211); border-width: 15px 25px;"">VALIDAR EMAIL</a></span></td>
//                                            </tr>
//                                        </tbody>
//                                    </table>
//                                </td>
//                            </tr>
//                        </tbody>
//                    </table>
//                </td>
//            </tr>
//        </tbody>
//    </table>
//</td>
//<td class=""esd-stripe"" esd-custom-block-id=""7393"" align=""center"">
//    <table class=""es-content-body"" style=""background-color: rgb(41, 40, 40);"" width=""600"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#292828"" align=""center"">
//        <tbody>
//            <tr>
//                <td class=""esd-structure es-p30t es-p30b es-p40r es-p40l"" align=""left"">
//                    <table width = ""100%"" cellspacing=""0"" cellpadding=""0"">
//                        <tbody>
//                            <tr>
//                                <td class=""esd-container-frame"" width=""520"" valign=""top"" align=""center"">
//                                    <table width = ""100%"" cellspacing=""0"" cellpadding=""0"">
//                                        <tbody>
//                                            <tr>
//                                                <td class=""esd-block-social"" align=""center"">
//                                                    <table class=""es-table-not-adapt es-social"" cellspacing=""0"" cellpadding=""0"">
//                                                        <tbody>
//                                                            <tr>
//                                                                <td class=""es-p10r"" valign=""top"" align=""center""><a target = ""_blank"" href><img title = ""Facebook"" src=""https://etnfxo.stripocdn.email/content/assets/img/social-icons/circle-white/facebook-circle-white.png"" alt=""Fb"" width=""24"" height=""24""></a></td>
//                                                                <td class=""es-p10r"" valign=""top"" align=""center""><a target = ""_blank"" href><img title = ""Twitter"" src=""https://etnfxo.stripocdn.email/content/assets/img/social-icons/circle-white/twitter-circle-white.png"" alt=""Tw"" width=""24"" height=""24""></a></td>
//                                                                <td class=""es-p10r"" valign=""top"" align=""center""><a target = ""_blank"" href><img title = ""Instagram"" src=""https://etnfxo.stripocdn.email/content/assets/img/social-icons/circle-white/instagram-circle-white.png"" alt=""Inst"" width=""24"" height=""24""></a></td>
//                                                                <td valign = ""top"" align=""center""><a target = ""_blank"" href><img title = ""Linkedin"" src=""https://etnfxo.stripocdn.email/content/assets/img/social-icons/circle-white/linkedin-circle-white.png"" alt=""In"" width=""24"" height=""24""></a></td>
//                                                            </tr>
//                                                        </tbody>
//                                                    </table>
//                                                </td>
//                                            </tr>
//                                        </tbody>
//                                    </table>
//                                </td>
//                            </tr>
//                        </tbody>
//                    </table>
//                </td>
//            </tr>
//        </tbody>
//    </table>
//</td>
//</html>
//</body>";




            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("developer.fatecgram.2@gmail.com"); // De
            mail.To.Add(dto.Email); // Para
            mail.Subject = "Teste API Dotnet"; // Assunto
            mail.IsBodyHtml = true;
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
                smtp.Credentials = new NetworkCredential("developer.fatecgram.2@gmail.com", "!q@wfatecgram1");

                // envia o e-mail
                smtp.Send(mail);
            }
        }
        #endregion

    }
}
