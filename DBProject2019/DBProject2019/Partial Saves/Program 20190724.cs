using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DBProject2019
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialise User object
            UserInfo UserDb = new UserInfo();
            char option;
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("1. Print all users");
                Console.WriteLine("2. Add user");
                Console.WriteLine("3. Remove user");
                Console.WriteLine("4. Save & exit");
                option = Console.ReadKey().KeyChar;
                switch (option)
                {
                    case '1':
                        UserDb.Print();
                        break;
                    case '2':
                        // TURN DATA GATHERING TO A FUNCTION
                        Console.Write("Name = ");
                        string name = Console.ReadLine();
                        Console.Write("Userid = ");
                        string userid = Console.ReadLine();
                        UserDb.AddUser(name, userid);
                        break;
                    case '3':
                        // REMOVE USER
                        break;
                    case '4':
                        exit = true;
                        break;
                }
            }

            // Save the file and exit;
            UserDb.Save();
            Console.WriteLine("Exiting the application");
            Console.ReadKey();

        }
    }

    [Serializable]
    class User
    {
        private string name;
        private string userid;
        private string email;
        private string postcode;

        public User(string name, string userid) // User constructor
        {
            // assign basic values (add more later when know working)
            this.name = name;
            this.userid = userid;
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string Userid
        {
            get { return this.userid; }
            set { this.userid = value; }
        }
    }


    // GOAL IS TO CHANGE THIS CLASS INTO A BASE CLASS FOR OTHER OBJECTS
    //      THIS MEANS THAT THERE WILL BE A CLASS FOR EACH TABLE WHICH
    //      ALL INHERIT FROM THIS PARENT CLASS.
    class UserInfo
    {
        private static UserInfo user_info;

        private Dictionary<string, User> user_dict;
         private BinaryFormatter formatter; // used for reading the file

        private const string filename = @"B:\Documents\Post-University\Programming\Databases\DBProject2019\DBProject2019\UserInfo.dat";

        // public static UserInfo Instance(){}

        public UserInfo() {
            this.user_dict = new Dictionary<string, User>();
            this.formatter = new BinaryFormatter();

            this.Load();
        }

        public void AddUser(string name, string userid)
        {
            if (this.user_dict.ContainsKey(userid)) // if this user has been added before
            {
                // do not add this user to the dict
                Console.WriteLine(name + " has been added before");
            }
            else
            {
                // add this user to the dict
                this.user_dict.Add(userid, new User(name, userid));
                Console.WriteLine(name + " added successfully");
            }
        }

        public void Save()
        {
            try
            {
                // Create a filestream for the data
                FileStream FileStreamWriter = new FileStream(filename, FileMode.Create, FileAccess.Write);
                // Save the data to the filestream
                this.formatter.Serialize(FileStreamWriter, this.user_dict);

                // Close the filestream 
                FileStreamWriter.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to save the file");
                Console.WriteLine(e);
            }
        }

        public void Load()
        {
            if (File.Exists(filename))
            {
                try
                {
                    // Create Filestream to open file
                    FileStream FileStreamReader = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    // Reconstruct data from the file
                    this.user_dict = (Dictionary<String, User>)this.formatter.Deserialize(FileStreamReader);

                    // Close the Filestream
                    FileStreamReader.Close();
                    Console.WriteLine("File Loaded");
                }
                catch
                {
                    Console.WriteLine("The data file is present but could not be read.");
                }
            }
            else
            {
                // CREATE the file
                FileStream FileStreamCreater = new FileStream(filename, FileMode.CreateNew);
                // Write empty file
                this.formatter.Serialize(FileStreamCreater, this.user_dict);

                FileStreamCreater.Close();
                Console.WriteLine("File Created");
            }
        }

        public void Print()
        {
            if (this.user_dict.Count > 0)
            {
                foreach(User user in this.user_dict.Values)
                {
                    Console.WriteLine(user.Name + " - " + user.Userid);
                }
            }
            else
            {
                Console.WriteLine("No user entries");
            }
        }
    }
}
