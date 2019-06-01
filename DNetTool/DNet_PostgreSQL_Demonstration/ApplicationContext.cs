using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNet_PostgreSQL_Demonstration.DataContract;
using Microsoft.EntityFrameworkCore;

namespace DNet_PostgreSQL_Demonstration
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<News> News { get; set; }
    }
}
