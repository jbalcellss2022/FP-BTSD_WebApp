﻿namespace Entities.DTOs
{
	public class UserDTO
	{
		public string? Username { get; set; }
		public bool KeepSigned { get; set; }
		public bool IsAdmin { get; set; }
	}
}