using Microsoft.EntityFrameworkCore;
using APIProdutos.Models;

namespace APIProdutos.Data;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
    }

    public DbSet<Produto> Produtos { get; set; }
}