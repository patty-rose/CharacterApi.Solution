using Microsoft.EntityFrameworkCore;
using CharacterApi.Models;

namespace CharacterApi.Models
{
  public class CharacterApiContext : DbContext
  {
    public CharacterApiContext(DbContextOptions<CharacterApiContext> options)
      : base(options)
    {
    }

    public DbSet<Character> Characters { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Character>()
        .HasData(
          new Character { CharacterId = 1, FirstName = "Matilda", LastName = "Woolly Mammoth", Age = 7, MediaTitle = "The Land Before Time", MediaType = "Film" },
          new Character { CharacterId = 2, FirstName = "Rexie", LastName = "Dinosaur", Age = 10, MediaTitle = "The Land Before Time", MediaType = "Film"},
          new Character { CharacterId = 3, FirstName = "Matilda", LastName = "Dinosaur", Age = 2, MediaTitle = "The Land Before Time", MediaType = "Film" },
          new Character { CharacterId = 4, FirstName = "Pip", LastName = "Shark", Age = 4, MediaTitle = "The Land Before Time", MediaType = "Film" },
          new Character { CharacterId = 5, FirstName = "Bartholomew", LastName = "Dinosaur", Age = 22, MediaTitle = "The Land Before Time", MediaType = "Film" }
      ); 
    }
  }
}