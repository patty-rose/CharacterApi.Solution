using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharacterApi.Models;
using System.ComponentModel.DataAnnotations;

namespace CharacterApi.Controllers
{
  [Route("api/Main")]
  [ApiController]
  public class MainController : Controller
  {
    private readonly CharacterApiContext _context;

    public MainController(CharacterApiContext context)
      {
          _context = context;
      }
    [HttpGet("gettoken")]
      public Object GetToken()
      {
          string key = "password"; //Secret key which will be used later during validation
          var issuer = "http://localhost:5004";  //normally this will be your site URL

          var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
          var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

          //Create a List of Claims, Keep claims name short
          var permClaims = new List<Claim>();
          permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
          permClaims.Add(new Claim("valid", "1"));
          permClaims.Add(new Claim("userid", "1"));
          permClaims.Add(new Claim("name", "bilal"));

          //Create Security Token object by giving required parameters
          var token = new JwtSecurityToken(issuer, //Issuer
                          issuer,  //Audience
                          permClaims,
                          expires: DateTime.Now.AddDays(1),
                          signingCredentials: credentials);
          var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
          return new { data = jwt_token };
      }

    [HttpPost("getname1")]
      public String GetName1() {
      if (User.Identity.IsAuthenticated) {
        var identity = User.Identity as ClaimsIdentity;
        if (identity != null) {
        IEnumerable < Claim > claims = identity.Claims;
        }
        return "Valid";
      } else {
        return "Invalid";
      }
      }

      [Authorize]
      [HttpPost("getname2")]
      public Object GetName2() {
      var identity = User.Identity as ClaimsIdentity;
      if (identity != null) {
        IEnumerable < Claim > claims = identity.Claims;
        var name = claims.Where(p => p.Type == "name").FirstOrDefault() ? .Value;
        return new {
        data = name
        };

      }
      return null;
      }
  }
}


