namespace MTGProject.Models;



using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;



public partial class MyDbContext : DbContext
{

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<CardColor> CardColors { get; set; }

    public virtual DbSet<CardType> CardTypes { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Format> Formats { get; set; }

    public virtual DbSet<Migration> Migrations { get; set; }

    public virtual DbSet<PersonalAccessToken> PersonalAccessTokens { get; set; }

    public virtual DbSet<Rarity> Rarities { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<Type> Types { get; set; }
    
   public virtual DbSet<Favorite> Favorites { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("User Id=postgres.sfxrgsbklirwpsieaphr;Password=Polderke1202;Server=aws-0-eu-west-3.pooler.supabase.com;Port=5432;Database=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("pgsodium", "key_status", new[] { "default", "valid", "invalid", "expired" })
            .HasPostgresEnum("pgsodium", "key_type", new[] { "aead-ietf", "aead-det", "hmacsha512", "hmacsha256", "auth", "shorthash", "generichash", "kdf", "secretbox", "secretstream", "stream_xchacha20" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "pgjwt")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("pgsodium", "pgsodium")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("artists_pkey");

            entity.ToTable("artists");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("updated_at");
        });
        
        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("favorites_pkey");

            entity.ToTable("favorites");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("user_id");
            entity.Property(e => e.CardId).HasColumnName("card_id");

            entity.HasOne(d => d.Card)
                .WithMany()
                .HasForeignKey(d => d.CardId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("favorites_card_id_foreign");
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cards_pkey");

            entity.ToTable("cards");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArtistId).HasColumnName("artist_id");
            entity.Property(e => e.ConvertedManaCost)
                .HasMaxLength(255)
                .HasColumnName("converted_mana_cost");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Flavor).HasColumnName("flavor");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Layout)
                .HasMaxLength(255)
                .HasColumnName("layout");
            entity.Property(e => e.ManaCost)
                .HasMaxLength(255)
                .HasColumnName("mana_cost");
            entity.Property(e => e.MtgId)
                .HasMaxLength(255)
                .HasColumnName("mtg_id");
            entity.Property(e => e.MultiverseId).HasColumnName("multiverse_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Number)
                .HasMaxLength(255)
                .HasColumnName("number");
            entity.Property(e => e.OriginalImageUrl)
                .HasMaxLength(255)
                .HasColumnName("original_image_url");
            entity.Property(e => e.OriginalText).HasColumnName("original_text");
            entity.Property(e => e.OriginalType)
                .HasMaxLength(255)
                .HasColumnName("original_type");
            entity.Property(e => e.Power)
                .HasMaxLength(255)
                .HasColumnName("power");
            entity.Property(e => e.RarityCode)
                .HasMaxLength(255)
                .HasColumnName("rarity_code");
            entity.Property(e => e.SetCode)
                .HasMaxLength(255)
                .HasColumnName("set_code");
            entity.Property(e => e.Text).HasColumnName("text");
            entity.Property(e => e.Toughness)
                .HasMaxLength(255)
                .HasColumnName("toughness");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Variations)
                .HasColumnType("json")
                .HasColumnName("variations");

            entity.HasOne(d => d.Artist).WithMany(p => p.Cards)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("cards_artist_id_foreign");

            entity.HasOne(d => d.RarityCodeNavigation).WithMany(p => p.Cards)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.RarityCode)
                .HasConstraintName("cards_rarity_code_foreign");

            entity.HasOne(d => d.SetCodeNavigation).WithMany(p => p.Cards)
                .HasPrincipalKey(p => p.Code)
                .HasForeignKey(d => d.SetCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cards_set_code_foreign");
        });

        modelBuilder.Entity<CardColor>(entity =>
        {
            entity.HasKey(e => new { e.CardId, e.ColorId }).HasName("card_colors_pkey");

            entity.ToTable("card_colors");

            entity.Property(e => e.CardId).HasColumnName("card_id");
            entity.Property(e => e.ColorId).HasColumnName("color_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Card).WithMany(p => p.CardColors)
                .HasForeignKey(d => d.CardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("card_colors_card_id_foreign");

            entity.HasOne(d => d.Color).WithMany(p => p.CardColors)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("card_colors_color_id_foreign");
        });

        modelBuilder.Entity<CardType>(entity =>
        {
            entity.HasKey(e => new { e.CardId, e.TypeId }).HasName("card_types_pkey");

            entity.ToTable("card_types");

            entity.Property(e => e.CardId).HasColumnName("card_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Card).WithMany(p => p.CardTypes)
                .HasForeignKey(d => d.CardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("card_types_card_id_foreign");

            entity.HasOne(d => d.Type).WithMany(p => p.CardTypes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("card_types_type_id_foreign");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("colors_pkey");

            entity.ToTable("colors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Format>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("formats_pkey");

            entity.ToTable("formats");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Migration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("migrations_pkey");

            entity.ToTable("migrations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Batch).HasColumnName("batch");
            entity.Property(e => e.Migration1)
                .HasMaxLength(255)
                .HasColumnName("migration");
        });

        modelBuilder.Entity<PersonalAccessToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("personal_access_tokens_pkey");

            entity.ToTable("personal_access_tokens");

            entity.HasIndex(e => e.Token, "personal_access_tokens_token_unique").IsUnique();

            entity.HasIndex(e => new { e.TokenableType, e.TokenableId }, "personal_access_tokens_tokenable_type_tokenable_id_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Abilities).HasColumnName("abilities");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.LastUsedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("last_used_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Token)
                .HasMaxLength(64)
                .HasColumnName("token");
            entity.Property(e => e.TokenableId).HasColumnName("tokenable_id");
            entity.Property(e => e.TokenableType)
                .HasMaxLength(255)
                .HasColumnName("tokenable_type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Rarity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rarities_pkey");

            entity.ToTable("rarities");

            entity.HasIndex(e => e.Code, "rarities_code_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sets_pkey");

            entity.ToTable("sets");

            entity.HasIndex(e => e.Code, "sets_code_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("types_pkey");

            entity.ToTable("types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Type1)
                .HasMaxLength(255)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
