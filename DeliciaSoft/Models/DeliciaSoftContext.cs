using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeliciaSoft.Models;

public partial class DeliciaSoftContext : IdentityDbContext
{
    public DeliciaSoftContext()
    {
    }

    public DeliciaSoftContext(DbContextOptions<DeliciaSoftContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriaInsumo> CategoriaInsumos { get; set; }

    public virtual DbSet<CategoriaProducto> CategoriaProductos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<DetalleProduccion> DetalleProduccions { get; set; }

    public virtual DbSet<DetalleProductoGeneral> DetalleProductoGenerals { get; set; }

    public virtual DbSet<DetalleVentum> DetalleVenta { get; set; }

    public virtual DbSet<EstadoProduccion> EstadoProduccions { get; set; }

    public virtual DbSet<Insumo> Insumos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<PersonaJuridica> PersonaJuridicas { get; set; }

    public virtual DbSet<PersonaNatural> PersonaNaturals { get; set; }

    public virtual DbSet<Produccion> Produccions { get; set; }

    public virtual DbSet<ProduccionEstado> ProduccionEstados { get; set; }

    public virtual DbSet<ProductoCompra> ProductoCompras { get; set; }

    public virtual DbSet<ProductoGeneral> ProductoGenerals { get; set; }

    public virtual DbSet<ProductoPersonalizado> ProductoPersonalizados { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Rol> Roles { get; set; }
    public virtual DbSet<RolPermiso> RolPermisos { get; set; }

    public virtual DbSet<Sede> Sedes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=DESKTOP-RK8VDF5;Initial Catalog=DeliciaSoft;Integrated Security=True;TrustServerCertificate=True");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CategoriaInsumo>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaInsumos).HasName("PK__Categori__85A0A3B34BF23D91");

            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.NombreCategoria).HasMaxLength(100);
        });

        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaProducto).HasName("PK__Categori__8A4F21C3CF92C3F3");

            entity.ToTable("CategoriaProducto");

            entity.Property(e => e.DescripcionProducto).HasMaxLength(300);
            entity.Property(e => e.NombreCategoria).HasMaxLength(100);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Cliente__D5946642C5F1138F");

            entity.ToTable("Cliente");

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Barrio).HasMaxLength(100);
            entity.Property(e => e.Celular).HasMaxLength(20);
            entity.Property(e => e.Ciudad).HasMaxLength(100);
            entity.Property(e => e.Contrasena).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(50);
            entity.Property(e => e.TipoDocumento).HasMaxLength(50);
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PK__Compra__0A5CDB5CB1C5EB3F");

            entity.ToTable("Compra");

            entity.Property(e => e.Iva).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MetodoPago).HasMaxLength(100);
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__Compra__IdProvee__5070F446");
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PK__DetalleC__E43646A5B3537DF6");

            entity.ToTable("DetalleCompra");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnidadMedida).HasMaxLength(50);

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK__DetalleCo__IdCom__5535A963");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__DetalleCo__IdPro__5629CD9C");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedido).HasName("PK__DetalleP__48AFFD95436DA65F");

            entity.ToTable("DetallePedido");

            entity.Property(e => e.Iva).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK__DetallePe__IdPed__5DCAEF64");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__DetallePe__IdPro__5EBF139D");
        });

        modelBuilder.Entity<DetalleProduccion>(entity =>
        {
            entity.HasKey(e => e.IdDetalleProduccion).HasName("PK__DetalleP__2BD8C21E561FFC13");

            entity.ToTable("DetalleProduccion");

            entity.Property(e => e.CantidadProducto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdProductoGeneralNavigation).WithMany(p => p.DetalleProduccions)
                .HasForeignKey(d => d.IdProductoGeneral)
                .HasConstraintName("FK__DetallePr__IdPro__7A672E12");

            entity.HasOne(d => d.IdProductoPersonalizadoNavigation).WithMany(p => p.DetalleProduccions)
                .HasForeignKey(d => d.IdProductoPersonalizado)
                .HasConstraintName("FK__DetallePr__IdPro__7B5B524B");
        });

        modelBuilder.Entity<DetalleProductoGeneral>(entity =>
        {
            entity.HasKey(e => e.IdDetalleGeneral).HasName("PK__DetalleP__9662848812CC6FB5");

            entity.ToTable("DetalleProductoGeneral");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnidadMedida).HasMaxLength(50);

            entity.HasOne(d => d.IdProductoGeneralNavigation).WithMany(p => p.DetalleProductoGenerals)
                .HasForeignKey(d => d.IdProductoGeneral)
                .HasConstraintName("FK__DetallePr__IdPro__70DDC3D8");
        });

        modelBuilder.Entity<DetalleVentum>(entity =>
        {
            entity.HasKey(e => e.IdDetalleVenta).HasName("PK__DetalleV__AAA5CEC2FD3A71E6");

            entity.Property(e => e.Iva).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__DetalleVe__IdPro__656C112C");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK__DetalleVe__IdVen__6477ECF3");
        });

        modelBuilder.Entity<EstadoProduccion>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__EstadoPr__FBB0EDC137FD814E");

            entity.ToTable("EstadoProduccion");

            entity.Property(e => e.NombreEstado).HasMaxLength(100);
        });

        modelBuilder.Entity<Insumo>(entity =>
        {
            entity.HasKey(e => e.IdInsumo).HasName("PK__Insumos__F378A2AF1D423976");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Imagen).HasMaxLength(200);
            entity.Property(e => e.Marca).HasMaxLength(100);
            entity.Property(e => e.NombreInsumo).HasMaxLength(100);
            entity.Property(e => e.UnidadMedida).HasMaxLength(50);

            entity.HasOne(d => d.IdCategoriaInsumosNavigation).WithMany(p => p.Insumos)
                .HasForeignKey(d => d.IdCategoriaInsumos)
                .HasConstraintName("FK__Insumos__IdCateg__5070F446");
        });
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedido__9D335DC3F541C90D");

            entity.ToTable("Pedido");

            entity.Property(e => e.MetodoPago).HasMaxLength(100);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__Pedido__IdClient__5AEE82B9");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__Permisos__0D626EC821ACD9B9");

            entity.Property(e => e.Accion).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Modulo).HasMaxLength(50);
        });

        modelBuilder.Entity<PersonaJuridica>(entity =>
        {
            entity.HasKey(e => e.IdProveedorJuridico).HasName("PK__PersonaJ__A732D0C49C8AEE6F");

            entity.ToTable("PersonaJuridica");

            entity.Property(e => e.NitEmpresa).HasMaxLength(100);
            entity.Property(e => e.NombreEmpresa).HasMaxLength(200);

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.PersonaJuridicas)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__PersonaJu__IdPro__4AB81AF0");
        });

        modelBuilder.Entity<PersonaNatural>(entity =>
        {
            entity.HasKey(e => e.IdProveedorNatural).HasName("PK__PersonaN__426448FA5D61BF36");

            entity.ToTable("PersonaNatural");

            entity.Property(e => e.Apellidos).HasMaxLength(100);
            entity.Property(e => e.Documento).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.PersonaNaturals)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__PersonaNa__IdPro__4D94879B");
        });

        modelBuilder.Entity<Produccion>(entity =>
        {
            entity.HasKey(e => e.IdProduccion).HasName("PK__Producci__3137BD831D98FE55");

            entity.ToTable("Produccion");

            entity.Property(e => e.NumeroPedido).HasMaxLength(100);
            entity.Property(e => e.ProductoAelaborar)
                .HasMaxLength(100)
                .HasColumnName("ProductoAElaborar");
        });

        modelBuilder.Entity<ProduccionEstado>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Produccion_Estado");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany()
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Produccio__IdEst__778AC167");

            entity.HasOne(d => d.IdProduccionNavigation).WithMany()
                .HasForeignKey(d => d.IdProduccion)
                .HasConstraintName("FK__Produccio__IdPro__76969D2E");
        });

        modelBuilder.Entity<ProductoCompra>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__09889210C33234BF");

            entity.ToTable("ProductoCompra");

            entity.Property(e => e.NombreProducto).HasMaxLength(100);
            entity.Property(e => e.UnidadMedida).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductoGeneral>(entity =>
        {
            entity.HasKey(e => e.IdProductoGeneral).HasName("PK__Producto__C28F19FB906B3788");

            entity.ToTable("ProductoGeneral");

            entity.Property(e => e.CantidadProducto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Imagen).HasMaxLength(200);
            entity.Property(e => e.NombreProducto).HasMaxLength(100);
            entity.Property(e => e.PrecioProducto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCategoriaProductoNavigation).WithMany(p => p.ProductoGenerals)
                .HasForeignKey(d => d.IdCategoriaProducto)
                .HasConstraintName("FK__ProductoG__IdCat__76969D2E");
        });


        modelBuilder.Entity<ProductoPersonalizado>(entity =>
        {
            entity.HasKey(e => e.IdProductoPersonalizado).HasName("PK__Producto__48F77C23A69F88D1");

            entity.ToTable("ProductoPersonalizado");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Decoracion).HasMaxLength(200);
            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.DisenioPersonalizado).HasMaxLength(300);
            entity.Property(e => e.ImagenReferencia).HasColumnType("image");
            entity.Property(e => e.Mensaje).HasMaxLength(300);
            entity.Property(e => e.TemasOmotivos)
                .HasMaxLength(200)
                .HasColumnName("TemasOMotivos");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ProductoPersonalizados)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__ProductoP__IdPro__797309D9");

            entity.HasOne(d => d.ToppingsNavigation).WithMany(p => p.ProductoPersonalizados)
                .HasForeignKey(d => d.Toppings)
                .HasConstraintName("FK__ProductoP__Toppi__7A672E12");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__Proveedo__E8B631AF4747EAA4");

            entity.ToTable("Proveedor");

            entity.Property(e => e.Celular).HasMaxLength(20);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.TipoProveedor).HasMaxLength(50);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__2A49584CFEC40992");

            entity.ToTable("Rol");

            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Rol1)
                .HasMaxLength(50)
                .HasColumnName("Rol");
        });

        modelBuilder.Entity<RolPermiso>(entity =>
        {
            entity.HasKey(e => e.IdRolPermiso).HasName("PK__RolPermi__0CC73B1B8F41CD56");

            entity.Property(e => e.Estado).HasDefaultValue(true);

            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.IdPermiso)
                .HasConstraintName("FK__RolPermis__IdPer__3D5E1FD2");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__RolPermis__IdRol__3C69FB99");
        });

        modelBuilder.Entity<Sede>(entity =>
        {
            entity.HasKey(e => e.IdSede).HasName("PK__Sede__A7780DFF677DA91F");

            entity.ToTable("Sede");

            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.Horarios).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF979AB9A878");

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Barrio).HasMaxLength(100);
            entity.Property(e => e.Celular).HasMaxLength(20);
            entity.Property(e => e.Ciudad).HasMaxLength(100);
            entity.Property(e => e.Contrasena).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(50);
            entity.Property(e => e.TipoDocumento).HasMaxLength(50);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuarios__IdRol__412EB0B6");
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PK__Venta__BC1240BDAB838551");

            entity.Property(e => e.MetodoPago).HasMaxLength(100);
            entity.Property(e => e.Sede).HasMaxLength(100);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__Venta__IdCliente__619B8048");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
