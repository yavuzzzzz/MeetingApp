﻿namespace MeetingDemo.Models
{

	public class Meeting
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Description { get; set; }
		public string Documents { get; set; }

		public int UserId { get; set; }

		public User User { get; set; } 
	}
}
