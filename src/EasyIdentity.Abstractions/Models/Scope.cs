namespace EasyIdentity.Models;

public class Scope
{
    public Scope(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public Scope()
    {

    }

    public string Name { get; set; }
    public string Description { get; set; }
}
