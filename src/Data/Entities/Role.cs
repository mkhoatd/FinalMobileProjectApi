namespace Data.Entities;

public class Role
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required List<User> Users { get; set; }
}