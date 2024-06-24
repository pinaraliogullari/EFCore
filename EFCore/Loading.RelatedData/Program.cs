
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Reflection;

ApplicationDbContext context = new();

#region Eager Loading
//eager loading , generate edilen bir sorguya ilişkisel verilerin parça parça eklenmesini sağlayan ve bunu yaparken iradeli/istekli bir şekilde yapmamızı sağlayan bir yöntemdir. 
#region Include
// Eager loading operasyonu yapmamızı sağlayan bir fonksiyondur. 
//Yani üretilen bir sorguya diğer ilişkisel tabloların dahil edilmesini sağlayan bir işleve sahiptir. 
//Include ile dilediğimiz kadar tabloyu dahil edebiliriz.
var employees= await context.Employees
    .Include(e=>e.Orders)
    .Include(e=>e.Region)
    .ToListAsync();
#endregion
#region ThenInclude
//Üretilen sorguda include edilen tabloların ilişkili olduğu diğer tabloları da sorguya ekleyebilmek için kullanılan bir fonksiyondur. Eğer ki üretilen sorguya include edilen navigation propertykoleksiyonel bir property ise işte o zaman bu property üzerinden diğer ilişkisel tabloya erişim gösterilememektedir. Böyle bir durumda koleksiyonel propertylerin türlerine erişip, o tür ile ilişkili diğer tabloları da sorguya eklememizi sağlayan fonksiyondur. 
//var regions = await context.Regions
//    .Include(r => r.Employees)
//    .ThenInclude(e => e.Orders)
//    .ToListAsync();
#endregion
#region Filtered Include
// Sorgulama süreçlerinde Include yaparken sonuçlar üzerinde filtreleme ve sıralama gerçekleştirebilmemizi sağlayan bir özelliktir.
//Desteklenen fonksiyonlar: Where,OrderBy, OrderByDescending, ThenBy,ThenByDescending,Skip, Take
//Change Tracker'ın aktif olduğu durumlarda Include ewdilmiş sorgular üzerindeki filtreleme sonuçları beklenmeyen olabilir. Bu durum, daha önce sorgulanmş ve Change Tracker tarafından takip edilmiş veriler arasında filtrenin gereksinimi dışında kalan veriler için söz konusu olacaktır. Bundan dolayı sağlıklı bir filtred include operasyonu için change tracker'ın kullanılmadığı sorguları tercih etmeyi düşünebilirsiniz.
var regions = await context.Regions
    .Include(r => r.Employees.Where(e => e.Name.Contains("a")).OrderByDescending(e => e.Surname)).ToListAsync();
#endregion
#region Eager Loading İçin Kritik Bir Bilgi
//EF Core, önceden üretilmiş ve execute edilerek verileri belleğe alınmış olan sorguların verileri, sonraki sorgularda KULLANIR!

//var orders = await context.Orders.ToListAsync();

//var employees = await context.Employees.ToListAsync();

#endregion
#region AutoInclude - EF Core 6
//Uygulama seviyesinde bir entitye karşılık yapılan tüm sorgulamalarda "kesinlikle" bir tabloya Include işlemi gerçekleştirlecekse eğer bunu her bir sorgu için tek tek yapmaktansa merkezi bir hale getirmemizi sağlayan özelliktir.Bu işlem onModelCreating metodu altında yapılır.

//var employees = await context.Employees.ToListAsync();
#endregion
#region IgnoreAutoIncludes
//AutoInclude konfigürasyonunu sorgu seviyesinde pasifize edebilmek için kullandığımız fonksiyondur.

//var employees = await context.Employees.IgnoreAutoIncludes().ToListAsync();
#endregion
#region Birbirlerinden Türetilmiş Entity'ler Arasında Include
#region Cast Operatörü İle Include
var persons1 = await context.Persons.Include(p => ((Employee)p).Orders).ToListAsync();
#endregion
#region as Operatörü İle Include
var persons2 = await context.Persons.Include(p => (p as Employee).Orders).ToListAsync();
#endregion
#region 2. Overload İle Include
var persons3 = await context.Persons.Include("Orders").ToListAsync();
#endregion
#endregion


#endregion
#region Explicit Loading

#endregion

public class Person
{
    public int Id { get; set; }
}

public class Employee
{
    //public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int Salary { get; set; }
    public int RegionId { get; set; }
    public Region Region { get; set; }
    public List<Order> Orders { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public DateTime OrderDate { get; set; }
}

public class Region
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Employee> Employees { get; set; }


}
class ApplicationDbContext:DbContext
{
	public DbSet<Person> Persons { get; set; }
	public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Region> Regions { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(@"Server=DESKTOP-QE6JDF1\SQLEXPRESS;Database=ApplicationDB;User Id=sa;Password=1q2w3e;TrustServerCertificate=true");
		base.OnConfiguring(optionsBuilder);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		//modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<Employee>().Navigation(e => e.Region).AutoInclude();
		base.OnModelCreating(modelBuilder);
	}
}