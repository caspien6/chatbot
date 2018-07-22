using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Context
{

    public class StoryContextFactory : IDesignTimeDbContextFactory<StoryContext>
    {
        public StoryContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StoryContext>();
            optionsBuilder.UseSqlServer("Data Source=lackofmoney.database.windows.net;Initial Catalog=ChatStory;Integrated Security=False;User ID=Caspien6;Password=Eleskamra96ede;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            return new StoryContext(optionsBuilder.Options);
        }
    }
}
