using answersbot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace answersbot.Storage
{
    public class DataContext
    {
        public DataContext()
        {
            Users = new List<User>();
            Questions = new List<Question>();
        }

        private static DataContext _database;

        public List<User> Users { get; set; }
        public List<Question> Questions { get; set; }

        public static DataContext Database()
        {
            if(_database == null)
            {
                _database = new DataContext();
            }

            return _database;
        }
    }
}