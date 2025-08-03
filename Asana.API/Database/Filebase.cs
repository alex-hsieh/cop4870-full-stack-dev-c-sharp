using Asana.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Api.ToDoApplication.Persistence
{
    public class Filebase
    {
        private string _root;
        private string _toDoRoot;
        private string _projectRoot;
        private static Filebase? _instance;

        public static Filebase Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Filebase();
                }
                return _instance;
            }
        }

        private Filebase()
        {
            _root = @"C:\temp";
            _toDoRoot = $"{_root}\\ToDos";
            _projectRoot = $"{_root}\\Projects";

            // Ensure directories exist
            Directory.CreateDirectory(_toDoRoot);
            Directory.CreateDirectory(_projectRoot);
        }

        public int LastToDoKey
        {
            get
            {
                if (ToDos.Any())
                {
                    return ToDos.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        public int LastProjectKey
        {
            get
            {
                if (Projects.Any())
                {
                    return Projects.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        // ToDo methods
        public ToDo AddOrUpdate(ToDo toDo)
        {
            if (toDo.Id <= 0)
            {
                toDo.Id = LastToDoKey + 1;
            }
            string path = $"{_toDoRoot}\\{toDo.Id}.json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(toDo));
            return toDo;
        }

        public List<ToDo> ToDos
        {
            get
            {
                var root = new DirectoryInfo(_toDoRoot);
                var _toDos = new List<ToDo>();
                foreach (var tOdOs in root.GetFiles("*.json"))
                {
                    var toDo = JsonConvert
                        .DeserializeObject<ToDo>
                        (File.ReadAllText(tOdOs.FullName));
                    if (toDo != null)
                    {
                        _toDos.Add(toDo);
                    }
                }
                return _toDos;
            }
        }

        public bool Delete(ToDo toDo)
        {
            string path = $"{_toDoRoot}\\{toDo.Id}.json";
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }

        // Project methods
        public Project AddOrUpdateProject(Project project)
        {
            if (project.Id <= 0)
            {
                project.Id = LastProjectKey + 1;
            }
            string path = $"{_projectRoot}\\{project.Id}.json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(project));
            return project;
        }

        public List<Project> Projects
        {
            get
            {
                var root = new DirectoryInfo(_projectRoot);
                var _projects = new List<Project>();
                foreach (var projFile in root.GetFiles("*.json"))
                {
                    var project = JsonConvert
                        .DeserializeObject<Project>
                        (File.ReadAllText(projFile.FullName));
                    if (project != null)
                    {
                        _projects.Add(project);
                    }
                }
                return _projects;
            }
        }

        public bool DeleteProject(Project project)
        {
            string path = $"{_projectRoot}\\{project.Id}.json";
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }

        public List<Project>? GetProjects(bool expand = false)
        {
            var projects = Projects;
            if (expand)
            {
                var todos = ToDos;
                foreach (var project in projects)
                {
                    project.ToDosListP = todos.Where(t => t.ProjectId == project.Id).ToList();
                }
            }
            return projects;
        }
    }
}