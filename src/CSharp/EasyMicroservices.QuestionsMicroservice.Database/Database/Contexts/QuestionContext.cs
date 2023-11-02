using EasyMicroservices.QuestionsMicroservice.Database.Entities;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore.Intrerfaces;

namespace EasyMicroservices.QuestionsMicroservice.Database.Contexts
{
    public class QuestionContext : RelationalCoreContext
    {
        IEntityFrameworkCoreDatabaseBuilder _builder;
        public QuestionContext(IEntityFrameworkCoreDatabaseBuilder builder) : base(builder)
        {
        }

        public DbSet<QuestionEntity> Questions { get; set; }
        public DbSet<AnswerEntity> Answers { get; set; }


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

            modelBuilder.Entity<AnswerEntity>(model =>
            {
                model.HasKey(x => x.Id);

                model.HasOne(x => x.Question)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.QuestionId);
            });
        }
    }
}