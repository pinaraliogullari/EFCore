using Microsoft.EntityFrameworkCore;

Console.Read();
ApplicationDbContext context = new();


#region One to One İlişkisel Senaryolarda Veri Ekleme
//#region 1. Yöntem -> Principal Entity Üzerinden Dependent Entity Verisi Ekleme
//Person person = new();
//person.Name = "Pinar";
//person.Address = new() { PersonAddress = "Pendik/İSTANBUL" };

//await context.AddAsync(person);
//await context.SaveChangesAsync();

//#endregion

////Eğer ki principal entity üzerinden ekleme gerçekleştiriliyorsa dependent entity nesnesi verilmek zorunda değildir! Amma velakin, dependent entity üzerinden ekleme işlemi gerçekleştiriliyorsa eğer burada principal entitynin nesnesine ihtiyacımız zaruridir.

//#region 2. Yöntem -> Dependent Entity Üzerinden Principal Entity Verisi Ekleme

//Address address = new()
//{
//    PersonAddress = "Kadıköy/İSTANBUL",
//    Person = new() { Name = "Emre" }
//};

//await context.AddAsync(address);
//await context.SaveChangesAsync();
//#endregion

//class Person
//{
//    public int Id { get; set; }
//    public string Name { get; set; }

//    public Address Address { get; set; }
//}
//class Address
//{
//    public int Id { get; set; }
//    public string PersonAddress { get; set; }

//    public Person Person { get; set; }
//}
//class ApplicationDbContext : DbContext
//{
//    public DbSet<Person> Persons { get; set; }
//    public DbSet<Address> Addresses { get; set; }
//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//		optionsBuilder.UseSqlServer(@"Server=DESKTOP-QE6JDF1\SQLEXPRESS;Database=EFCoreTrainingDB;User Id=sa;Password=1q2w3e;TrustServerCertificate=true");
//	}

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Address>()
//            .HasOne(a => a.Person)
//            .WithOne(p => p.Address)
//            .HasForeignKey<Address>(a => a.Id);
//    }
//}
#endregion

#region One To Many İlişkisel Senaryolarda Veri Ekleme
#region 1. Yöntem-> Principal Entity Üzerinden Dependent Entity Verisi Ekleme
#region Nesne Referansı Üzerinden Ekleme
//Blog blog = new() { Name="pinaraliogullari.com"};
//blog.Posts.Add(new Post() { Title = "A Blog" });
//blog.Posts.Add(new Post() { Title = "B Blog" });
//blog.Posts.Add(new Post() { Title = "C Blog" });

//await context.AddAsync(blog);
//await context.SaveChangesAsync();
#endregion
#region Object Initilazier Üzerinden Ekleme
//Blog blog2 = new()
//{
//	Name = "A Blog",
//	Posts = new HashSet<Post>() { new() { Title = "Post 4" }, new() { Title = "Post 5" } }
//};
//await context.AddAsync(blog2);
//await context.SaveChangesAsync();
#endregion
#endregion
#region 2.Yöntem ->Dependent Entity Üzerinden Principal Entity Verisi Ekleme
//tercih edilmez.
//Post post = new()
//{
//	Title = "Post 6",
//	Blog = new() { Name = "B Blog" }
//};
#endregion
#region 3.Yöntem-> Foreign Key Kolonu Üzerinden Veri Ekleme- Önceden var olan principal entitye karşılık yeni dependent entityler oluşturmak gerektiğinde
//1. ve 2. yöntemler hiç olmayan verilerin ilişkisel olarak eklenmesini sağlarken , bu  3. yöntem önceden eklenmiş olan bir principal entity verisi ile yeni dependent entitylerin ilişkisel olarak eşleştirilmesini sağlamaktadır. 
Post post = new()
{
	BlogId = 1,
	Title = "Post 7",
};
#endregion
class Blog
{
    public Blog()
    {
        Posts= new HashSet<Post>();    // nesne referansı üzerinden veri ekleme yönteminde  nesne referansı üzerinden eriştiğimiz koleksiyonel propertynin kesinlikle boş olmaması gerekir. bu sebeple constructorda koleksiyonel nesne oluşturuyoruz.
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Post> Posts { get; set; }
}
class Post
{
    public int Id  { get; set; }
    public string Title { get; set; }
    public int BlogId { get; set; }
    public Blog Blog { get; set; }

}
class ApplicationDbContext : DbContext
{
	public DbSet<Blog> Blogs { get; set; }
	public DbSet<Post> Posts { get; set; }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(@"Server=DESKTOP-QE6JDF1\SQLEXPRESS;Database=EFCoreTrainingDB;User Id=sa;Password=1q2w3e;TrustServerCertificate=true");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Post>()
			.HasOne(p => p.Blog)
			.WithMany(b => b.Posts)
			.HasForeignKey(p => p.BlogId);
	}
}
#endregion

#region Many To Many İlişkisel Senaryolarda Veri Ekleme
#region 1. Yöntem
// n to n ilişkisi default convention üzerinden tasarlanmışsa kullanılan bir yöntemdir.
//Book book = new()
//{
//	BookName = "Test",
//	Authors = new HashSet<Author>()
//	{
//		new() { AuthorName = "a" },
//		new() { AuthorName = "b" },
//		new() { AuthorName = "c" }
//	}
//};
//await context.Books.AddAsync(book);
//await context.SaveChangesAsync();
//class Book
//{
//	public Book()
//	{
//		Authors = new HashSet<Author>();
//	}
//	public int Id { get; set; }
//	public string BookName { get; set; }
//	public ICollection<Author> Authors { get; set; }
//}


//class Author
//{
//	public Author()
//	{
//		Books = new HashSet<Book>();
//	}
//	public int Id { get; set; }
//	public string AuthorName { get; set; }
//	public ICollection<Book> Books { get; set; }
//}

#endregion
#region 2. Yöntem
//n to n ilişkisi fluent api üzerinden tasarlanmışsa kullanılan bir yöntemdir.
//Author author = new()
//				{
//					authorname = "ufuk",
//					books = new hashset<bookauthor>()
//	{
//		new() { bookıd = 1 }, // var olan kitap ile yazarı ilişkilendirerek ekleme
//		new() { book = new() { bookname = "b kitap" } // yeni kitap ile yazarı ilişkilendirerek ekleme
//		}
//	};

//await context.Authors.AddAsync(author);
//await context.SaveChangesAsync();
//class Book
//{
//	public Book()
//	{
//		BookAuthors = new HashSet<BookAuthor>();
//	}
//	public int Id { get; set; }
//	public string BookName { get; set; }
//	public ICollection<BookAuthor> BookAuthors { get; set; }
//}

//class BookAuthor
//{
//	public int BookId { get; set; }
//	public Book Book { get; set; }
//	public int AuthorId { get; set; }
//	public Author Author { get; set; }
//}

//class Author
//{
//	public Author()
//	{
//		BookAuthors = new HashSet<BookAuthor>();
//	}
//	public int Id { get; set; }
//	public string AuthorName { get; set; }
//	public ICollection<BookAuthor> BookAuthors { get; set; }
//}

#endregion


#endregion



//class ApplicationDBContext : DbContext
//{
//	public DbSet<Book> Books { get; set; }
//	public DbSet<Author> Authors { get; set; }
//	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//	{
//		optionsBuilder.UseSqlServer(@"Server=DESKTOP-QE6JDF1\SQLEXPRESS;Database=EFCoreTrainingDB;User Id=sa;Password=1q2w3e;TrustServerCertificate=true");
//		base.OnConfiguring(optionsBuilder);
//	}

//	protected override void OnModelCreating(ModelBuilder modelBuilder)
//	{
//		modelBuilder.Entity<BookAuthor>().HasKey(ba => new { ba.AuthorId, ba.BookId });
//		modelBuilder.Entity<BookAuthor>().HasOne(ba => ba.Book).WithMany(b => b.BookAuthors).HasForeignKey(ba => ba.BookId);
//		modelBuilder.Entity<BookAuthor>().HasOne(ba => ba.Author).WithMany(a => a.BookAuthors).HasForeignKey(ba => ba.AuthorId);
//		base.OnModelCreating(modelBuilder);
//	}
//}


