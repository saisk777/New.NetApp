using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

///api/users
[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseApiController // here we are using the dpendency injection for utilise the db context or connection string form the program.cs file using the constructors it's an best example
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() // using member DTO insted of app user suing the auto mapper to map the fields
    {
        var users = await userRepository.GetMembersAync();

        return Ok(users);
    }

    [HttpGet("{username}")] //api/users/id
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await userRepository.GetMemberAsync(username);

        if (user == null) return NotFound();

        return user;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // from the token finding the user name to check who was logged in

        // if(username == null) return BadRequest("Username not found");

        var user = await userRepository.GetUserByUserNameAsync(User.GetUsername()); //sending this username to the user details

        if (user == null) return BadRequest("Could not find user");

        mapper.Map(memberUpdateDto, user); // useing the mapper to map the vaslues of user

        if (await userRepository.SaveAllAsync()) return NoContent(); //saving the all the saving all the updated values

        return BadRequest("failed to Upadte the User");
    }

    [HttpPost("add-photo")]
    [AllowAnonymous]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUserByUserNameAsync(User.GetUsername());
        if (user == null) return BadRequest("Cannot update user");

        var result = await photoService.AddPhotoAsync(file);

        // if (result.Error != null) return BadRequest(result.Error.Message);
        var photo = new Photo
        {
            Url = result.Url,
            publicId = result.BlobName

        };
        user.Photos.Add(photo);
        if (await userRepository.SaveAllAsync())
            return CreatedAtAction(nameof(GetUser),
               new { username = user.UserName }, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problem adding photo");

    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUserByUserNameAsync(User.GetUsername());
        if (user == null) return BadRequest("Cannot update user");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("This is already your main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to set main photo");
    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await userRepository.GetUserByUserNameAsync(User.GetUsername());
        if (user == null) return BadRequest("Could not find user");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("You cannot delete your main photo");

        if (photo.publicId != null)
        {
            var result = await photoService.DeletePhotoAsync(photo.publicId);
            if (!result.IsDeleted) return BadRequest("Failed to delete photo from storage");
        }

        user.Photos.Remove(photo);

        if (await userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Failed to delete photo");
    }
}
