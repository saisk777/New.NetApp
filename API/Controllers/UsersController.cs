using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
///api/users
public class UsersController(DataContext context) : BaseApiController // here we are using the dpendency injection for utilise the db context or connection string form the program.cs file using the constructors it's an best example
{
    [AllowAnonymous]
    [HttpGet]
    public async Task< ActionResult<IEnumerable<AppUser>>>GetUsers()
    {
        var users = await context.Users.ToListAsync();

        return users;
    }
    [Authorize]
    [HttpGet("{id:int}")] //api/users/id
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if( user == null) return NotFound();
        return user;
    }



}
