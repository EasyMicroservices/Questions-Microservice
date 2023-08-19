using Microsoft.EntityFrameworkCore;

namespace EasyMicroservices.QuestionsMicroservice.Database
{
    public interface IDatabaseBuilder
    {
        void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
    }
}
