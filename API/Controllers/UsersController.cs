using System;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

///api/users
[Authorize]
public class UsersController(IUserRepository userRepository,IMapper mapper) : BaseApiController // here we are using the dpendency injection for utilise the db context or connection string form the program.cs file using the constructors it's an best example
{
    [HttpGet]
    public async Task< ActionResult<IEnumerable<MemberDto>>>GetUsers() // using member DTO insted of app user suing the auto mapper to map the fields
    {
        var users = await userRepository.GetMembersAync();

        return Ok(users);
    }

    [HttpGet("{username}")] //api/users/id
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await userRepository.GetMemberAsync(username);

        if( user == null) return NotFound();

        return user;
    }



}
