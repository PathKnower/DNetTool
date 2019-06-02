using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DNet_PostgreSQL_Demonstration.DataContract
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        public string UserName { get; set; }

        
    }
}
