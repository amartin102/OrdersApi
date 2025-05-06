using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Repository.Coconseconsentext
{
    public class OrdersDb : DbContext
    {
        public string DefaultConnection { get; set; }

        public OrdersDb(DbContextOptions<OrdersDb> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            optionsBuilder.UseNpgsql(GetConnection());
            //}
        }

        private string GetConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            DefaultConnection = configuration.GetConnectionString("PostgreConnectionString");
            return DefaultConnection;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación uno a muchos entre Cliente y Orden
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client) // Una orden tiene un cliente
                .WithMany(c => c.Orders) // Un cliente tiene muchas órdenes
                .HasForeignKey(o => o.IdClient); // La clave foránea que referencia a Cliente

            modelBuilder.Entity<OrderItem>()
           .HasOne(d => d.Order) // Un detalle pertenece a un pedido
           .WithMany(p => p.OrderItems) // Un pedido tiene muchos detalles
           .HasForeignKey(d => d.IdOrder) // Clave foránea
           .OnDelete(DeleteBehavior.Cascade); // Borrar en cascada si el pedido se elimina

            modelBuilder.Entity<OrderItem>()
           .HasOne(d => d.Item) // Un detalle pertenece a un item
           .WithMany(p => p.Items) // Un pedido tiene muchos items asociados
           .HasForeignKey(d => d.IdItem); // Clave foránea


        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Item> Item { get; set; }

        public DbSet<OrderItem> OrderItem { get; set; }
    }
}
