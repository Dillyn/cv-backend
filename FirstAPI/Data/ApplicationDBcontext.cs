using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Data
{
    public class ApplicationDBcontext: DbContext
    {
        public ApplicationDBcontext(DbContextOptions dbContextOptions):base(dbContextOptions) { }//Pass up to Db context
        public DbSet<User> user {  get; set; }// set table
        public DbSet<Hobby> hobby { get; set; }// set table
    }
}
