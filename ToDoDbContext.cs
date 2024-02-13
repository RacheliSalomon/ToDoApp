using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace TodoApi;

public partial class ToDoDbContext : DbContext
{
    public ToDoDbContext()
    {
    }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }


    public List<Item> getAllDoings(){
        return Items.ToList<Item>();
    }


    public void postDoing(Item item){
       Item i=new Item(){Name=item.Name,IsComplete=item.IsComplete};
       Items.Add(i);
       SaveChanges();
    }

    public void putDoing(int id,Item item){
       

        var res=Items.Where(i => i.Id==id).FirstOrDefault();
        if(res==null){
            res.IsComplete=item.IsComplete;
            res.Name=item.Name; 
        }
       SaveChanges();
    }
    
    public void deleteDoing(int id){
       
       Items.Remove(Items.Where(i=>i.Id==id).FirstOrDefault());
       SaveChanges();
    }
    


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=ToDoDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("items");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
