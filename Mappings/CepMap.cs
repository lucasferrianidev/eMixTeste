using EMixApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMixApi.Mappings
{
    public class CepMap : IEntityTypeConfiguration<CEP>
    {
        public void Configure(EntityTypeBuilder<CEP> builder)
        {
            builder.HasKey(e => e.Id)
                .HasName("IDX_Id");

            builder.ToTable("CEP");

            builder.Property(e => e.Cep)
                .IsRequired()
                .HasColumnName("CEP")
                .HasColumnType("varchar(8)");

            builder.Property(e => e.Logradouro)
                .IsRequired()
                .HasColumnName("LOGRADOURO")
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Complemento)
                .IsRequired()
                .HasColumnName("COMPLEMENTO")
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Bairro)
                .IsRequired()
                .HasColumnName("BAIRRO")
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Localidade)
                .IsRequired()
                .HasColumnName("LOCALIDADE")
                .HasColumnType("varchar(500)");

            builder.Property(e => e.Uf)
                .IsRequired()
                .HasColumnName("UF")
                .HasColumnType("varchar(2)");

            builder.Property(e => e.Unidade)
                .IsRequired()
                .HasColumnName("UNIDADE")
                .HasColumnType("int");

            builder.Property(e => e.Ibge)
                .IsRequired()
                .HasColumnName("IBGE")
                .HasColumnType("int");

            builder.Property(e => e.Gia)
                .IsRequired()
                .HasColumnName("GIA")
                .HasColumnType("varchar(500)");
        }
    }
}
