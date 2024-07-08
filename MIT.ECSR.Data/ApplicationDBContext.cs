using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MIT.ECSR.Data.Model;


namespace MIT.ECSR.Data
{
    public partial class ApplicationDBContext : DbContext
    {
        public virtual DbSet<MstPerusahaan> MstPerusahaan { get; set; }
        public virtual DbSet<RefDati> RefDati { get; set; }
        public virtual DbSet<RefJenisProgram> RefJenisProgram { get; set; }
        public virtual DbSet<RefKegiatan> RefKegiatan { get; set; }
        public virtual DbSet<RefOpd> RefOpd { get; set; }
        public virtual DbSet<RefStatus> RefStatus { get; set; }
        public virtual DbSet<RefSubProgram> RefSubProgram { get; set; }
        public virtual DbSet<SetRole> SetRole { get; set; }
        public virtual DbSet<SetUser> SetUser { get; set; }
        public virtual DbSet<TrsMedia> TrsMedia { get; set; }
        public virtual DbSet<TrsNotification> TrsNotification { get; set; }
        public virtual DbSet<TrsPenawaran> TrsPenawaran { get; set; }
        public virtual DbSet<TrsPenawaranItem> TrsPenawaranItem { get; set; }
        public virtual DbSet<TrsProgram> TrsProgram { get; set; }
        public virtual DbSet<TrsProgramItem> TrsProgramItem { get; set; }
        public virtual DbSet<TrsProgresProgram> TrsProgresProgram { get; set; }
        public virtual DbSet<TrsUsulan> TrsUsulan { get; set; }
        public virtual DbSet<TrsUsulanItem> TrsUsulanItem { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MstPerusahaan>(entity =>
            {
                entity.ToTable("MST_PERUSAHAAN");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Alamat)
                    .HasColumnType("text")
                    .HasColumnName("ALAMAT");

                entity.Property(e => e.BidangUsaha)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("BIDANG_USAHA");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdUser).HasColumnName("ID_USER");

                entity.Property(e => e.JenisPerseroan)
                    .HasMaxLength(50)
                    .HasColumnName("JENIS_PERSEROAN");

                entity.Property(e => e.NamaPerusahaan)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("NAMA_PERUSAHAAN");

                entity.Property(e => e.Nib)
                    .HasMaxLength(50)
                    .HasColumnName("NIB");

                entity.Property(e => e.Npwp)
                    .HasMaxLength(50)
                    .HasColumnName("NPWP");

                entity.Property(e => e.UpdateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.MstPerusahaan)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_MST_PERUSAHAAN_SET_USER");
            });

            modelBuilder.Entity<RefDati>(entity =>
            {
                entity.ToTable("REF_DATI");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Active).HasColumnName("ACTIVE");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("CREATE_BY");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE");

                entity.Property(e => e.KodeDati1)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("KODE_DATI1")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.KodeDati2)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("KODE_DATI2")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.KodeDati3)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("KODE_DATI3")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.KodeDati4)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("KODE_DATI4")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.NamaDati1)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NAMA_DATI1")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.NamaDati2)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NAMA_DATI2")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.NamaDati3)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NAMA_DATI3")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.NamaDati4)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NAMA_DATI4")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(40)
                    .HasColumnName("UPDATE_BY");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");
            });

            modelBuilder.Entity<RefJenisProgram>(entity =>
            {
                entity.ToTable("REF_JENIS_PROGRAM");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdSubProgram).HasColumnName("ID_SUB_PROGRAM");

                entity.Property(e => e.Kode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("KODE");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("NAME")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");

                entity.HasOne(d => d.IdSubProgramNavigation)
                    .WithMany(p => p.RefJenisProgram)
                    .HasForeignKey(d => d.IdSubProgram)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("REF_JENIS_PROGRAM_FK");
            });

            modelBuilder.Entity<RefKegiatan>(entity =>
            {
                entity.ToTable("REF_KEGIATAN");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdJenisProgram).HasColumnName("ID_JENIS_PROGRAM");

                entity.Property(e => e.Kode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("KODE")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("NAME")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");

                entity.HasOne(d => d.IdJenisProgramNavigation)
                    .WithMany(p => p.RefKegiatan)
                    .HasForeignKey(d => d.IdJenisProgram)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("REF_KEGIATAN_FK");
            });

            modelBuilder.Entity<RefOpd>(entity =>
            {
                entity.ToTable("REF_OPD");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("NAME")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");
            });

            modelBuilder.Entity<RefStatus>(entity =>
            {
                entity.ToTable("REF_STATUS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Tipe)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TIPE")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");

                entity.Property(e => e.Value).HasColumnName("VALUE");
            });

            modelBuilder.Entity<RefSubProgram>(entity =>
            {
                entity.ToTable("REF_SUB_PROGRAM");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdOpd).HasColumnName("ID_OPD");

                entity.Property(e => e.Kode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("KODE")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("NAME")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");

                entity.HasOne(d => d.IdOpdNavigation)
                    .WithMany(p => p.RefSubProgram)
                    .HasForeignKey(d => d.IdOpd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("REF_SUB_PROGRAM_FK");
            });

            modelBuilder.Entity<SetRole>(entity =>
            {
                entity.ToTable("SET_ROLE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdDati).HasColumnName("ID_DATI");

                entity.Property(e => e.IdOpd).HasColumnName("ID_OPD");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");

                entity.HasOne(d => d.IdDatiNavigation)
                    .WithMany(p => p.SetRole)
                    .HasForeignKey(d => d.IdDati)
                    .HasConstraintName("SET_ROLE_DATI_FK");

                entity.HasOne(d => d.IdOpdNavigation)
                    .WithMany(p => p.SetRole)
                    .HasForeignKey(d => d.IdOpd)
                    .HasConstraintName("SET_ROLE_OPD_FK");
            });

            modelBuilder.Entity<SetUser>(entity =>
            {
                entity.ToTable("SET_USER");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Active).HasColumnName("ACTIVE");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE");

                entity.Property(e => e.Fullname)
                    .IsUnicode(false)
                    .HasColumnName("FULLNAME")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.IdRole).HasColumnName("ID_ROLE");

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("MAIL")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("PHONE_NUMBER")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Token)
                    .HasMaxLength(250)
                    .HasColumnName("TOKEN")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(80)
                    .HasColumnName("USERNAME")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.SetUser)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SET_USER_ROLE");
            });

            modelBuilder.Entity<TrsMedia>(entity =>
            {
                entity.ToTable("TRS_MEDIA");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EXTENSION");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("FILE_NAME");

                entity.Property(e => e.IsImage).HasColumnName("IS_IMAGE");

                entity.Property(e => e.Modul)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("MODUL")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.OriginalPath)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ORIGINAL_PATH");

                entity.Property(e => e.ResizePath)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("RESIZE__PATH");

                entity.Property(e => e.Tipe)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIPE");
            });

            modelBuilder.Entity<TrsNotification>(entity =>
            {
                entity.ToTable("TRS_NOTIFICATION");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.IdUser).HasColumnName("ID_USER");

                entity.Property(e => e.IsOpen).HasColumnName("IS_OPEN");

                entity.Property(e => e.Navigation)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("NAVIGATION");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SUBJECT");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.TrsNotification)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRS_NOTIFICATION_USER");
            });

            modelBuilder.Entity<TrsPenawaran>(entity =>
            {
                entity.ToTable("TRS_PENAWARAN");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Deskripsi)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("DESKRIPSI");

                entity.Property(e => e.IdPerusahaan).HasColumnName("ID_PERUSAHAAN");

                entity.HasOne(d => d.IdPerusahaanNavigation)
                    .WithMany(p => p.TrsPenawaran)
                    .HasForeignKey(d => d.IdPerusahaan)
                    .HasConstraintName("FK_PENAWARAN_PERUSAHAAN");
            });

            modelBuilder.Entity<TrsPenawaranItem>(entity =>
            {
                entity.ToTable("TRS_PENAWARAN_ITEM");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ApprovedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("APPROVED_AT");

                entity.Property(e => e.ApprovedBy)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("APPROVED_BY");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdPenawaran).HasColumnName("ID_PENAWARAN");

                entity.Property(e => e.IdProgramItem).HasColumnName("ID_PROGRAM_ITEM");

                entity.Property(e => e.Jumlah).HasColumnName("JUMLAH");

                entity.Property(e => e.Notes)
                    .IsUnicode(false)
                    .HasColumnName("NOTES")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Rupiah).HasColumnName("RUPIAH");

                entity.Property(e => e.Status).HasColumnName("STATUS");

                entity.HasOne(d => d.IdPenawaranNavigation)
                    .WithMany(p => p.TrsPenawaranItem)
                    .HasForeignKey(d => d.IdPenawaran)
                    .HasConstraintName("FK_PENAWARAN_ITEM_PENAWARAN");

                entity.HasOne(d => d.IdProgramItemNavigation)
                    .WithMany(p => p.TrsPenawaranItem)
                    .HasForeignKey(d => d.IdProgramItem)
                    .HasConstraintName("FK_PENAWARAN_ITEM_PROGRAM_ITEM");
            });

            modelBuilder.Entity<TrsProgram>(entity =>
            {
                entity.ToTable("TRS_PROGRAM");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ApprovedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("APPROVED_AT");

                entity.Property(e => e.ApprovedBy)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("APPROVED_BY");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Deskripsi)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("DESKRIPSI");

                entity.Property(e => e.EndProgramKerja)
                    .HasColumnType("datetime")
                    .HasColumnName("END_PROGRAM_KERJA");

                entity.Property(e => e.EndTglPelaksanaan)
                    .HasColumnType("datetime")
                    .HasColumnName("END_TGL_PELAKSANAAN");

                entity.Property(e => e.IdJenisProgram).HasColumnName("ID_JENIS_PROGRAM");

                entity.Property(e => e.Lokasi).HasColumnName("LOKASI");

                entity.Property(e => e.NamaProgram).HasColumnName("NAMA_PROGRAM");

                entity.Property(e => e.Notes)
                    .IsUnicode(false)
                    .HasColumnName("NOTES")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.StartTglPelaksanaan)
                    .HasColumnType("datetime")
                    .HasColumnName("START_TGL_PELAKSANAAN");

                entity.Property(e => e.Status).HasColumnName("STATUS");

                entity.Property(e => e.UpdateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");

                entity.HasOne(d => d.IdJenisProgramNavigation)
                    .WithMany(p => p.TrsProgram)
                    .HasForeignKey(d => d.IdJenisProgram)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROGRAM_JENIS_PROGRAM");

                entity.HasOne(d => d.LokasiNavigation)
                    .WithMany(p => p.TrsProgram)
                    .HasForeignKey(d => d.Lokasi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROGRAM_LOKASI");

                entity.HasOne(d => d.NamaProgramNavigation)
                    .WithMany(p => p.TrsProgram)
                    .HasForeignKey(d => d.NamaProgram)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TRS_PROGRAM_FK");
            });

            modelBuilder.Entity<TrsProgramItem>(entity =>
            {
                entity.ToTable("TRS_PROGRAM_ITEM");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndTglPelaksanaan)
                    .HasColumnType("datetime")
                    .HasColumnName("END_TGL_PELAKSANAAN");

                entity.Property(e => e.IdProgram).HasColumnName("ID_PROGRAM");

                entity.Property(e => e.Jumlah).HasColumnName("JUMLAH");

                entity.Property(e => e.Lokasi)
                    .IsUnicode(false)
                    .HasColumnName("LOKASI");

                entity.Property(e => e.Nama)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("NAMA");

                entity.Property(e => e.Progress).HasColumnName("PROGRESS");

                entity.Property(e => e.Rupiah).HasColumnName("RUPIAH");

                entity.Property(e => e.SatuanUnit)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SATUAN_UNIT");

                entity.Property(e => e.StartTglPelaksanaan)
                    .HasColumnType("datetime")
                    .HasColumnName("START_TGL_PELAKSANAAN");

                entity.Property(e => e.UpdateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");

                entity.HasOne(d => d.IdProgramNavigation)
                    .WithMany(p => p.TrsProgramItem)
                    .HasForeignKey(d => d.IdProgram)
                    .HasConstraintName("FK_PROGRAM_ITEM_PERENCANAAN_PROGRAM");
            });

            modelBuilder.Entity<TrsProgresProgram>(entity =>
            {
                entity.ToTable("TRS_PROGRES_PROGRAM");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ApprovedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("APPROVED_AT");

                entity.Property(e => e.ApprovedBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("APPROVED_BY");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Deskripsi)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("DESKRIPSI");

                entity.Property(e => e.IdPerusahaan).HasColumnName("ID_PERUSAHAAN");

                entity.Property(e => e.IdProgramItem).HasColumnName("ID_PROGRAM_ITEM");

                entity.Property(e => e.Notes)
                    .IsUnicode(false)
                    .HasColumnName("NOTES");

                entity.Property(e => e.Progress).HasColumnName("PROGRESS");

                entity.Property(e => e.Status).HasColumnName("STATUS");

                entity.Property(e => e.TglProgress)
                    .HasColumnType("datetime")
                    .HasColumnName("TGL_PROGRESS");

                entity.HasOne(d => d.IdPerusahaanNavigation)
                    .WithMany(p => p.TrsProgresProgram)
                    .HasForeignKey(d => d.IdPerusahaan)
                    .HasConstraintName("FK_TRS_PROGRES_PROGRAM_MST_PERUSAHAAN");

                entity.HasOne(d => d.IdProgramItemNavigation)
                    .WithMany(p => p.TrsProgresProgram)
                    .HasForeignKey(d => d.IdProgramItem)
                    .HasConstraintName("FK_PROGRES_PROGRAM_PROGRAM_ITEM");
            });

            modelBuilder.Entity<TrsUsulan>(entity =>
            {
                entity.ToTable("TRS_USULAN");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ApprovedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("APPROVED_AT");

                entity.Property(e => e.ApprovedBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("APPROVED_BY");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Deskripsi)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("DESKRIPSI");

                entity.Property(e => e.EndTglPelaksanaan)
                    .HasColumnType("datetime")
                    .HasColumnName("END_TGL_PELAKSANAAN");

                entity.Property(e => e.IdJenisProgram).HasColumnName("ID_JENIS_PROGRAM");

                entity.Property(e => e.IdPerusahaan).HasColumnName("ID_PERUSAHAAN");

                entity.Property(e => e.Lokasi)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("LOKASI");

                entity.Property(e => e.NamaProgram)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("NAMA_PROGRAM");

                entity.Property(e => e.Notes)
                    .IsUnicode(false)
                    .HasColumnName("NOTES");

                entity.Property(e => e.StartTglPelaksanaan)
                    .HasColumnType("datetime")
                    .HasColumnName("START_TGL_PELAKSANAAN");

                entity.Property(e => e.Status).HasColumnName("STATUS");

                entity.HasOne(d => d.IdJenisProgramNavigation)
                    .WithMany(p => p.TrsUsulan)
                    .HasForeignKey(d => d.IdJenisProgram)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USULAN_JENIS_PROGRAM");

                entity.HasOne(d => d.IdPerusahaanNavigation)
                    .WithMany(p => p.TrsUsulan)
                    .HasForeignKey(d => d.IdPerusahaan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USULAN_PERUSAHAAN");
            });

            modelBuilder.Entity<TrsUsulanItem>(entity =>
            {
                entity.ToTable("TRS_USULAN_ITEM");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdUsulan).HasColumnName("ID_USULAN");

                entity.Property(e => e.Jumlah).HasColumnName("JUMLAH");

                entity.Property(e => e.Nama)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("NAMA");

                entity.Property(e => e.Rupiah).HasColumnName("RUPIAH");

                entity.Property(e => e.SatuanUnit)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SATUAN_UNIT");

                entity.Property(e => e.UpdateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");

                entity.HasOne(d => d.IdUsulanNavigation)
                    .WithMany(p => p.TrsUsulanItem)
                    .HasForeignKey(d => d.IdUsulan)
                    .HasConstraintName("FK_USULAN_ITEM_PERENCANAAN_PROGRAM");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
