using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core; // useful thing
using System.Linq.Expressions;
using System.Threading.Tasks;
using DNet_PostgreSQL_Demonstration.DataContract;
using Microsoft.EntityFrameworkCore;

namespace DNet_PostgreSQL_Demonstration
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options) 
            : base(options)
        {
            Database.Migrate();
        }

        /// <summary>
        /// User's table
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// News table
        /// </summary>
        public DbSet<News> News { get; set; }

    }
}
