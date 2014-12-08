using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RAD301_CA2_s00128052.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        [Required, Display(Name = "Movie Title")]
        [StringLength(50, ErrorMessage = "Must be between {2} and {1} characters long", MinimumLength = 2)]
        public string MovieTitle { get; set; }

        [Display(Name = "Rating"), Range(1, 100), Required]
        public double MovieRating { get; set; }

        [Display(Name = "Release Date"), DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime MovieReleaseDate { get; set; }

        [Display(Name = "Run Time"), Range(0, int.MaxValue), Required]
        public int MovieLength { get; set; }

        [Display(Name = "Genre"), Required]
        public int GenreId { get; set; }

        public Genre MovieGenre { get; set; }

        [Display(Name = "Director"), Required]
        [StringLength(50, ErrorMessage = "Must be between {2} and {1} characters long", MinimumLength = 2)]
        public string MovieDirector { get; set; }

        [StringLength(200, ErrorMessage = "Must be between {2} and {1} characters long", MinimumLength = 2)]
        [Display(Name = "Description"), Required]
        public string MovieDescription { get; set; }

        [Required]
        public string MoviePosterUrl { get; set; }

        public List<MovieImage> MovieImage { get; set; }
        public List<MovieActor> Characters { get; set; }
        public List<Actor> Actors
        {
            get
            {
                return (Characters == null ? null : Characters.Select(c => c.Actor).ToList());
            }
        }
    }

    public enum Gender { Male, Female }

    public class Actor
    {
        [Key]
        public int ActorId { get; set; }

        [Required, Display(Name = "First Name")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Must be between {2} and {1} characters long")]
        public string ActorFirstName { get; set; }

        [Required, Display(Name = "Surname")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Must be between {2} and {1} characters long")]
        public string ActorSurname { get; set; }

        [Display(Name = "Name")]
        public string ActorFullName { get { return ActorFirstName + " " + ActorSurname; } }

        [Display(Name = "Date of Birth"), Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ActorDOB { get; set; }

        [Display(Name = "Description"), Required]
        [StringLength(200, ErrorMessage = "Must be between {2} and {1} characters long", MinimumLength = 2)]
        public string ActorDescription { get; set; }

        [Display(Name = "Gender"), Required, EnumDataType(typeof(Gender))]
        public Gender ActorGender { get; set; }

        [Required]
        public string ActorImageUrl { get; set; }

        public List<MovieActor> Characters { get; set; }
        public List<Movie> Movies
        {
            get
            {
                return (Characters == null ? null : Characters.Select(c => c.Movie).ToList());
            }
        }
    }

    public class MovieActor
    {
        [Key, Column(Order = 0)]
        public int ActorId { get; set; }
        [Key, Column(Order = 1)]
        public int MovieId { get; set; }

        [Required, Display(Name = "Screen Name")]
        [StringLength(50, ErrorMessage = "Must be between {2} and {1} characters long", MinimumLength = 2)]
        public string ActorScreenName { get; set; }
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }
    }

    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        [Required, StringLength(20, MinimumLength = 2, ErrorMessage = "Must be between {2} and {1} characters long")]
        public string GenreName { get; set; }
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Must be between {2} and {1} characters long")]
        public string GenreDescription { get; set; }
    }

    public class MovieImage
    {
        [Key]
        public int ImageId { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public Movie Movie { get; set; }
    }

    public class MovieActorDb : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieImage> MovieImages { get; set; }
        public MovieActorDb() : base("MoviesActorsDb") { }
    }

    public class MovieActorDbInitialiser : DropCreateDatabaseAlways<MovieActorDb>
    {
        protected override void Seed(MovieActorDb context)
        {
            List<Genre> genres = new List<Genre>(){
                new Genre { GenreName = "Horror", GenreDescription = "Scary Films" },
                new Genre { GenreName = "Thriller", GenreDescription = "Thrilling Films" },
                new Genre { GenreName = "Sport", GenreDescription = "Sporty Films" },
                new Genre { GenreName = "Action", GenreDescription = "Action Films" },
                new Genre { GenreName = "Adventure", GenreDescription = "Adventure Films" },
                new Genre { GenreName = "Animation", GenreDescription = "Animated Films" },
                new Genre { GenreName = "Comedy", GenreDescription = "Comedic Films" },
                new Genre { GenreName = "Drama", GenreDescription = "Dramatic Films" }
            };

            List<Actor> actors = new List<Actor>()
            {
                new Actor{ 
                    ActorFirstName="Mark", ActorSurname="Wahlberg", ActorDOB=DateTime.Parse("05/06/1971"), ActorGender=Gender.Male, 
                    ActorDescription="Hello",
                    ActorImageUrl=@"~\Images\Actors\MarkWahlberg\MarkWahlberg.jpg"
                },
                new Actor{ 
                    ActorFirstName="Dwayne ", ActorSurname="Johnson", ActorDOB=DateTime.Parse("02/05/1972"), ActorGender=Gender.Male, 
                    ActorDescription="Hello",
                    ActorImageUrl=@"~\Images\Actors\DwayneJohnson\DwayneJohnson.jpg"
                },
                new Actor{ 
                    ActorFirstName="Jonah", ActorSurname="Hill", ActorDOB=DateTime.Parse("20/12/1983"), ActorGender=Gender.Male, 
                    ActorDescription="Hello",
                    ActorImageUrl=@"~\Images\Actors\JonahHill\JonahHill.jpg"
                },
                new Actor{ 
                    ActorFirstName="Jennifer", ActorSurname="Lawrence", ActorDOB=DateTime.Parse("15/08/1990"), ActorGender=Gender.Female, 
                    ActorDescription="Hello",
                    ActorImageUrl=@"~\Images\Actors\JenniferLawrence\JenniferLawrence.jpg"
                },
                new Actor{ 
                    ActorFirstName="Emma", ActorSurname="Stone", ActorDOB=DateTime.Parse("06/11/1988"), ActorGender=Gender.Female, 
                    ActorDescription="Hello",
                    ActorImageUrl=@"~\Images\Actors\EmmaStone\EmmaStone.jpg"
                },
                new Actor{ 
                    ActorFirstName="Andrew", ActorSurname="Garfield", ActorDOB=DateTime.Parse("20/08/1983"), ActorGender=Gender.Male, 
                    ActorDescription="Hello",
                    ActorImageUrl=@"~\Images\Actors\AndrewGarfield\AndrewGarfield.jpg"
                },
                new Actor{ 
                    ActorFirstName="Jesse", ActorSurname="Eisenberg", ActorDOB=DateTime.Parse("05/10/1983"), ActorGender=Gender.Male, 
                    ActorDescription="Hello",
                    ActorImageUrl=@"~\Images\Actors\JesseEisenberg\JesseEisenberg.jpg"
                },
                new Actor{ 
                    ActorFirstName="Woody", ActorSurname="Harrelson", ActorDOB=DateTime.Parse("23/07/1961"), ActorGender=Gender.Male, 
                    ActorDescription="Hello",
                    ActorImageUrl=@"~\Images\Actors\WoodyHarrelson\WoodyHarrelson.jpg"
                },
                new Actor{ 
                    ActorFirstName="Jason", ActorSurname="Bateman", ActorDOB=DateTime.Parse("14/01/1969"), ActorGender=Gender.Male, 
                    ActorDescription="Hello",
                    ActorImageUrl=@"~\Images\Actors\JasonBateman\JasonBateman.jpg"
                },
                new Actor{ 
                    ActorFirstName="Jennifer", ActorSurname="Aniston", ActorDOB=DateTime.Parse("11/02/1969"), ActorGender=Gender.Female, 
                    ActorDescription="Hello",
                    ActorImageUrl=@"~\Images\Actors\JenniferAniston\JenniferAniston.jpg"
                }
            };

            List<Movie> movies = new List<Movie>(){
                new Movie{
                    MovieTitle="Pain and Gain", 
                    MovieDirector="Michael Bay", 
                    MovieLength=129, 
                    MovieRating=65, 
                    MovieReleaseDate=DateTime.Parse("30/08/2013"), 
                    MovieGenre= genres[3],
                    MovieDescription="Hello",
                    MovieImage=new List<MovieImage>(){
                        new MovieImage(){ImageUrl=@"~\Images\Movies\PainandGain\PainandGain1.jpg"},
                        new MovieImage(){ImageUrl=@"~\Images\Movies\PainandGain\PainandGain2.jpg"},
                        new MovieImage(){ImageUrl=@"~\Images\Movies\PainandGain\PainandGain3.jpg"}
                    },
                    MoviePosterUrl = @"~\Images\Movies\PainandGain\PainandGainPoster.jpg"
                },
                new Movie{
                    MovieTitle="Superbad", 
                    MovieDirector="Greg Mottola", 
                    MovieLength=113, 
                    MovieRating=77, 
                    MovieReleaseDate=DateTime.Parse("14/09/2007"), 
                    MovieGenre= genres[6],
                    MovieDescription="Hello",
                    MovieImage=new List<MovieImage>(){
                        new MovieImage(){ImageUrl=@"~\Images\Movies\Superbad\Superbad1.jpg"},
                        new MovieImage(){ImageUrl=@"~\Images\Movies\Superbad\Superbad2.jpg"},
                        new MovieImage(){ImageUrl=@"~\Images\Movies\Superbad\Superbad3.jpg"}
                    },
                    MoviePosterUrl = @"~\Images\Movies\Superbad\SuperbadPoster.jpg"
                },
                new Movie{
                    MovieTitle="The Hunger Games", 
                    MovieDirector="Garry Ross", 
                    MovieLength=142, 
                    MovieRating=73, 
                    MovieReleaseDate=DateTime.Parse("23/03/2012"), 
                    MovieGenre= genres[4],
                    MovieDescription="Hello",
                    MovieImage=new List<MovieImage>(){
                        new MovieImage(){ImageUrl=@"~\Images\Movies\TheHungerGames\TheHungerGames1.jpg"},
                        new MovieImage(){ImageUrl=@"~\Images\Movies\TheHungerGames\TheHungerGames2.jpg"},
                        new MovieImage(){ImageUrl=@"~\Images\Movies\TheHungerGames\TheHungerGames3.jpg"}
                    },
                    MoviePosterUrl = @"~\Images\Movies\TheHungerGames\TheHungerGamesPoster.jpg"
                },
                new Movie{
                    MovieTitle="The Amazing Spiderman", 
                    MovieDirector="Marc Webb", 
                    MovieLength=136, 
                    MovieRating=71, 
                    MovieReleaseDate=DateTime.Parse("03/07/2012"), 
                    MovieGenre= genres[3],
                    MovieDescription="Hello",
                    MovieImage=new List<MovieImage>(){
                        new MovieImage(){ImageUrl=@"~\Images\Movies\TheAmazingSpiderman\AmazingSpiderman1.jpeg"},
                        new MovieImage(){ImageUrl=@"~\Images\Movies\TheAmazingSpiderman\AmazingSpiderman2.jpg"},
                        new MovieImage(){ImageUrl=@"~\Images\Movies\TheAmazingSpiderman\AmazingSpiderman3.jpg"}
                    },
                    MoviePosterUrl = @"~\Images\Movies\TheAmazingSpiderman\AmazingSpidermanPoster.jpg"
                },
                new Movie{
                    MovieTitle="Horrible Bosses 2", 
                    MovieDirector="Sean Anders", 
                    MovieLength=108, 
                    MovieRating=67, 
                    MovieReleaseDate=DateTime.Parse("28/11/2014"), 
                    MovieGenre= genres[6],
                    MovieDescription="Hello",
                    MovieImage=new List<MovieImage>(){
                        new MovieImage(){ImageUrl=@"~\Images\Movies\HorribleBosses2\HorribleBosses21.jpg"},
                        new MovieImage(){ImageUrl=@"~\Images\Movies\HorribleBosses2\HorribleBosses22.jpg"},
                        new MovieImage(){ImageUrl=@"~\Images\Movies\HorribleBosses2\HorribleBosses23.jpg"}
                    },
                    MoviePosterUrl = @"~\Images\Movies\HorribleBosses2\HorribleBosses2Poster.jpg"
                },
                new Movie{
                    MovieTitle="Social Network", 
                    MovieDirector="David Fincher", 
                    MovieLength=120, 
                    MovieRating=78, 
                    MovieReleaseDate=DateTime.Parse("15/10/2010"), 
                    MovieGenre= genres[7],
                    MovieDescription="Hello",
                    MovieImage=new List<MovieImage>(){
                        new MovieImage(){ImageUrl=@"~\Images\Movies\TheSocialNetwork\SocialNetwork1.jpg"},
                        new MovieImage(){ImageUrl=@"~\Images\Movies\TheSocialNetwork\SocialNetwork2.png"},
                        new MovieImage(){ImageUrl=@"~\Images\Movies\TheSocialNetwork\SocialNetwork3.jpg"}
                    },
                    MoviePosterUrl = @"~\Images\Movies\TheSocialNetwork\SocialNetworkPoster.jpg"
                },
            };

            List<MovieActor> characters = new List<MovieActor>()
            {
                new MovieActor{Actor=actors[0], Movie=movies[0], ActorScreenName="Daniel Lugo"},
                new MovieActor{Actor=actors[1], Movie=movies[0], ActorScreenName="Paul Doyle"},
                new MovieActor{Actor=actors[2], Movie=movies[1], ActorScreenName="Seth"},
                new MovieActor{Actor=actors[4], Movie=movies[1], ActorScreenName="Jules"},
                new MovieActor{Actor=actors[7], Movie=movies[2], ActorScreenName="Haymitch Abernathy"},
                new MovieActor{Actor=actors[3], Movie=movies[2], ActorScreenName="Katniss Everdeen"},
                new MovieActor{Actor=actors[5], Movie=movies[3], ActorScreenName="Peter Parker"},
                new MovieActor{Actor=actors[4], Movie=movies[3], ActorScreenName="Gwen Stacey"},
                new MovieActor{Actor=actors[8], Movie=movies[4], ActorScreenName="Nick Hendricks"},
                new MovieActor{Actor=actors[9], Movie=movies[4], ActorScreenName="Dr. Julia Harris"},
                new MovieActor{Actor=actors[6], Movie=movies[5], ActorScreenName="Mark Zuckerberg"},
                new MovieActor{Actor=actors[5], Movie=movies[5], ActorScreenName="Eduardo Saverin"},
            };

            genres.ForEach(g => context.Genres.Add(g));
            actors.ForEach(a => context.Actors.Add(a));
            movies.ForEach(m => context.Movies.Add(m));
            characters.ForEach(c => context.MovieActors.Add(c));

            context.SaveChanges();
            base.Seed(context);
        }
    }
}