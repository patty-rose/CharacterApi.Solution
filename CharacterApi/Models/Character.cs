using System.ComponentModel.DataAnnotations;

namespace CharacterApi.Models
{
  public class Character
  {
    public int CharacterId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int Age { get; set; }

    public string MediaTitle{ get; set; }

    public string MediaType { get; set; }
  }
}