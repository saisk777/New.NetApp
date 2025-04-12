using System;
using API.DTOs;
using API.Entities;
using AutoMapper.Execution;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUserNameAsync(string username);
    Task<IEnumerable<MemberDto>> GetMembersAync();
    Task<MemberDto?> GetMemberAsync(string username);


}
