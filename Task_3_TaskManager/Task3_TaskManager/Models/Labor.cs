using System;

namespace Task3_TaskManager
{
    public class Labor
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"#{Id}|{Title}|{Description}|{IsCompleted}|{CreatedAt}";
        }
    }
}
