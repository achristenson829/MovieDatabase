using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.Design;
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
                _logger.LogError(message: "Invalid input");
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
                if (movieQuery != null)
                {
                    foreach (var movie in movieQuery)
                    {
                        Console.Write($"Id:{movie.Id} Title:{movie.Title} Genres:");
                        foreach (var genre in genreQuery)
                        {
                            Console.Write(genre + "|");
                        }

                        Console.WriteLine();
                    }
                }
                else _logger.LogError(message: "Movie doesn't exist in database");
            }
        }

        public void AddMovie()
        {
            var movie = new Movie();
            Console.WriteLine("Enter Movie Title: ");
            var title = Console.ReadLine();
            movie.Title = title;

            Console.WriteLine("Enter Release Date: ");

            var releaseDate = Console.ReadLine();
            var validReleaseDate = DateTime.Now;
            while (!DateTime.TryParse(releaseDate, out validReleaseDate))
            {
                Console.WriteLine("Not a valid date! Enter new release date (mm/dd/yyyy)");
                releaseDate = Console.ReadLine();
            }


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
            while (!int.TryParse(Console.ReadLine(), out searchId))
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
                        _logger.LogInformation(message: $"Updated {movieQuery.Title} in database");
                    }

                    Console.WriteLine("Do you want to update release date (y)?");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        var newReleaseDate = DateTime.Now;
                        Console.WriteLine(
                            $"Release date is {movieQuery.ReleaseDate} - Enter new release date (mm/dd/yyy): ");
                        while (!DateTime.TryParse(Console.ReadLine(), out newReleaseDate))
                        {
                            Console.WriteLine("Not a valid date! Enter new release date (mm/dd/yyyy)");
                        }

                        movieQuery.ReleaseDate = newReleaseDate;
                        _logger.LogInformation(message: $"Updated release date of {movieQuery.Title} in database");

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

                        var update = "";
                        do
                        {
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
                                var genreID = Console.ReadLine();
                                while (!int.TryParse(genreID, out gId))
                                {
                                    Console.WriteLine("Id must be a number! Enter valid Id: ");
                                    genreID = Console.ReadLine();
                                }

                                while (gId < 1 || gId > 18)
                                {
                                    Console.WriteLine("Not a valid Id! Enter valid Id: ");
                                    gId = Int32.Parse(Console.ReadLine());
                                }

                                var addGenre = (from genre in context.Genres
                                                join movieGenre in context.MovieGenres on genre.Id equals movieGenre.Genre.Id
                                                where movieGenre.Genre.Id == gId
                                                select movieGenre).FirstOrDefault();
                                movieQuery.MovieGenres.Add(addGenre);
                                _logger.LogInformation(message: $"Added {addGenre.Genre.Name} to {movieQuery.Title}");

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
                                _logger.LogInformation(
                                    message: $"Removed {removeGenre.Genre.Name} from {movieQuery.Title}");
                            }
                            Console.WriteLine("Do you want to update another genre?");
                            update = Console.ReadLine().ToLower();
                        } while (update != "n");

                        context.Movies.Update(movieQuery);
                        context.SaveChanges();
                        _logger.LogInformation(message: $"Updated {movieQuery.Title} in database");

                    }
                }
                else _logger.LogError(message: "Movie doesn't exist in database");
            }

        }

        public void RateMovie()
        {
            UserMovie userMovie = new UserMovie();
            using (var context = new MovieContext())
            {
                Console.WriteLine("Enter title of movie to rate: ");
                var searchString = Console.ReadLine();
                var movieQuery = (from movie in context.Movies
                                  where movie.Title.ToLower().Contains(searchString)
                                  select movie).FirstOrDefault();
                if (movieQuery != null)
                {
                    Console.WriteLine($"You want to rate {movieQuery.Title}? (y/n)");
                    var choice = Console.ReadLine().ToLower().Substring(0, 1);
                    if (choice == "y")
                    {
                        userMovie.Movie = movieQuery;
                    }
                }
                else _logger.LogError(message: "Movie not found in database");

                Console.WriteLine("Enter user Id of user who is rating movie: ");
                var searchId = Console.ReadLine();
                var id = 0;
                while (!int.TryParse(searchId, out id))
                {
                    Console.WriteLine("Not a number! Enter a valid userID: ");
                    searchId = Console.ReadLine();
                }

                var userQuery = (from user in context.Users where user.Id == id select user).FirstOrDefault();
                if (userQuery != null)
                {
                    userMovie.User = userQuery;
                }
                else _logger.LogError(message: "User doesn't exist in database");

                Console.WriteLine($"What do you want to rate {movieQuery.Title}? 1-5");
                var rating = Console.ReadLine();
                var userRating = 0;
                if (!int.TryParse(rating, out userRating))
                {
                    Console.WriteLine("Not a number!  Enter valid rating: ");
                }
                else if (userRating < 1 || userRating > 5)
                {
                    Console.WriteLine("Not a valid rating! Rating must be from 1 to 5! Enter valid rating");
                    userRating = Int32.Parse(Console.ReadLine());
                }

                userMovie.Rating = userRating;
                userMovie.RatedAt = DateTime.Now;

                context.UserMovies.Add(userMovie);
                context.SaveChanges();

                _logger.LogInformation(
                    message:
                    $"User {userQuery.Id} rated {movieQuery.Title} a {userMovie.Rating} at {userMovie.RatedAt}");

            }
        }

        public void AddUser()
        {
            using (var context = new MovieContext())
            {
                User user = new User();
                var addChoice = "";
                do
                {
                    var userAge = 0;
                    Console.WriteLine("Enter age of new user: ");
                    var age = Console.ReadLine();
                    while (!int.TryParse(age, out userAge))
                    {
                        Console.WriteLine("Must be a number!  Enter valid age: ");
                        age = Console.ReadLine();
                    }

                    if (userAge < 1 || userAge > 100)
                    {
                        Console.WriteLine("Not a valid age! Age must be >1 and >100");
                        userAge = Int32.Parse(Console.ReadLine());
                    }

                    user.Age = userAge;
                    Console.WriteLine("Enter user gender (M/F): ");
                    var userGender = Console.ReadLine().Substring(0, 1).ToUpper();

                    user.Gender = userGender;

                    Console.WriteLine("Enter User ZipCode: ");
                    var zipCode = Console.ReadLine();
                    while (zipCode.Length != 5)
                    {
                        Console.WriteLine("ZipCode is 5 digits! Enter 5 digit user ZipCode");
                        zipCode = Console.ReadLine();
                    }

                    user.ZipCode = zipCode;
                    Console.WriteLine("Enter occupation Id, here are possible occupations: ");

                    foreach (var o in context.Occupations)
                    {
                        Console.WriteLine($"Id: {o.Id}) {o.Name}");
                    }

                    var occId = Console.ReadLine();
                    var oID = 0;
                    while (!int.TryParse(occId, out oID))
                    {
                        Console.WriteLine("Must be a number!  Enter valid Occupation Id: ");
                        occId = Console.ReadLine();
                    }

                    while (oID < 0 || oID > 21)
                    {
                        Console.WriteLine("Not a valid Occupation Id! Enter a number from 1 - 21: ");
                    }

                    var occQuery = (from o in context.Occupations where o.Id == oID select o).FirstOrDefault();
                    if (occQuery != null)
                    {
                        user.Occupation = occQuery;
                    }


                    Console.WriteLine($"You've entered a User Age of: {user.Age}\nUser Gender of: {user.Gender}\n" +
                                      $"User ZipCode of: {user.ZipCode}\nUser Occupation of: {user.Occupation.Name}\n" +
                                      $"Add user to database? (y/n)");

                    addChoice = Console.ReadLine().ToLower().Substring(0, 1);


                } while (addChoice != "y");

                context.Users.Add(user);
                context.SaveChanges();

                _logger.LogInformation(message: "Add new user to database");
            }
        }

    }


}

