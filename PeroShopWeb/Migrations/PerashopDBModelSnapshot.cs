﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PeroShopWeb.Models;

#nullable disable

namespace PeroShopWeb.Migrations
{
    [DbContext(typeof(PerashopDB))]
    partial class PerashopDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PeroShopWeb.Models.Carrito", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PrecioTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("RutaImagen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Carrito");
                });

            modelBuilder.Entity("PeroShopWeb.Models.Direccion", b =>
                {
                    b.Property<int>("iD")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("iD"));

                    b.Property<string>("ApellidoMaterno")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApellidoPaterno")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Calle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodigoPostal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Colonia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Delegacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("iD");

                    b.ToTable("Direccion");
                });

            modelBuilder.Entity("PeroShopWeb.Models.Producto", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Activo")
                        .HasColumnType("int");

                    b.Property<string>("Caracteristicas")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Existencia")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PrecioCompra")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PrecioVenta")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("RutaImagen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("idproveedor")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("idproveedor");

                    b.ToTable("Producto");
                });

            modelBuilder.Entity("PeroShopWeb.Models.Proveedores", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Activo")
                        .HasColumnType("int");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonaContacto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("iddireccion")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("iddireccion");

                    b.ToTable("Proveedores");
                });

            modelBuilder.Entity("PeroShopWeb.Models.Usuario", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Contrasena")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DireccionImagen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoUsuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("iddireccion")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("iddireccion");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("PeroShopWeb.Models.Venta", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("IVA")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("idusuario")
                        .HasColumnType("int");

                    b.Property<int>("idusuarios")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("idusuarios");

                    b.ToTable("Venta");
                });

            modelBuilder.Entity("PeroShopWeb.Models.Producto", b =>
                {
                    b.HasOne("PeroShopWeb.Models.Proveedores", "Proveedor")
                        .WithMany()
                        .HasForeignKey("idproveedor")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Proveedor");
                });

            modelBuilder.Entity("PeroShopWeb.Models.Proveedores", b =>
                {
                    b.HasOne("PeroShopWeb.Models.Direccion", "Direccion")
                        .WithMany()
                        .HasForeignKey("iddireccion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Direccion");
                });

            modelBuilder.Entity("PeroShopWeb.Models.Usuario", b =>
                {
                    b.HasOne("PeroShopWeb.Models.Direccion", "Direccion")
                        .WithMany()
                        .HasForeignKey("iddireccion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Direccion");
                });

            modelBuilder.Entity("PeroShopWeb.Models.Venta", b =>
                {
                    b.HasOne("PeroShopWeb.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("idusuarios")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });
#pragma warning restore 612, 618
        }
    }
}
