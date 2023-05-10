namespace dominio
{
    public class Item
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public bool Status { get; set; }

        public Item(int id, string? nome, bool status)
        {
            Id = id;
            Nome = nome;
            Status = status;
        }
    }
}
