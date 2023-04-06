using Data;
using Data.Seed;

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
                .ShouldEndWith($"{GetType().Name}_{nameof(TestPostgreSqUniqueMethodOk)}");
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
        int numUser = 50;
        using (new TimeThings(_output, "Time to seed database"))
        {
            await context.SeedDbContextAsync(numUser);
        }

        //VERIFY
        context.Students.Count().ShouldEqual(numUser);
        context.Classrooms.Count().ShouldEqual(5);
    }

    [Fact]
    public async Task TestEnsureDeletedOk()
    {
        //SETUP
        var options = this.CreatePostgreSqlUniqueClassOptions<TutorDbContext>();
        await using var context = new TutorDbContext(options);
        await context.Database.EnsureCreatedAsync();
        await context.SeedDbContextAsync(2);
        //ATTEMPT
        using (new TimeThings(_output, "Time to EnsureDeleted and EnsureCreated"))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        //VERIFY
        context.Students.Count().ShouldEqual(0);
    }
}