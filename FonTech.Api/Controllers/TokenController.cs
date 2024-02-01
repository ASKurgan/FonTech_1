using FonTech.Application.Services;
using FonTech.Domain.Dto;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace FonTech.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenService"></param>
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }


        [HttpPost]
        public async Task<ActionResult<BaseResult<TokenDto>>> RefreshToken([FromBody] TokenDto tokenDto)
        {
            var response = await _tokenService.RefreshToken(tokenDto);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
