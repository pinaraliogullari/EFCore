using Microsoft.EntityFrameworkCore;

EFCoreTrainingDBContext context = new();
#region AsNoTracking
//Context üzerinden gelen tüm datalar Change Tracker mekanizması tarafından takip edilmektedir.

//Change Tracker, takip ettiği nesnelerin sayısıyla doğru orantılı olacak şekilde bir maliyete sahiptir. O yüzden üzerinde işlem yapılmayacak verilerin takip edilmesi bizlere lüzumsuz yere bir maliyet ortaya çıkaracaktır.

//AsNoTracking metodu, context üzerinden sorgu neticesinde gelecek olan verilerin Change Tracker tarafından takip edilmesini engeller.

//AsNoTracking metodu ile Change Tracker'ın ihtiyaç olmayan verilerdeki maliyetini törpülemiş oluruz.

//AsNoTracking fonksiyonu ile yapılan sorgulamalarda, verileri elde edebilir, bu verileri istenilen noktalarda kullanabilir lakin veriler üzerinde herhangi bir değişiklik/update işlemi yapamayız.

var kullanicilar =await context.Kullanicilar.AsNoTracking().ToListAsync();
foreach (var kullanici in kullanicilar)
{
	Console.WriteLine(kullanici.Adi);
	kullanici.Adi = $"yeni {kullanici}";
	context.Kullanicilar.Update(kullanici); //tracking i kestiğimiz için savechangestaki track mekanizması devre dışı kalacaktır. bu sebeple nesne takip edilmediği için update fonk kullanılır. 
}
await context.SaveChangesAsync();
#endregion
public class EFCoreTrainingDBContext : DbContext
{
    public DbSet<Kullanici> Kullanicilar { get; set; }
    public DbSet<Rol> Roller { get; set; }
    public DbSet<Kitap> Kitaplar { get; set; }
    public DbSet<Yazar> Yazarlar { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(@"Server=DESKTOP-QE6JDF1\SQLEXPRESS;Database=EFCoreTrainingDB;User Id=sa;Password=1q2w3e;TrustServerCertificate=true");
		base.OnConfiguring(optionsBuilder);
	}
}



public class Kullanici
{
	public int Id { get; set; }
	public string Adi { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }

	public ICollection<Rol> Roller { get; set; }
}
public class Rol
{
	public int Id { get; set; }
	public string RolAdi { get; set; }
	public ICollection<Kullanici> Kullanicilar { get; set; }
}
public class Kitap
{
    public Kitap() => Console.WriteLine("Kitap nesnesi oluşturuldu.");
	public int Id { get; set; }
	public string KitapAdi { get; set; }
	public int SayfaSayisi { get; set; }

	public ICollection<Yazar> Yazarlar { get; set; }
}
public class Yazar
{
	public Yazar() => Console.WriteLine("Yazar nesnesi oluşturuldu.");
	public int Id { get; set; }
	public string YazarAdi { get; set; }

	public ICollection<Kitap> Kitaplar { get; set; }
}