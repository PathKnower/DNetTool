using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DNet_PostgreSQL_Demonstration.DataContract
{
    public class News
    {
        [Key]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public User Author { get; set; }

        public User Editor { get; set; }
    }
}
