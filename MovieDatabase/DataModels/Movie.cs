using System;
using System.Collections.Generic;

namespace MovieDatabase.DataModels
{
    public class Movie
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }

        
        public virtual ICollection<MovieGenre> MovieGenres {get;set;}
        public virtual ICollection<UserMovie> UserMovies {get;set;}
    }
}
