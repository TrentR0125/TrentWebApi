using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrentWebApi.Data;
using TrentWebApi.Models;

namespace TrentWebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly ApiDbContext _apiDb;

        public MemberController(ApiDbContext apiDb)
        {
            _apiDb = apiDb;
        }

        // i typically use Json to return stuff but im too lazy to find the package that has it

        [HttpPut("UpdateMember/{MemberId}")]
        public async Task<IActionResult> UpdateMember([FromRoute] int MemberId, [FromBody] Member member)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                Member chosen = await _apiDb.Members
                    .Where(m => m.MemberId == MemberId)
                    .FirstOrDefaultAsync();

                if (chosen != null)
                {
                    chosen.MemberId = member.MemberId;
                    chosen.Name = member.Name;
                    chosen.IsAdmin = member.IsAdmin;

                    _apiDb.Members.Update(chosen);
                    await _apiDb.SaveChangesAsync();

                    return Ok($"{chosen.Name} has been updated.");
                }
                else
                {
                    return NotFound("There is no member with that Id.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal Server error: {ex.Message}");
            }
        }

        [HttpPost("CreateMember")]
        public async Task<IActionResult> CreateMember([FromBody] Member member)
        {
            try
            {
                Member exists = await _apiDb.Members
                    .Where(m => m == member)
                    .FirstOrDefaultAsync();

                if (exists == null)
                {
                    _apiDb.Members.Add(member);
                    await _apiDb.SaveChangesAsync();

                    return Ok("Member has been Created.");
                }
                else
                {
                    return BadRequest("There is already a Member with that exact information.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteMember/{MemberId}")]
        public async Task<IActionResult> DeleteMember([FromRoute] int MemberId)
        {
            try
            {
                Member member = await _apiDb.Members
                    .Where(m => m.MemberId == MemberId)
                    .FirstOrDefaultAsync();

                if (member != null)
                {
                    _apiDb.Remove(member);
                    await _apiDb.SaveChangesAsync();

                    return Ok("Member has been Deleted.");
                }
                else
                {
                    return NotFound("Could not find a Member with that Id.");
                }
            }
            catch (Exception ex) 
            {
                return BadRequest($"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("GetMemberById/{MemberId}")]
        public async Task<IActionResult> GetMemberById([FromRoute] int MemberId)
        {
            try
            {
                Member member = await _apiDb.Members
                    .Where(m => m.MemberId == MemberId)
                    .FirstOrDefaultAsync();

                if (member != null)
                {
                    return Ok(member);
                }
                else
                {
                    return NotFound("Could not find a Member with that Id.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Internal Server Error: {ex.Message}");
            }
        }
    }
}
