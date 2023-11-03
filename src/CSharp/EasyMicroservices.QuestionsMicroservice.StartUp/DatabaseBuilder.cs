using EasyMicroservices.Cores.Relational.EntityFrameworkCore.Intrerfaces;
using EasyMicroservices.QuestionsMicroservice.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.QuestionsMicroservice
{
    public class DatabaseBuilder : IEntityFrameworkCoreDatabaseBuilder
    {
        IConfiguration _configuration;
        public DatabaseBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("QuestionDatabase");
            //optionsBuilder.UseSqlServer(_configuration.GetConnectionString("local"));
        }
    }
}
