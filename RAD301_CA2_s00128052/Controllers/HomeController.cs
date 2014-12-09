using RAD301_CA2_s00128052.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace RAD301_CA2_s00128052.Controllers
{
    public class HomeController : Controller
    {
        MovieActorDb db = new MovieActorDb();

        public ActionResult Index()
        {
            using (MovieActorDb db = new MovieActorDb())
            {
                var movies = db.Movies.Include(m => m.MovieImage).ToList();
                if (movies != null)
                    return View(movies);
                else
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        //Movie Views

        public PartialViewResult RandomMovie()
        {
            using (MovieActorDb db = new MovieActorDb())
            {
                var movie = db.Movies.Include(m => m.MovieGenre).OrderBy(m => Guid.NewGuid()).First();
                if (movie != null)
                    return PartialView("_RandomMovie", movie);
                else
                    return PartialView("_RandomMovie", null);
            }
        }

        public PartialViewResult AllMovies(string sort)
        {
            using (MovieActorDb db = new MovieActorDb())
            {
                List<Movie> movies;
                switch (sort)
                {
                    case "N":
                        movies = db.Movies.Include(m => m.MovieGenre).ToList();
                        break;
                    case "NC":
                        movies = db.Movies.Include(m => m.MovieGenre).OrderBy(m => m.Characters.Count()).ToList();
                        break;
                    case "G":
                        movies = db.Movies.Include(m => m.MovieGenre).OrderBy(m => m.MovieGenre.GenreName).ToList();
                        break;
                    case "A":
                        movies = db.Movies.Include(m => m.MovieGenre).OrderBy(m => m.MovieTitle).ToList();
                        break;
                    case "RA":
                        movies = db.Movies.Include(m => m.MovieGenre).OrderByDescending(m => m.MovieTitle).ToList();
                        break;
                    default:
                        movies = db.Movies.Include(m => m.MovieGenre).ToList();
                        break;
                }
                if (movies != null)
                    return PartialView("_AllMovies", movies);
                else
                    return PartialView("_AllMovies", null);
            }
        }

        public PartialViewResult OtherMovies(int id)
        {
            using (MovieActorDb db = new MovieActorDb())
            {
                var Characters = db.MovieActors.Include(m => m.Movie).Where(a => a.ActorId == id).ToList();
                if (Characters != null)
                    return PartialView("_OtherMovies", Characters);
                else
                    return PartialView("_OtherMovies", null);
            }
        }

        public PartialViewResult MovieDetails(int id)
        {
            using (MovieActorDb db = new MovieActorDb())
            {
                Movie movie = db.Movies.Include(m => m.MovieGenre).Include(m => m.Characters).Include(m => m.Actors).SingleOrDefault(c => c.MovieId == id);
                if (movie != null)
                    return PartialView("_MovieDetails", movie);
                else
                    return PartialView("_MovieDetails", null);
            }
        }

        //Actor Views

        public PartialViewResult RandomActor()
        {
            using (MovieActorDb db = new MovieActorDb())
            {
                var actor = db.Actors.OrderBy(a => Guid.NewGuid()).First();
                if (actor != null)
                    return PartialView("_RandomActor", actor);
                else
                    return PartialView("_RandomActor", null);
            }
        }

        public PartialViewResult AllActors(string filter)
        {
            using (MovieActorDb db = new MovieActorDb())
            {
                List<Actor> actors;
                switch (filter)
                {
                    case "All":
                        actors = db.Actors.Include(a => a.Characters).ToList();
                        break;
                    case "Male":
                        actors = db.Actors.Include(a => a.Characters).Where(a => a.ActorGender == Gender.Male).ToList();
                        break;
                    case "Female":
                        actors = db.Actors.Include(a => a.Characters).Where(a => a.ActorGender == Gender.Female).ToList();
                        break;
                    default:
                        actors = db.Actors.Include(a => a.Characters).ToList();
                        break;
                }
                if (actors != null)
                    return PartialView("_AllActors", actors);
                else
                    return PartialView("_AllActors", null);
            }
        }

        public PartialViewResult ActorDetails(int id)
        {
            using (MovieActorDb db = new MovieActorDb())
            {
                Actor actor = db.Actors.Include(a => a.Movies).Include(a => a.Characters).SingleOrDefault(a => a.ActorId == id);
                if (actor != null)
                    return PartialView("_ActorDetails", actor);
                else
                {
                    Debug.WriteLine("Actor Not Found");
                    return PartialView("_ActorDetails", null);
                }
            }
        }

        public PartialViewResult OtherActors(int id)
        {
            using (MovieActorDb db = new MovieActorDb())
            {
                var Characters = db.MovieActors.Include(m => m.Actor).Where(a => a.MovieId == id).ToList();
                if (Characters != null)
                    return PartialView("_OtherActors", Characters);
                else
                    return PartialView("_OtherActors", null);
            }
        }

        //CRUD
        //MOVIE CRUD
        public PartialViewResult CreateMovie()
        {
            using (MovieActorDb db = new MovieActorDb())
            {
                ViewBag.Genres = db.Genres.ToList();
                return PartialView("_CreateMovie");
            }
        }

        [HttpPost]
        public HttpStatusCodeResult CreateMovie(Movie IncomingMovie)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (MovieActorDb db = new MovieActorDb())
                    {
                        IncomingMovie.MovieGenre = db.Genres.SingleOrDefault(g => g.GenreId == IncomingMovie.GenreId);
                        db.Movies.Add(IncomingMovie);
                        db.SaveChanges();
                        return new HttpStatusCodeResult(200);
                    }
                }
                else
                    return new HttpStatusCodeResult(400);
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
        }

        public PartialViewResult EditMovie(int id)
        {
            ViewBag.Genres = db.Genres.ToList();
            var movie = db.Movies.SingleOrDefault(m => m.MovieId == id);
            if (movie != null)
                return PartialView("_EditMovie", movie);
            else
                return PartialView("_EditMovie", null);
        }

        [HttpPost]
        public HttpStatusCodeResult EditMovie(Movie EditMovieIncoming)
        {
            if (EditMovieIncoming != null)
            {
                try
                {
                    using (MovieActorDb db = new MovieActorDb())
                    {
                        EditMovieIncoming.MovieGenre = db.Genres.SingleOrDefault(g => g.GenreId == EditMovieIncoming.GenreId);
                        db.Entry(EditMovieIncoming).State = System.Data.EntityState.Modified;
                        db.SaveChanges();
                        return new HttpStatusCodeResult(200);
                    }
                }
                catch
                {
                    return new HttpStatusCodeResult(400);
                }
            }
            else
                return new HttpStatusCodeResult(400);
        }

        public PartialViewResult DeleteMovie(int id)
        {
            var movie = db.Movies.Include(m => m.MovieGenre).SingleOrDefault(m => m.MovieId == id);
            return PartialView("_DeleteMovieConfirm", movie);
        }

        [HttpPost]
        public HttpStatusCodeResult DeleteMovieConfirmed(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (MovieActorDb db = new MovieActorDb())
                    {
                        db.Movies.Remove(db.Movies.SingleOrDefault(m => m.MovieId == id));
                        db.SaveChanges();
                        return new HttpStatusCodeResult(200);
                    }
                }
                else
                    return new HttpStatusCodeResult(400);
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
        }

        //ACTOR CRUD
        public PartialViewResult CreateActor()
        {
            using (MovieActorDb db = new MovieActorDb())
            {
                return PartialView("_CreateActor");
            }
        }

        [HttpPost]
        public HttpStatusCodeResult CreateActor(Actor IncomingActor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (MovieActorDb db = new MovieActorDb())
                    {
                        db.Actors.Add(IncomingActor);
                        db.SaveChanges();
                        return new HttpStatusCodeResult(200);
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(400);
                }
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
        }

        public PartialViewResult EditActor(int id)
        {
            var actor = db.Actors.SingleOrDefault(a => a.ActorId == id);
            if (actor != null)
                return PartialView("_EditActor", actor);
            else
                return PartialView("_EditActor", null);
        }

        [HttpPost]
        public HttpStatusCodeResult EditActor(Actor EditActorIncoming)
        {
            if (EditActorIncoming != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        using (MovieActorDb db = new MovieActorDb())
                        {
                            db.Entry(EditActorIncoming).State = System.Data.EntityState.Modified;
                            db.SaveChanges();
                            return new HttpStatusCodeResult(200);
                        }
                    }
                    else
                        return new HttpStatusCodeResult(400);
                }
                catch
                {
                    return new HttpStatusCodeResult(400);
                }
            }
            else return new HttpStatusCodeResult(400);
        }

        public PartialViewResult DeleteActor(int id)
        {
            var actor = db.Actors.SingleOrDefault(a => a.ActorId == id);
            return PartialView("_DeleteActorConfirm", actor);
        }

        [HttpPost]
        public HttpStatusCodeResult DeleteActorConfirmed(int id)
        {
            try
            {
                using (MovieActorDb db = new MovieActorDb())
                {
                    db.Actors.Remove(db.Actors.SingleOrDefault(a => a.ActorId == id));
                    db.SaveChanges();
                    return new HttpStatusCodeResult(200);
                }
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
        }

        //MOVIEACTOR CRUD
        public PartialViewResult AddActorToMovie(int Id)
        {
            using (MovieActorDb db = new MovieActorDb())
            {
                var IncomingMovie = db.Movies.Include(m => m.MovieGenre).SingleOrDefault(m => m.MovieId == Id);
                if (IncomingMovie != null)
                {
                    ViewBag.ActorsList = db.Actors.ToList();
                    return PartialView("_AddActorToMovie", IncomingMovie);
                }
                else
                    return PartialView("_AddActorToMovie", null);
            }
        }

        public PartialViewResult AddMovieToActor(int Id)
        {
            using (MovieActorDb db = new MovieActorDb())
            {
                var IncomingActor = db.Actors.SingleOrDefault(a => a.ActorId == Id);
                if (IncomingActor != null)
                {
                    ViewBag.MovieList = db.Movies.ToList();
                    return PartialView("_AddMovieToActor", IncomingActor);
                }
                else
                    return PartialView("_AddActorToMovie", null);
            }
        }

        [HttpPost]
        public HttpStatusCodeResult CreateMovieActor(MovieActor IncomingCharacter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (MovieActorDb db = new MovieActorDb())
                    {
                        IncomingCharacter.Actor = db.Actors.SingleOrDefault(a => a.ActorId == IncomingCharacter.ActorId);
                        IncomingCharacter.Movie = db.Movies.SingleOrDefault(m => m.MovieId == IncomingCharacter.MovieId);
                        db.MovieActors.Add(IncomingCharacter);
                        db.SaveChanges();
                        return new HttpStatusCodeResult(200);
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(400);
                }
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
        }

    }//Controller Class
}//Controller Namespace