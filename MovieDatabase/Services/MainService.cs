using System;
using System.Threading.Channels;
using MovieDatabase.Context;

namespace MovieDatabase.Services
{


    public class MainService : IMainService
    {
        private readonly IDataService _dataService;

        public MainService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void Invoke()
        {
            string choice;
            do
            {
                Console.WriteLine("1) Display movies");
                Console.WriteLine("2) Search movies");
                Console.WriteLine("3) Add movie");
                Console.WriteLine("4) Update movie");
                Console.WriteLine("5) Delete movie");

                Console.WriteLine("X) Quit");
                choice = Console.ReadLine().ToUpper();

                if (choice == "1")
                {
                    _dataService.DisplayMovies();
                }
                else if (choice == "2")
                {
                    _dataService.Search();
                }
                else if (choice == "3")
                {
                    _dataService.AddMovie();
                }
                else if (choice == "4")
                {
                    _dataService.Update();
                }
                else if (choice == "5")
                {
                    _dataService.Delete();
                }
                
            } while (choice != "X");
        }
    }
}