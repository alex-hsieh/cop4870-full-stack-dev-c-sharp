using Asana.Library.Models;

namespace Asana.API.Database
{
    public class FakeDatabase
    {
        static FakeDatabase? instance;
        static object instanceLock = new object();

        private List<ToDo> toDos = new List<ToDo>();
        public List<ToDo> ToDos
        {
            get { return toDos; }
        }

        private List<Project> projects = new List<Project>();

        private FakeDatabase()
        {

            toDos = new List<ToDo>
                {
                    new ToDo{Id = 1, Name = "Task 1", Description = "My Task 1", IsCompleted=true, ProjectId =1},
                    new ToDo{Id = 2, Name = "Task 2", Description = "My Task 2", IsCompleted=false, ProjectId = 1 },
                    new ToDo{Id = 3, Name = "Task 3", Description = "My Task 3", IsCompleted=true , ProjectId = 1},
                    new ToDo{Id = 4, Name = "Task 4", Description = "My Task 4", IsCompleted=false , ProjectId = 2},
                    new ToDo{Id = 5, Name = "Task 5", Description = "My Task 5", IsCompleted=true , ProjectId = 3}
                };

            projects = new List<Project>()
            {
                new Project
                {
                    Id = 1,
                    Name = "Project 1",
                    Description = "First project description",
                    CompletePercent = 75.0,
                    ToDosListP = toDos.Where(t => t.ProjectId == 1).ToList()
                },
                new Project
                {
                    Id = 2,
                    Name = "Project 2",
                    Description = "Second project description",
                    CompletePercent = 40.0,
                    ToDosListP= toDos.Where(t => t.ProjectId == 2).ToList()
                },
                new Project
                {
                    Id = 3,
                    Name = "Project 3",
                    Description = "Third project description",
                    CompletePercent = 100.0,
                    ToDosListP= toDos.Where(t => t.ProjectId == 3).ToList()
                },
                new Project
                {
                    Id = 4,
                    Name = "Project 4",
                    Description = "Fourth project description",
                    CompletePercent = 0.0,
                    ToDosListP = new List<ToDo>()
                },
                new Project
                {
                    Id = 5,
                    Name = "Project 5",
                    Description = "Fifth project description",
                    CompletePercent = 0.0,
                    ToDosListP = new List<ToDo>()
                },
                new Project
                {
                    Id = 6,
                    Name = "Project 6",
                    Description = "Sixth project description",
                    CompletePercent = 0.0,
                    ToDosListP = new List<ToDo>()
                },
            };

            // Initialize nextKeys properly based on existing data
            nextKeys = new Dictionary<DataType, int>();
            nextKeys.Add(DataType.ToDo, GetNextAvailableId(toDos.Select(t => t.Id)));
            nextKeys.Add(DataType.Project, GetNextAvailableId(projects.Select(p => p.Id)));
        }

        // Helper method to calculate next available ID
        private int GetNextAvailableId(IEnumerable<int> existingIds)
        {
            return existingIds.Any() ? existingIds.Max() + 1 : 1;
        }

        public List<Project>? GetProjects(bool Expand = false)
        {
            if (Expand)
            {
                var projectList = new List<Project>();
                foreach (var project in projects)
                {
                    var proj = project;
                    proj.ToDosListP = ToDos.Where(t => t.ProjectId == proj.Id).ToList();
                    projectList.Add(proj);
                }
                return projectList;
            }
            return projects;
        }

        public ToDo? AddOrUpdateToDo(ToDo? toDoToAdd)
        {
            if (toDoToAdd == null)
            {
                return toDoToAdd;
            }

            if (toDoToAdd.Id <= 0)
            {
                toDoToAdd.Id = nextKeys[DataType.ToDo]++;
                toDos.Add(toDoToAdd);
            }
            else
            {
                var oldToDo = toDos.FirstOrDefault(t => t.Id == toDoToAdd.Id);
                if (oldToDo != null)
                {
                    toDos.Remove(oldToDo);
                }
                toDos.Add(toDoToAdd);
            }
            return toDoToAdd;
        }

        public ToDo? DeleteToDo(ToDo? toDoToDelete)
        {
            if (toDoToDelete == null)
            {
                return null;
            }

            toDos.Remove(toDoToDelete);
            return toDoToDelete;
        }

        public Project? AddOrUpdateProject(Project? projectToAdd)
        {
            if (projectToAdd == null) return projectToAdd;

            lock (instanceLock) // Add this for thread safety
            {
                if (projectToAdd.Id <= 0)
                {
                    projectToAdd.Id = nextKeys[DataType.Project]++;
                    projects.Add(projectToAdd);
                }
                else
                {
                    var oldProject = projects.FirstOrDefault(p => p.Id == projectToAdd.Id);
                    if (oldProject != null)
                    {
                        projects.Remove(oldProject);
                    }
                    projects.Add(projectToAdd);
                }
            }
            return projectToAdd;
        }

        public Project? DeleteProject(Project? projectToDelete)
        {
            if (projectToDelete == null)
            {
                return null;
            }

            projects.Remove(projectToDelete);
            return projectToDelete;
        }

        public static FakeDatabase Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new FakeDatabase();
                    }
                }
                return instance;
            }
        }

        private Dictionary<DataType, int> nextKeys;

        // Optional: Methods to reset/reinitialize the database for testing
        public static void Reset()
        {
            lock (instanceLock)
            {
                instance = null;
            }
        }

        // Debug method to check current next keys
        public void LogNextKeys()
        {
            Console.WriteLine($"Next ToDo ID: {nextKeys[DataType.ToDo]}");
            Console.WriteLine($"Next Project ID: {nextKeys[DataType.Project]}");
        }
    }

    public enum DataType
    {
        ToDo, Project
    }
}