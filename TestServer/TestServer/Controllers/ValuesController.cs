using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TestServer.Models;

namespace TestServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly JwtSetting _jwtSetting;
        public ValuesController(IOptions<JwtSetting> option)
        {
            _jwtSetting = option.Value;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            //创建用户身份标识，可按需要添加更多信息,注 此地方为测试，使用时从数据库等地方读取
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", "111", ClaimValueTypes.Integer32), // 用户id
                new Claim("name", "fxy"), // 用户名
                new Claim("sex", "男") // 性别
            };

            //创建令牌
            var token = new JwtSecurityToken(
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
                signingCredentials: _jwtSetting.Credentials,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddSeconds(_jwtSetting.ExpireSeconds)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new
            {
                Status = true,
                Token = jwtToken,
                Type = "Bearer"
            };

            return JsonConvert.SerializeObject(response);
        }
        
    }
}
