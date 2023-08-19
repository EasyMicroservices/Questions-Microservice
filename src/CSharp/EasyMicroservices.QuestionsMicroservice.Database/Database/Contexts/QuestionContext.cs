using EasyMicroservices.QuestionsMicroservice.Database.Entities;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EasyMicroservices.QuestionsMicroservice.Database.Contexts
{
    public class QuestionContext : RelationalCoreContext
    {
        IDatabaseBuilder _builder;
        public QuestionContext(IDatabaseBuilder builder)
        {
            _builder = builder;
        }

        public DbSet<QuestionEntity> Questions { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_builder != null)
                _builder.OnConfiguring(optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuestionEntity>(model =>
            {
                model.HasKey(x => x.Id);
            });

        }
    }
}