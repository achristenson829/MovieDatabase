namespace MovieDatabase.Services
{

    public interface IDataService
    {
        void DisplayMovies();
        void Search();
        void AddMovie();
        void Delete();
        void Update();

    }
}