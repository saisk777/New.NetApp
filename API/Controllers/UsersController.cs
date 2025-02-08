using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")] ///api/users
public class UsersController(DataContext context) : ControllerBase // here we are using the dpendency injection for utilise the db context or connection string form the program.cs file using theconstructors it's an best example
{

    [HttpGet]
    public async Task< ActionResult<IEnumerable<AppUser>>>GetUsers()
    {
        var users = await context.Users.ToListAsync();

        return users;
    }

    [HttpGet("{id:int}")] //api/users/id
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if( user == null) return NotFound();
        return user;
    }



}
