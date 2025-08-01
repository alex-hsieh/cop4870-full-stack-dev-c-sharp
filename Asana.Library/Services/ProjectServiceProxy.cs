using Asana.Library.Models;
using Asana.Maui.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public  class ProjectServiceProxy
    {
        private List<Project> projects;
        public List<Project> Projects
        {
            get
            {
                return projects;
            }
        }
        private ProjectServiceProxy() {
            var projectData = new WebRequestHandler().Get("/Project/Expand").Result;
            projects = JsonConvert.DeserializeObject<List<Project>>(projectData) ?? new List<Project>();
        }
        private static object _lock = new object();
        private static ProjectServiceProxy? instance;
        public static ProjectServiceProxy Current
        {
            get
            {
                lock (_lock) {
                    if (instance == null)
                    {
                        instance = new ProjectServiceProxy();
                    }
                }

                return instance;
            }
        }
    }
}
