namespace auth
{
    public class AuthUserModel<TPermission> where TPermission : struct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Administrador { get; set; } = false;
        public List<TPermission>? Permissions { get; set; }
    }
}
