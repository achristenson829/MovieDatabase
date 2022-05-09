namespace MovieDatabase.Services
{

    public interface IDataService
    {
        void DisplayMovies();
        void Search();
        void AddMovie();
        void Update();
        void AddUser();
        void RateMovie();

    }
}