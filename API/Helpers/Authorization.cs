namespace API.Helpers;

public class Authorization
{
    public enum Roles
    {
        Administrator,
        Employee,
        Patient,
        WithoutRol
    }
    public const Roles rol_default = Roles.WithoutRol;
}
