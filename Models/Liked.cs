using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Journal.Models
{
	public class Liked
	{
		public int Id { get; set; }
		public int PostId { get; set; }
		public string User { get; set; }
	}
}