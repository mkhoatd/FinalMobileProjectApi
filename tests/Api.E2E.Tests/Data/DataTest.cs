using Data;

using Microsoft.EntityFrameworkCore;

using Npgsql;

using Test.Helpers;

using TestSupport.EfHelpers;

using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace FinalMobileProjectApi.E2E.Tests.Data;

public class DataTest
{
    private readonly ITestOutputHelper _output;

    public DataTest(ITestOutputHelper output)
    {
        _output = output;
    }


    [Fact]
    public void TestPostgreSqlUniqueClassOk()
    {
        //SETUP
        //ATTEMPT
        var options = this.CreatePostgreSqlUniqueClassOptions<TutorDbContext>();
        using (var context = new TutorDbContext(options))
        {
            //VERIFY
            var builder = new NpgsqlConnectionStringBuilder(
                context.Database.GetDbConnection().ConnectionString);
            builder.Database.ShouldEndWith(GetType().Name);
        }
    }
    [Fact]
    public void TestPostgreSqUniqueMethodOk()
    {
        //SETUP
        //ATTEMPT
        var options = this.CreatePostgreSqlUniqueMethodOptions<TutorDbContext>();
        using (var context = new TutorDbContext(options))
        {
            //VERIFY
            var builder = new NpgsqlConnectionStringBuilder(context.Database.GetDbConnection().ConnectionString);
            builder.Database
                .ShouldEndWith($"{GetType().Name}_{nameof(TestPostgreSqUniqueMethodOk)}" );
        }
    }
    [Fact]
    public async Task TestEnsureCreatedOk()
    {
        //SETUP
        var options = this.CreatePostgreSqlUniqueClassOptions<TutorDbContext>();
        using var context = new TutorDbContext(options);
            
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        //ATTEMPT
        await context.SeedDbContextTestAsync();

        //VERIFY
        context.Users.Count().ShouldEqual(2);
        context.Roles.Count().ShouldEqual(2);
    }
    [Fact]
    public async Task TestEnsureDeletedOk()
    {
        //SETUP
        var options = this.CreatePostgreSqlUniqueClassOptions<TutorDbContext>();
        using var context = new TutorDbContext(options);
        context.Database.EnsureCreated();
        await context.SeedDbContextTestAsync();
        //ATTEMPT
        using (new TimeThings(_output, "Time to EnsureDeleted and EnsureCreated"))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        //VERIFY
        context.Users.Count().ShouldEqual(0);
    }
    
}