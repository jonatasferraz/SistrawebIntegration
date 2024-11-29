using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegracaoSitraWeb.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IntegracaoSitraWeb.Infrastructure.Persistence
{
    public partial class DiaslogNewContext : DbContext
    {
        public DiaslogNewContext()
        {
        }

        public DiaslogNewContext(DbContextOptions<DiaslogNewContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArquivoEntradum> ArquivoEntrada { get; set; }

        public virtual DbSet<ArquivoSitraweb> ArquivoSitrawebs { get; set; }

        public virtual DbSet<Ordem> Ordems { get; set; }

        public virtual DbSet<OrdemArquivoSitraweb> OrdemArquivoSitrawebs { get; set; }

        public virtual DbSet<Origem> Origems { get; set; }

        public virtual DbSet<Volume> Volumes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArquivoEntradum>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_ARQUIVO");

                entity.ToTable("ARQUIVO_ENTRADA");

                entity.HasIndex(e => new { e.Origem, e.NomeArquivo }, "IX_ARQUIVO");

                entity.HasIndex(e => new { e.Origem, e.Status }, "IX_ARQUIVO_ENTRADA");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.DataFimProcessamento)
                    .HasColumnType("datetime")
                    .HasColumnName("DATA_FIM_PROCESSAMENTO");
                entity.Property(e => e.DataFtp)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("DATA_FTP");
                entity.Property(e => e.DataInclusao)
                    .HasColumnType("datetime")
                    .HasColumnName("DATA_INCLUSAO");
                entity.Property(e => e.DataInicioProcessamento)
                    .HasColumnType("datetime")
                    .HasColumnName("DATA_INICIO_PROCESSAMENTO");
                entity.Property(e => e.NomeArquivo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NOME_ARQUIVO");
                entity.Property(e => e.Origem)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ORIGEM");
                entity.Property(e => e.QtdeOrdemIntegrados).HasColumnName("QTDE_ORDEM_INTEGRADOS");
                entity.Property(e => e.Status).HasColumnName("STATUS");
            });

            modelBuilder.Entity<ArquivoSitraweb>(entity =>
            {
                entity.ToTable("ARQUIVO_SITRAWEB");

                entity.HasIndex(e => e.Status, "IX_ARQUIVO_SITRAWEB");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.DataEnvio)
                    .HasColumnType("datetime")
                    .HasColumnName("DATA_ENVIO");
                entity.Property(e => e.DataInclusao)
                    .HasColumnType("datetime")
                    .HasColumnName("DATA_INCLUSAO");
                entity.Property(e => e.NomeArquivo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NOME_ARQUIVO");
                entity.Property(e => e.Status).HasColumnName("STATUS");
            });

            modelBuilder.Entity<Ordem>(entity =>
            {
                entity.ToTable("ORDEM");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CodigoTransportadora).HasColumnName("CODIGO_TRANSPORTADORA");
                entity.Property(e => e.CondicaoExpedicao).HasColumnName("CONDICAO_EXPEDICAO");
                entity.Property(e => e.DataEmbarque).HasColumnName("DATA_EMBARQUE");
                entity.Property(e => e.DataPrazo).HasColumnName("DATA_PRAZO");
                entity.Property(e => e.DestinatarioBairro)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESTINATARIO_BAIRRO");
                entity.Property(e => e.DestinatarioCep).HasColumnName("DESTINATARIO_CEP");
                entity.Property(e => e.DestinatarioCidade)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESTINATARIO_CIDADE");
                entity.Property(e => e.DestinatarioCodMun).HasColumnName("DESTINATARIO_COD_MUN");
                entity.Property(e => e.DestinatarioCodigo).HasColumnName("DESTINATARIO_CODIGO");
                entity.Property(e => e.DestinatarioCompl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESTINATARIO_COMPL");
                entity.Property(e => e.DestinatarioCpf).HasColumnName("DESTINATARIO_CPF");
                entity.Property(e => e.DestinatarioEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESTINATARIO_EMAIL");
                entity.Property(e => e.DestinatarioEnd)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESTINATARIO_END");
                entity.Property(e => e.DestinatarioIe)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESTINATARIO_IE");
                entity.Property(e => e.DestinatarioNome)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESTINATARIO_NOME");
                entity.Property(e => e.DestinatarioPontoReferencia)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESTINATARIO_PONTO_REFERENCIA");
                entity.Property(e => e.DestinatarioTelefone)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESTINATARIO_TELEFONE");
                entity.Property(e => e.DestinatarioUf)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DESTINATARIO_UF");
                entity.Property(e => e.FaturamentoAdvalorem).HasColumnName("FATURAMENTO_ADVALOREM");
                entity.Property(e => e.FaturamentoCnpj)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FATURAMENTO_CNPJ");
                entity.Property(e => e.FaturamentoFrete).HasColumnName("FATURAMENTO_FRETE");
                entity.Property(e => e.FaturamentoFreteCalculado).HasColumnName("FATURAMENTO_FRETE_CALCULADO");
                entity.Property(e => e.FaturamentoGris).HasColumnName("FATURAMENTO_GRIS");
                entity.Property(e => e.GrauRisco).HasColumnName("GRAU_RISCO");
                entity.Property(e => e.IdArquivoEntrada).HasColumnName("ID_ARQUIVO_ENTRADA");
                entity.Property(e => e.NfControle).HasColumnName("NF_CONTROLE");
                entity.Property(e => e.NfDataEmissao).HasColumnName("NF_DATA_EMISSAO");
                entity.Property(e => e.NfNumero).HasColumnName("NF_NUMERO");
                entity.Property(e => e.NfPeso).HasColumnName("NF_PESO");
                entity.Property(e => e.NfSerie).HasColumnName("NF_SERIE");
                entity.Property(e => e.NfValor).HasColumnName("NF_VALOR");
                entity.Property(e => e.NomePromotora)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NOME_PROMOTORA");
                entity.Property(e => e.NumeroRemessa).HasColumnName("NUMERO_REMESSA");
                entity.Property(e => e.NumeroRomaneio).HasColumnName("NUMERO_ROMANEIO");
                entity.Property(e => e.NumeroRomaneioRedespacho).HasColumnName("NUMERO_ROMANEIO_REDESPACHO");
                entity.Property(e => e.PedidoData).HasColumnName("PEDIDO_DATA");
                entity.Property(e => e.PedidoNumero).HasColumnName("PEDIDO_NUMERO");
                entity.Property(e => e.PedidoTipo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PEDIDO_TIPO");
                entity.Property(e => e.PeriodoEntrega).HasColumnName("PERIODO_ENTREGA");
                entity.Property(e => e.QtdeVolumes).HasColumnName("QTDE_VOLUMES");
                entity.Property(e => e.RecebedorBairro)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("RECEBEDOR_BAIRRO");
                entity.Property(e => e.RecebedorCep).HasColumnName("RECEBEDOR_CEP");
                entity.Property(e => e.RecebedorCidade)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("RECEBEDOR_CIDADE");
                entity.Property(e => e.RecebedorCodMun).HasColumnName("RECEBEDOR_COD_MUN");
                entity.Property(e => e.RecebedorCompl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("RECEBEDOR_COMPL");
                entity.Property(e => e.RecebedorCpf).HasColumnName("RECEBEDOR_CPF");
                entity.Property(e => e.RecebedorEndereco)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("RECEBEDOR_ENDERECO");
                entity.Property(e => e.RecebedorEstado)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("RECEBEDOR_ESTADO");
                entity.Property(e => e.RecebedorNome)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("RECEBEDOR_NOME");
                entity.Property(e => e.RemetenteCep).HasColumnName("REMETENTE_CEP");
                entity.Property(e => e.RemetenteCidade)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("REMETENTE_CIDADE");
                entity.Property(e => e.RemetenteCnpj).HasColumnName("REMETENTE_CNPJ");
                entity.Property(e => e.RemetenteEnd)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("REMETENTE_END");
                entity.Property(e => e.RemetenteIe)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REMETENTE_IE");
                entity.Property(e => e.RemetenteRazao)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("REMETENTE_RAZAO");
                entity.Property(e => e.RemetenteUf)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("REMETENTE_UF");
                entity.Property(e => e.SequenciaEntrega).HasColumnName("SEQUENCIA_ENTREGA");
                entity.Property(e => e.Setor)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SETOR");
                entity.Property(e => e.SubrotaEntrega)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SUBROTA_ENTREGA");
                entity.Property(e => e.TipoTransporte)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_TRANSPORTE");
                entity.Property(e => e.ValorFrete).HasColumnName("VALOR_FRETE");

                entity.HasOne(d => d.IdArquivoEntradaNavigation).WithMany(p => p.Ordems)
                    .HasForeignKey(d => d.IdArquivoEntrada)
                    .HasConstraintName("FK_ORDEM_ARQUIVO");
            });

            modelBuilder.Entity<OrdemArquivoSitraweb>(entity =>
            {
                entity.HasKey(e => new { e.IdOrdem, e.IdArquivoSitraweb });

                entity.ToTable("ORDEM_ARQUIVO_SITRAWEB");

                entity.Property(e => e.IdOrdem).HasColumnName("ID_ORDEM");
                entity.Property(e => e.IdArquivoSitraweb).HasColumnName("ID_ARQUIVO_SITRAWEB");
            });

            modelBuilder.Entity<Origem>(entity =>
            {
                entity.HasKey(e => e.Origem1);

                entity.ToTable("ORIGEM");

                entity.Property(e => e.Origem1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ORIGEM");
                entity.Property(e => e.DirConemb)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DIR_CONEMB");
                entity.Property(e => e.DirDoccob)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DIR_DOCCOB");
                entity.Property(e => e.DirNotfis)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DIR_NOTFIS");
                entity.Property(e => e.DirOcorren)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DIR_OCORREN");
                entity.Property(e => e.Host)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("HOST");
                entity.Property(e => e.Senha)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SENHA");
                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIPO");
                entity.Property(e => e.Usuario)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USUARIO");
            });

            modelBuilder.Entity<Volume>(entity =>
            {
                entity.ToTable("VOLUME");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CodigoBarras)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO_BARRAS");
                entity.Property(e => e.IdOrdem).HasColumnName("ID_ORDEM");
                entity.Property(e => e.NumeroVolume).HasColumnName("NUMERO_VOLUME");
                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIPO");

                entity.HasOne(d => d.IdOrdemNavigation).WithMany(p => p.Volumes)
                    .HasForeignKey(d => d.IdOrdem)
                    .HasConstraintName("FK_VOLUME_ORDEM");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
