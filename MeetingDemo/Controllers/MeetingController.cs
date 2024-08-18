using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetingDemo.Data;
using MeetingDemo.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class MeetingController : ControllerBase
{
	private readonly ApplicationDbContext _context;

	public MeetingController(ApplicationDbContext context)
	{
		_context = context;
	}

	
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Meeting>>> GetMeetings()
	{
		return await _context.Meetings.Include(m => m.User).ToListAsync();
	}

	
	[HttpGet("{id}")]
	public async Task<ActionResult<Meeting>> GetMeeting(int id)
	{
		var meeting = await _context.Meetings.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);

		if (meeting == null)
		{
			return NotFound();
		}

		return meeting;
	}


	[HttpPut("{id}")]
	public async Task<IActionResult> PutMeeting(int id, Meeting meeting)
	{
		if (id != meeting.Id)
		{
			return BadRequest();
		}

		_context.Entry(meeting).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!MeetingExists(id))
			{
				return NotFound();
			}
			else
			{
				throw;
			}
		}

		return NoContent();
	}


	[HttpPost]
	public async Task<ActionResult<Meeting>> PostMeeting(Meeting meeting)
	{
		_context.Meetings.Add(meeting);
		await _context.SaveChangesAsync();

		return CreatedAtAction("GetMeeting", new { id = meeting.Id }, meeting);
	}

	
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteMeeting(int id)
	{
		var meeting = await _context.Meetings.FindAsync(id);
		if (meeting == null)
		{
			return NotFound();
		}

		_context.Meetings.Remove(meeting);
		await _context.SaveChangesAsync();

		return NoContent();
	}

	private bool MeetingExists(int id)
	{
		return _context.Meetings.Any(e => e.Id == id);
	}
}
