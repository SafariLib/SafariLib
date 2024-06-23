using Microsoft.EntityFrameworkCore;
using Test.Utils.Data.Models;

namespace Test.Utils.Data;

public class TestContext(DbContextOptions<TestContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}