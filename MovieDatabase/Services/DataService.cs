using MovieDatabase.DataModels;
using Microsoft.Extensions.Logging;
using MovieDatabase.Context;


namespace MovieDatabase.Services
{

    public class DataService : IDataService
    {
        private readonly ILogger<IDataService> _logger;

        public DataService(ILogger<IDataService> logger)
        {
            _logger = logger;
        }


        public void DisplayMovies()
        {
            Console.WriteLine("How many movies do you want to see? Or a) for all movies ");
            try
            {
                var value = Console.ReadLine().ToLower();
                using (var context = new MovieContext())
                {
                    if (value != "a")
                    {
                        var numMovies = 0;
                        while (!int.TryParse(value, out numMovies))
                        {
                            Console.WriteLine("Must be a number! How many movies do you want to see?");
                            value = Console.ReadLine();
                        }
                        List<Movie> movies = context.Movies.Take(numMovies).ToList();
                        foreach (var movie in movies)
                        {
                            Console.WriteLine($"{movie.Id}  {movie.Title}  {movie.ReleaseDate}");
                        }
                    }
                    else
                    {
                        List<Movie> movies = context.Movies.ToList();
                        foreach (var movie in movies)
                        {
                            Console.WriteLine($"{movie.Id}  {movie.Title}  {movie.ReleaseDate}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid Input");

            }
        }

        public void Search()
        {
            Console.WriteLine("Enter search string: ");
            var searchString = Console.ReadLine().ToLower();
            using (var context = new MovieContext())
            {
                var movieQuery = (from movie in context.Movies
                    where movie.Title.ToLower().Contains(searchString)
                    select movie).Distinct().ToList();
                var genreQuery = (from genre in context.Genres
                    join movieGenre in context.MovieGenres
                        on genre.Id equals movieGenre.Genre.Id
                    join movie in context.Movies on movieGenre.Movie.Id equals movie.Id
                    where movie.Title.ToLower().Contains(searchString)
                    select genre.Name).Distinct().ToList();

                foreach (var movie in movieQuery)
                {
                    Console.Write($"Id:{movie.Id} Title:{movie.Title} Genres:");
                    foreach (var genre in genreQuery)
                    {
                        Console.Write(genre + "|");
                        Console.WriteLine();
                    }
                }
            }
        }

        public void AddMovie()
        {
            var movie = new Movie();
            Console.WriteLine("Enter Movie Title: ");
            var title = Console.ReadLine();
            Console.WriteLine("Enter Release Date: ");
            var releaseDate = Console.ReadLine();

            movie.Title = title;
            movie.ReleaseDate = DateTime.Parse(releaseDate);

            using (var context = new MovieContext())
            {
                context.Movies.Add(movie);
                context.SaveChanges();
            }
            _logger.LogInformation(message: $"Added {movie.Title} to database");
        }
        
        public void Update()
        {
            Console.WriteLine("Enter ID of movie to update: ");
            var searchId = 0; 
            while(!int.TryParse(Console.ReadLine(), out searchId))
            {
                Console.WriteLine("Id must be a number! Enter valid Id: ");
            }

            using (var context = new MovieContext())
            {
                var movieQuery = (from movie in context.Movies
                    where movie.Id == searchId
                    select movie).FirstOrDefault();
                Console.WriteLine($"Updating {movieQuery.Title}");

                if (movieQuery != null)
                {
                    Console.WriteLine("Do you want to update movie title (y)?");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        Console.WriteLine($"Title is {movieQuery.Title} Enter new title: ");
                        movieQuery.Title = Console.ReadLine();
                        _logger.LogInformation(message:$"Updated {movieQuery.Title} in database");
                    }

                    Console.WriteLine("Do you want to update release date (y)?");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        var newReleaseDate = DateTime.Now;
                        Console.WriteLine($"Release date is {movieQuery.ReleaseDate} - Enter new release date (mm/dd/yyy): ");
                        while (!DateTime.TryParse(Console.ReadLine(), out newReleaseDate))
                        {
                            Console.WriteLine("Not a valid date! Enter new release date (mm/dd/yyyy)");
                        }
                        movieQuery.ReleaseDate = newReleaseDate;
                        _logger.LogInformation(message:$"Updated release date of {movieQuery.Title} in database");
                        
                    }

                    Console.WriteLine("Do you want to update movie genres? (y)");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        var genreQuery = (from genre in context.Genres
                            join movieGenre in context.MovieGenres
                                on genre.Id equals movieGenre.Genre.Id
                            join movie in context.Movies on movieGenre.Movie.Id equals movie.Id
                            where movie.Id == searchId
                            select genre.Name).Distinct().ToList();
                        Console.WriteLine("Genres are : ");
                        foreach (var g in genreQuery)
                        {
                            Console.Write(g + "|");
                            Console.WriteLine();
                        }

                        Console.WriteLine("Do you want to (a) add or (r) remove genre?");
                        var choice = Console.ReadLine().ToLower();
                        if (choice == "a")
                        {
                            Console.WriteLine($"Possible genres are: ");
                            foreach (var genre in context.Genres)
                            {
                                Console.WriteLine($"{genre.Id} {genre.Name}");
                            }

                            Console.WriteLine("Enter genreId to add: ");
                            var gId = 0;
                            while (!int.TryParse(Console.ReadLine(), out gId))
                            {
                                Console.WriteLine("Id must be a number! Enter valid Id: ");
                            }
                            if (gId < 1 || gId > 18)
                            {
                                Console.WriteLine("Not a valid Id! Enter valid Id: ");
                            }
                            else
                            {
                                var addGenre = (from genre in context.Genres
                                    join movieGenre in context.MovieGenres on genre.Id equals movieGenre.Genre.Id
                                    where movieGenre.Genre.Id == gId
                                    select movieGenre).FirstOrDefault();
                                movieQuery.MovieGenres.Add(addGenre);
                                _logger.LogInformation(message: $"Added {addGenre.Genre.Name} to {movieQuery.Title}");
                            }
                        }
                        else if (choice == "r")
                        {
                            Console.WriteLine("Which genre do you want to remove? ");
                            var genreQuery2 = (from genre in context.Genres
                                join movieGenre in context.MovieGenres
                                    on genre.Id equals movieGenre.Genre.Id
                                join movie in context.Movies on movieGenre.Movie.Id equals movie.Id
                                where movie.Id == searchId
                                select genre.Name).Distinct().ToList();
                            foreach (var g in genreQuery2)
                            {
                                Console.WriteLine($"{g}");
                            }
                            var result = Console.ReadLine().ToLower();
                            var removeGenre = (from movieGenre in context.MovieGenres
                                join genre in context.Genres on movieGenre.Genre.Id equals genre.Id
                                where genre.Name.ToLower() == result
                                select movieGenre).FirstOrDefault();
                            movieQuery.MovieGenres.Remove(removeGenre);
                            _logger.LogInformation(message:$"Removed {removeGenre.Genre.Name} from {movieQuery.Title}");

                        }
                        context.Movies.Update(movieQuery);
                        context.SaveChanges();
                        _logger.LogInformation(message:$"Updated {movieQuery.Title} in database");
                    }
                }
                else Console.WriteLine("Movie does not exist in database");
            }

        }
        
        public void Delete()
        {

            Console.WriteLine("Enter title of movie to delete: ");

            var searchString = Console.ReadLine().ToLower();
            using (var context = new MovieContext())
            {
                var movieQuery = (from movie in context.Movies
                    where movie.Title.ToLower().Contains(searchString)
                    select movie).FirstOrDefault();

                if (movieQuery != null)
                {
                    var choice = "";
                    Console.WriteLine($"Delete: {movieQuery.Title} (y)?");
                    choice = Console.ReadLine().ToLower();
                    if (choice == "y")
                    {
                        context.Movies.Remove(movieQuery);
                        context.SaveChanges();
                    }
                    _logger.LogInformation(message: $"Deleted {movieQuery.Title} from database");
                }
                else Console.WriteLine("Movie does not exist in database");
            }

        }

    }
}
