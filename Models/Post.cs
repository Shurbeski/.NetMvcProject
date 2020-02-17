using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Journal.Models
{
	public class Post
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string PublisherId { get; set; }
		[DefaultValue(0)]
		public DateTime date { get; set; }
		[DefaultValue("")]
		public string ImgUrl { get; set; }
		[DefaultValue(0)]
		public int LikeCount { get; set; }
		public List<string> Liked { get; set; }
		public List<string> Comments { get; set; }

		public Post()
		{
			Liked = new List<string>();
			Comments = new List<string>();
		}
	}
}