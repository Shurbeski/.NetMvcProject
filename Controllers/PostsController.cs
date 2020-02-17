using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Journal.Models;
using Microsoft.AspNet.Identity;

namespace Journal.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.OrderByDescending(p=> p.date).ToList());
        }
        public ActionResult AjaxDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public string AjaxLike(int id)
        {
            Post post = db.Posts.FirstOrDefault(x => x.Id == id);
            List<Liked> postLikes = db.Likes.Where(x => x.PostId == id).ToList();
            var userId = User.Identity.GetUserName().ToString();
            bool deleted = false;

            foreach(Liked likes in postLikes)
            {
                if (likes.User.Equals(userId))
                {
                    post.LikeCount--;
                    db.Likes.Remove(likes);
                    deleted = true;
                }

            }
            if (!deleted)
            {
                post.LikeCount++;
                db.Likes.Add(new Liked()
                {
                    PostId = post.Id,
                    User = userId
                });
            }

            db.SaveChanges();

            return post.LikeCount.ToString();
        }

        public string AjaxComment(int id, string comment)
        {
            Post post = db.Posts.FirstOrDefault(x => x.Id == id);

            db.Comments.Add(new Comment { PostId = id, Content = comment, User = User.Identity.GetUserName() });
            db.SaveChanges();

            post.Comments.Add(comment);

            return comment;
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            var comments = db.Comments.Where(x => x.PostId == post.Id).ToList();

            post.Comments = comments.Select(x => x.Content).ToList();

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }
        [Authorize(Roles ="Publisher")]
        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Publisher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,ImgUrl")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.PublisherId = User.Identity.GetUserName().ToString();
                post.date = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        [Authorize(Roles = "Publisher")]
        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);


            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Publisher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,ImgUrl")] Post post)
        {
            if (ModelState.IsValid)
            {
                var Post = db.Posts.First(p => p.Id == post.Id);
                Post.Title = post.Title;
                Post.Content = post.Content;
                Post.ImgUrl = post.ImgUrl;
                Post.date = DateTime.Now;
                
                
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }
        [Authorize(Roles = "Publisher")]
        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }
        [Authorize(Roles = "Publisher")]
        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
