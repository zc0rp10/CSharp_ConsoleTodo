namespace ConsoleTodo
{
    class TodoApp
    {

        static List<Task> userTasks = new List<Task>();
        static string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//TaskList.txt";
        static bool endApp = false;
        static int taskIdCount = 0;
        static string sortOrder = "taskPriority";

        static void Main(string[] args)
        {
            LoadDataFromFile(); //Retrives Saved Tasks
            
            while (!endApp)
            {
                DrawView(); //Paints the UI
                StartListeningForUserKeyPress(); //User Key Input to start Add,Delete,Sort etc. functions.
                SaveDataToFile();
            }

        }

        private static void LoadDataFromFile()
        {
            if (File.Exists(filePath)) //Will not exist first time the user runs program.
            {
                List<string> lines = File.ReadAllLines(filePath).ToList();
                List<int> taskIdCountList = new List<int>();
                foreach (var line in lines)
                {
                    string[] entries = line.Split(",");
                    userTasks.Add(new Task(int.Parse(entries[0]), int.Parse(entries[1]), entries[2])); //Order: Id, Priority, TaskName
                    taskIdCountList.Add(int.Parse(entries[0]));
                }
                if (taskIdCountList.Any()) taskIdCount = taskIdCountList.Max();
            }
        }

        private static void SaveDataToFile()
        {
            List<string> output = new List<string>();
            foreach (var task in userTasks)
            {
                output.Add($"{task.taskId},{task.taskPriority},{task.taskName}");
            }
            File.WriteAllLines(filePath, output);
        }

        private static void EditTask()
        {
            Console.Write("Enter the Id for the task you'd like to EDIT: ");
            int taskId = int.Parse(Console.ReadLine());

            try
            {
                userTasks.Remove(userTasks.Single(r => r.taskId == taskId));
                Console.Write("Enter an updated task description: ");
                string newTask = Console.ReadLine();
                Console.Write("Enter new task priority, 0 - Low, 1 - Normal, 2 - High: ");
                int priority = int.Parse(Console.ReadLine());
                userTasks.Add(new Task(taskId, priority, newTask)); //Order: Id, Priority, TaskName
            }
            catch (Exception e)
            {
                Console.WriteLine("\nERROR: " + e.Message + "\nPress any key to continue..");
                Console.ReadKey();
            }
        }

        private static void RemoveTask()
        {
            Console.Write("Enter the Id for the task you'd like to REMOVE: ");
            int taskId = int.Parse(Console.ReadLine());

            try
            {
                userTasks.Remove(userTasks.Single(r => r.taskId == taskId));
            }
            catch (Exception e)
            {
                Console.WriteLine("\nERROR: " + e.Message + "\nPress any key to continue..");
                Console.ReadKey();
            }
        }

        static void AddTask()
        {
            try
            {
                Console.Write("Enter new task description: ");
                string newTask = Console.ReadLine();
                Console.Write("Enter task priority, 0 - Low, 1 - Normal, 2 - High: ");
                int priority = int.Parse(Console.ReadLine());
                userTasks.Add(new Task(++taskIdCount, priority, newTask)); //Order: Id, Priority, TaskName
            }
            catch (Exception e)
            {
                Console.WriteLine("\nERROR: " + e.Message + "\nPress any key to continue..");
                Console.ReadKey();
            }
        }

        public static void DrawView()
        {
            Console.Clear();
            Console.WriteLine("Console Todo Application in C#\r");
            Console.WriteLine("------------------------------------------------------------------------\n");
            Console.WriteLine("These are your current tasks:");
            Console.Write("\n");
            PrintList(); //Gets Current Tasks
            Console.WriteLine("\n------------------------------------------------------------------------");
            Console.Write("\n");    
            Console.WriteLine("A - Add Task | R - Remove Task | E - Edit Task | S - Sort By | Q - Quit");
            Console.Write("\n");
        }

        public static void StartListeningForUserKeyPress()
        {
            ConsoleKeyInfo keyInfo;
            keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.A:
                    {
                        AddTask();
                        break;
                    }
                case ConsoleKey.R:
                    {
                        RemoveTask();
                        break;
                    }
                case ConsoleKey.E:
                    {
                        EditTask();
                        break;
                    }
                case ConsoleKey.S:
                    {
                        Console.Write("Select sort order, I - Id, P - Priority, D - Description: ");
                        keyInfo = Console.ReadKey(true);
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.I:
                                {
                                    sortOrder = "taskId";
                                    break;
                                }
                            case ConsoleKey.P:
                                {
                                    sortOrder = "taskPriority";
                                    break;
                                }
                            case ConsoleKey.D:
                                {
                                    sortOrder = "taskName";
                                    break;
                                }
                        }
                        break;
                    }
                case ConsoleKey.Escape:
                case ConsoleKey.Q:
                    {
                        endApp = true; //Quits the application
                        break;
                    }

                default: break;
            }
        }
        public static void PrintList()
        {
            Console.WriteLine("ID\tDescription\t\t\t\t\t\tPriority");
            Console.WriteLine("--\t----\t\t\t\t\t\t\t--------");

            List<Task> sortedList = userTasks;
            switch (sortOrder)
            {
                case "taskId":
                    sortedList = userTasks.OrderBy(o => o.taskId).ToList();
                    break;
                case "taskPriority":
                    sortedList = userTasks.OrderByDescending(o => o.taskPriority).ToList();
                    break;
                case "taskName":
                    sortedList = userTasks.OrderBy(o => o.taskName).ToList();
                    break;
            }

            foreach (var task in sortedList)
            {
                string priorityInText;
                switch (task.taskPriority)
                {
                    case 0:
                        {
                            priorityInText = "Low";
                            break;
                        }
                    case 1:
                        {
                            priorityInText = "Normal";
                            break;
                        }
                    case 2:
                        {
                            priorityInText = "High";
                            break;
                        }
                    default:
                        {
                            priorityInText = "None";
                            break;
                        }
                }
                Console.WriteLine($"{task.taskId}\t{task.taskName}\t\t\t\t\t\t{priorityInText}");
            }
        }
    }
}
