﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebBanThatLung.Repositoty;

#nullable disable

namespace WebBanThatLung.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240722043611_udsfg")]
    partial class udsfg
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebBanThatLung.Models.ChiTietDonHangModel", b =>
                {
                    b.Property<int>("ID_CHI_TIET_DON_HANG")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_CHI_TIET_DON_HANG"), 1L, 1);

                    b.Property<decimal>("GIA")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ID_DON_HANG")
                        .HasColumnType("int");

                    b.Property<int>("ID_SAN_PHAM")
                        .HasColumnType("int");

                    b.Property<int>("SO_LUONG")
                        .HasColumnType("int");

                    b.Property<decimal>("THANH_TIEN")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ID_CHI_TIET_DON_HANG");

                    b.HasIndex("ID_DON_HANG");

                    b.HasIndex("ID_SAN_PHAM");

                    b.ToTable("CHI_TIET_DON_HANGs");
                });

            modelBuilder.Entity("WebBanThatLung.Models.DonHangModel", b =>
                {
                    b.Property<int>("ID_DON_HANG")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_DON_HANG"), 1L, 1);

                    b.Property<string>("DIACHI")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HinhThucThanhToan")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ID_NGUOI_DUNG")
                        .HasColumnType("int");

                    b.Property<int?>("ID_NHAN_VIEN")
                        .HasColumnType("int");

                    b.Property<string>("LY_DO_HUY")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NGAY_DAT")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("NGAY_GIAO")
                        .HasColumnType("datetime2");

                    b.Property<string>("TEN_NGUOI_NHAN")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<int>("TRANG_THAI_DH")
                        .HasColumnType("int");

                    b.Property<string>("TRANG_THAI_THANH_THAM")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_DON_HANG");

                    b.HasIndex("ID_NGUOI_DUNG");

                    b.HasIndex("ID_NHAN_VIEN");

                    b.ToTable("DON_HANGs");
                });

            modelBuilder.Entity("WebBanThatLung.Models.GioHangModel", b =>
                {
                    b.Property<int>("ID_GIO_HANG")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_GIO_HANG"), 1L, 1);

                    b.Property<decimal>("GIA")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ID_NGUOI_DUNG")
                        .HasColumnType("int");

                    b.Property<int>("ID_SAN_PHAM")
                        .HasColumnType("int");

                    b.Property<int>("MAU_SP")
                        .HasColumnType("int");

                    b.Property<int>("SO_LUONG_GH")
                        .HasColumnType("int");

                    b.Property<decimal>("THANH_TIEN")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ID_GIO_HANG");

                    b.HasIndex("ID_NGUOI_DUNG");

                    b.HasIndex("ID_SAN_PHAM");

                    b.ToTable("GIO_HANGs");
                });

            modelBuilder.Entity("WebBanThatLung.Models.HinhAnhModel", b =>
                {
                    b.Property<int>("ID_HINH_ANH")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_HINH_ANH"), 1L, 1);

                    b.Property<int>("ID_SAN_PHAM")
                        .HasColumnType("int");

                    b.Property<string>("TEN_HINH_ANH")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_HINH_ANH");

                    b.HasIndex("ID_SAN_PHAM");

                    b.ToTable("HINH_ANHs");
                });

            modelBuilder.Entity("WebBanThatLung.Models.LoaiSanPhamModel", b =>
                {
                    b.Property<int>("ID_LOAI_SAN_PHAM")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_LOAI_SAN_PHAM"), 1L, 1);

                    b.Property<string>("TEN_LOAI_SAN_PHAM")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_LOAI_SAN_PHAM");

                    b.ToTable("LOAI_SAN_PHAMs");
                });

            modelBuilder.Entity("WebBanThatLung.Models.MauModel", b =>
                {
                    b.Property<int>("ID_MAU")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_MAU"), 1L, 1);

                    b.Property<string>("MAU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_MAU");

                    b.ToTable("MAUs");
                });

            modelBuilder.Entity("WebBanThatLung.Models.NguoiDungModel", b =>
                {
                    b.Property<int>("ID_NGUOI_DUNG")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_NGUOI_DUNG"), 1L, 1);

                    b.Property<string>("CCCD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EMAIL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HINH_ANH")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HO_TEN")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("MAT_KHAU")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("NGAY_SINH")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NGAY_TAO")
                        .HasColumnType("datetime2");

                    b.Property<string>("SDT")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TAI_KHOAN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TRANG_THAI")
                        .HasColumnType("int");

                    b.Property<int>("VAI_TRO")
                        .HasColumnType("int");

                    b.HasKey("ID_NGUOI_DUNG");

                    b.ToTable("NGUOI_DUNGs");
                });

            modelBuilder.Entity("WebBanThatLung.Models.SanPhamModel", b =>
                {
                    b.Property<int>("ID_SAN_PHAM")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_SAN_PHAM"), 1L, 1);

                    b.Property<string>("CHAT_LIEU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("GIA")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ID_LOAI_SAN_PHAM")
                        .HasColumnType("int");

                    b.Property<int>("ID_MAU")
                        .HasColumnType("int");

                    b.Property<int>("ID_THUONG_HIEU")
                        .HasColumnType("int");

                    b.Property<string>("MO_TA")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NGAY_TAO")
                        .HasColumnType("datetime2");

                    b.Property<int>("SO_LUONG")
                        .HasColumnType("int");

                    b.Property<string>("TEN_SAN_PHAM")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_SAN_PHAM");

                    b.HasIndex("ID_LOAI_SAN_PHAM");

                    b.HasIndex("ID_MAU");

                    b.HasIndex("ID_THUONG_HIEU");

                    b.ToTable("SAN_PHAMs");
                });

            modelBuilder.Entity("WebBanThatLung.Models.ThuongHieuModel", b =>
                {
                    b.Property<int>("ID_THUONG_HIEU")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_THUONG_HIEU"), 1L, 1);

                    b.Property<string>("TEN_THUONG_HIEU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_THUONG_HIEU");

                    b.ToTable("THUONG_HIEUs");
                });

            modelBuilder.Entity("WebBanThatLung.Models.ChiTietDonHangModel", b =>
                {
                    b.HasOne("WebBanThatLung.Models.DonHangModel", "DON_HANG")
                        .WithMany("CHI_TIET_DON_HANG")
                        .HasForeignKey("ID_DON_HANG")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebBanThatLung.Models.SanPhamModel", "SAN_PHAM")
                        .WithMany()
                        .HasForeignKey("ID_SAN_PHAM")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DON_HANG");

                    b.Navigation("SAN_PHAM");
                });

            modelBuilder.Entity("WebBanThatLung.Models.DonHangModel", b =>
                {
                    b.HasOne("WebBanThatLung.Models.NguoiDungModel", "NGUOI_DUNG")
                        .WithMany()
                        .HasForeignKey("ID_NGUOI_DUNG")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebBanThatLung.Models.NguoiDungModel", "NHAN_VIEN")
                        .WithMany()
                        .HasForeignKey("ID_NHAN_VIEN");

                    b.Navigation("NGUOI_DUNG");

                    b.Navigation("NHAN_VIEN");
                });

            modelBuilder.Entity("WebBanThatLung.Models.GioHangModel", b =>
                {
                    b.HasOne("WebBanThatLung.Models.NguoiDungModel", "NGUOI_DUNG")
                        .WithMany()
                        .HasForeignKey("ID_NGUOI_DUNG")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebBanThatLung.Models.SanPhamModel", "SAN_PHAM")
                        .WithMany()
                        .HasForeignKey("ID_SAN_PHAM")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NGUOI_DUNG");

                    b.Navigation("SAN_PHAM");
                });

            modelBuilder.Entity("WebBanThatLung.Models.HinhAnhModel", b =>
                {
                    b.HasOne("WebBanThatLung.Models.SanPhamModel", "SAN_PHAM")
                        .WithMany("HINH_ANH")
                        .HasForeignKey("ID_SAN_PHAM")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SAN_PHAM");
                });

            modelBuilder.Entity("WebBanThatLung.Models.SanPhamModel", b =>
                {
                    b.HasOne("WebBanThatLung.Models.LoaiSanPhamModel", "LOAI_SAN_PHAM")
                        .WithMany()
                        .HasForeignKey("ID_LOAI_SAN_PHAM")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebBanThatLung.Models.MauModel", "MAU")
                        .WithMany()
                        .HasForeignKey("ID_MAU")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebBanThatLung.Models.ThuongHieuModel", "THUONG_HIEU")
                        .WithMany()
                        .HasForeignKey("ID_THUONG_HIEU")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LOAI_SAN_PHAM");

                    b.Navigation("MAU");

                    b.Navigation("THUONG_HIEU");
                });

            modelBuilder.Entity("WebBanThatLung.Models.DonHangModel", b =>
                {
                    b.Navigation("CHI_TIET_DON_HANG");
                });

            modelBuilder.Entity("WebBanThatLung.Models.SanPhamModel", b =>
                {
                    b.Navigation("HINH_ANH");
                });
#pragma warning restore 612, 618
        }
    }
}
