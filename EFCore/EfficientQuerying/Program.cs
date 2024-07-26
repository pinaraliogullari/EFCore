
using Microsoft.EntityFrameworkCore;
using System.Reflection;

ApplicationDbContext context = new();

#region EF Core Select Sorgularını Güçlendirme Teknikleri

#region IQueryable - IEnumerable Farkı

//IQueryable, bu arayüz üzerinde yapılan işlemler direkt generate edilecek olan sorguya yansıtılacaktır.
//IEnumerable, bu arayüz üzerinde yapılan işlemler temel sorgu neticesinde gelen ve in-memorye yüklenen instance'lar üzerinde gerçekleştirilir. Yani sorguya yansıtılmaz.

//IQueryable ile yapılan sorgulama çalışmalarında sql sorgusu hedef verileri elde edecek şekilde generate edilecekken, IEnumerable ile yapılan sorgulama çalışmalarında sql daha geniş verileri getirebilecek şekilde execute edilerek hedef veriler in-memory'de ayıklanır.

//IQueryable hedef verileri getirirken,IEnumerable hedef verilerden daha fazlasını getirip in-memory'de ayıklar.

#region IQueryable
//var persons = await context.Persons.Where(p => p.Name.Contains("a"))
//                             .Take(3)
//                             .ToListAsync();


//var persons = await context.Persons.Where(p => p.Name.Contains("a"))
//                             .Where(p => p.PersonId > 3)
//                             .Take(3)
//                             .Skip(3)
//                             .ToListAsync();

#endregion
#region IEnumerable
//var persons = context.Persons.Where(p => p.Name.Contains("a"))
//                             .AsEnumerable()
//                             .Take(3)
//                             .ToList();
#endregion

#region AsQueryable

#endregion
#region AsEnumerable

#endregion
#endregion

#region Yalnızca İhtiyaç Olan Kolonları Listele - Select
//var persons = await context.Persons.Select(p => new
//{
//    p.Name
//}).ToListAsync();
#endregion

#region Result'ı Limitleyin - Take
//await context.Persons.Take(50).ToListAsync();
#endregion

#region Join Sorgularında Eager Loading Sürecinde Verileri Filtrele
//var persons = await context.Persons.Include(p => p.Orders
//                                                  .Where(o => o.OrderId % 2 == 0)
//                                                  .OrderByDescending(o => o.OrderId)
//                                                  .Take(4))
//    .ToListAsync();

//foreach (var person in persons)
//{
//    var orders = person.Orders.Where(o => o.CreatedDate.Year == 2022);
//}

#endregion

#region Şartlara Bağlı Join Yapılacaksa Eğer Explicit Loading Kullan
//var person = await context.Persons.Include(p => p.Orders).FirstOrDefaultAsync(p => p.PersonId == 1);
//var person = await context.Persons.FirstOrDefaultAsync(p => p.PersonId == 1);

//if (person.Name == "Ayşe")
//{
//    //Order'larını getir...
//    await context.Entry(person).Collection(p => p.Orders).LoadAsync();
//}
#endregion

#region Lazy Loading Kullanırken Dikkatli Ol!
#region Riskli Durum
//var persons = await context.Persons.ToListAsync();

//foreach (var person in persons)
//{
//    foreach (var order in person.Orders)
//    {
//        Console.WriteLine($"{person.Name} - {order.OrderId}");
//    }
//    Console.WriteLine("***********");
//}
#endregion
#region İdeal Durum
//var persons = await context.Persons.Select(p => new { p.Name, p.Orders }).ToListAsync();

//foreach (var person in persons)
//{
//    foreach (var order in person.Orders)
//    {
//        Console.WriteLine($"{person.Name} - {order.OrderId}");
//    }
//    Console.WriteLine("***********");
//}
#endregion
#endregion

#region İhtiyaç Noktalarında Ham SQL Kullan - FromSql

#endregion

#region Asenkron Fonksiyonları Tercih Et

#endregion

#endregion

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}
public class Order
{
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }

    public virtual Person Person { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies().UseSqlServer(@"Server=DESKTOP-QE6JDF1\SQLEXPRESS;Database=ApplicationDB;User Id=sa;Password=1q2w3e;TrustServerCertificate=true")
     .UseLazyLoadingProxies();
    }
}
