using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using curso.api.Models.Usuarios;
using curso.api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using curso.api.Filters;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace curso.api.Controllers
{

    [SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", Type = typeof(LoginViewModelInput))]
    [SwaggerResponse(statusCode: 400, description: "Campos necessários!", Type = typeof(ValidaCampoViewModelOutput))]
    [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenericoViewModel))]
    [Route("api/v1/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpPost]
        [Route("logar")]
        [ValidaModelStateCustomizado]
        public IActionResult Logar(LoginViewModelInput loginViewModelInput)
        {
            /*            if(!ModelState.IsValid)
                        {
                            return BadRequest(new ValidaCampoViewModelOutput(ModelState.SelectMany(sm => sm.Value.Errors).Select(s => s.ErrorMessage)));
                        }*/

            var usuarioViewModelOutput = new UsuarioViewModelOutput()
            {
                Codigo = 1,
                Login = "testeqq",
                Email = "senha@email.com"
            };

            var secret = Encoding.ASCII.GetBytes("weqwkjhekjhiuyiuyJHAKSdhkajhsdKH12");
            var symmetricSecurityKey = new SymmetricSecurityKey(secret);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {

                    new Claim(ClaimTypes.NameIdentifier, usuarioViewModelOutput.Codigo.ToString()),
                    new Claim(ClaimTypes.Name, usuarioViewModelOutput.Login.ToString()),
                    new Claim(ClaimTypes.Email, usuarioViewModelOutput.Email.ToString())

                }),

                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
        };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenGenereted = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(tokenGenereted);


            return Ok(new 
            {
                Token = token,
               Usuario = usuarioViewModelOutput

            });
        }

        [HttpPost]
        [Route("registrar")]
        [ValidaModelStateCustomizado]
        public IActionResult Registrar(LoginViewModelInput loginViewModelInput)
        {
            return Created("", loginViewModelInput);
        }
    }
}
