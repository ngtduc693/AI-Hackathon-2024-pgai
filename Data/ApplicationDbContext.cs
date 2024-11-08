using InsuranceBot.Models;
using Microsoft.Bot.Schema.Teams;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InsuranceBot.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
    }
}
