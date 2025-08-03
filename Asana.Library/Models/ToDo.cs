using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Asana.Library.Models
{
    public class ToDo : INotifyPropertyChanged
    {
        public ToDo()
        {
            Id = 0;
            IsCompleted = false;
        }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Priority { get; set; }

        private bool? isCompleted;
        public bool? IsCompleted
        {
            get => isCompleted;
            set
            {
                if (isCompleted != value)
                {
                    isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }

        public int? ProjectId { get; set; }

        public int Id { get; set; }

        public DateTime? DueDate { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Description}";
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}