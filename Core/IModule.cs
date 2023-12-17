namespace Core
{
    public interface IModule
    {
        string Name { get; }

        List<NavItem> NavItems { get; }

    }
}
