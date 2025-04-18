using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
public class AccountController(DataContext context,ITokenService tokenService) : BaseApiController
{

    [HttpPost("register")]//account//register
    public async Task<ActionResult<UserDto>>Register(RegisterDto registerDto)
    {
        if (await UserExixts(registerDto.username)) return BadRequest("Usernmae is already Exists");
        return Ok();
        // using var hmac = new HMACSHA512();
        // var user = new AppUser 
        // {
        //     UserName = registerDto.username.ToLower(),
        //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
        //     PasswordSalt = hmac.Key

        // };
        // context.Users.Add(user);
        // await context.SaveChangesAsync();
    
        // return new UserDto
        // {
        //     username = user.UserName,
        //     Token = tokenService.CreateToken(user)
 
        // };

        
    }

    [HttpPost("login")] 
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => 
            x.UserName == loginDto.username.ToLower() );

        if (user == null) return Unauthorized("Invalid Username");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));
        for (int i = 0; i < ComputeHash.Length; i++)
        {
            if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            
        }
        return new UserDto{
            username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }
    private async Task<bool> UserExixts( string username)
    {
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower()); // this chcek user name exixt or not 
    }

}
