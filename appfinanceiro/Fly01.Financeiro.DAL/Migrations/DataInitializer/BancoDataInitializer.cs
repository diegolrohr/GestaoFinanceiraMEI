﻿using Fly01.Financeiro.DAL.Migrations.DataInitializer.Contract;
using System;
using System.Linq;
using System.Data.Entity.Migrations;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.DAL.Migrations.DataInitializer
{
    public class BancoDataInitializer : IDataInitializer
    {
        public void Initialize(AppDataContext context)
        {
            if (!context.Bancos.Any())
            {
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "654", Nome = "BANCO A.J.RENNER S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "246", Nome = "BANCO ABC BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "025", Nome = "BANCO ALFA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "641", Nome = "BANCO ALVORADA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "213", Nome = "BANCO ARBI S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "019", Nome = "BANCO AZTECA DO BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "029", Nome = "BANCO BANERJ S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "000", Nome = "BANCO BANKPAR S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "740", Nome = "BANCO BARCLAYS S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "107", Nome = "BANCO BBM S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "031", Nome = "BANCO BEG S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "739", Nome = "BANCO BGN S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "096", Nome = "BANCO BM&F DE SERVICOS DE LIQUIDACAO E C" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "318", Nome = "BANCO BMG S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "752", Nome = "BANCO BNP PARIBAS BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "248", Nome = "BANCO BOAVISTA INTERATLANTICO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "218", Nome = "BANCO BONSUCESSO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "065", Nome = "BANCO BRACCE S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "036", Nome = "BANCO BRADESCO BBI S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "204", Nome = "BANCO BRADESCO CARTOES S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "394", Nome = "BANCO BRADESCO FINANCIAMENTOS S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "237", Nome = "BANCO BRADESCO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "225", Nome = "BANCO BRASCAN S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M15", Nome = "BANCO BRJ S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "208", Nome = "BANCO BTG PACTUAL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "044", Nome = "BANCO BVA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "263", Nome = "BANCO CACIQUE S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "473", Nome = "BANCO CAIXA GERAL - BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "412", Nome = "BANCO CAPITAL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "040", Nome = "BANCO CARGILL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "745", Nome = "BANCO CITIBANK S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M08", Nome = "BANCO CITICARD S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "241", Nome = "BANCO CLASSICO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M19", Nome = "BANCO CNH CAPITAL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "215", Nome = "BANCO COMERCIAL E DE INVESTIMENTO SUDAME" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "756", Nome = "BANCO COOPERATIVO DO BRASIL S.A. - BANCO" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "748", Nome = "BANCO COOPERATIVO SICREDI S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "075", Nome = "BANCO CR2 S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "721", Nome = "BANCO CREDIBEL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "222", Nome = "BANCO CREDIT AGRICOLE BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "505", Nome = "BANCO CREDIT SUISSE (BRASIL) S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "229", Nome = "BANCO CRUZEIRO DO SUL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "266", Nome = "BANCO CEDULA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "003", Nome = "BANCO DA AMAZONIA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "083", Nome = "BANCO DA CHINA BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M21", Nome = "BANCO DAIMLERCHRYSLER S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "707", Nome = "BANCO DAYCOVAL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "300", Nome = "BANCO DE LA NACION ARGENTINA" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "495", Nome = "BANCO DE LA PROVINCIA DE BUENOS AIRES" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "494", Nome = "BANCO DE LA REPUBLICA ORIENTAL DEL URUGU" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M06", Nome = "BANCO DE LAGE LANDEN BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "024", Nome = "BANCO DE PERNAMBUCO S.A. - BANDEPE" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "456", Nome = "BANCO DE TOKYO-MITSUBISHI UFJ BRASIL S.A" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "214", Nome = "BANCO DIBENS S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "001", Nome = "BANCO DO BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "047", Nome = "BANCO DO ESTADO DE SERGIPE S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "037", Nome = "BANCO DO ESTADO DO PARA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "039", Nome = "BANCO DO ESTADO DO PIAUÍ S.A. - BEP" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "041", Nome = "BANCO DO ESTADO DO RIO GRANDE DO SUL S.A" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "004", Nome = "BANCO DO NORDESTE DO BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "265", Nome = "BANCO FATOR S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M03", Nome = "BANCO FIAT S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "224", Nome = "BANCO FIBRA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "626", Nome = "BANCO FICSA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M18", Nome = "BANCO FORD S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "233", Nome = "BANCO GE CAPITAL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "734", Nome = "BANCO GERDAU S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M07", Nome = "BANCO GMAC S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "612", Nome = "BANCO GUANABARA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M22", Nome = "BANCO HONDA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "063", Nome = "BANCO IBI S.A. BANCO MULTIPLO" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M11", Nome = "BANCO IBM S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "604", Nome = "BANCO INDUSTRIAL DO BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "320", Nome = "BANCO INDUSTRIAL E COMERCIAL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "653", Nome = "BANCO INDUSVAL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "630", Nome = "BANCO INTERCAP S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "077", Nome = "BANCO INTERMEDIUM S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "249", Nome = "BANCO INVESTCRED UNIBANCO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M09", Nome = "BANCO ITAUCRED FINANCIAMENTOS S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "184", Nome = "BANCO ITAU BBA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "479", Nome = "BANCO ITAUBANK S.A" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "376", Nome = "BANCO J. P. MORGAN S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "074", Nome = "BANCO J. SAFRA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "217", Nome = "BANCO JOHN DEERE S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "076", Nome = "BANCO KDB S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "757", Nome = "BANCO KEB DO BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "600", Nome = "BANCO LUSO BRASILEIRO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "212", Nome = "BANCO MATONE S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M12", Nome = "BANCO MAXINVEST S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "389", Nome = "BANCO MERCANTIL DO BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "746", Nome = "BANCO MODAL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M10", Nome = "BANCO MONEO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "738", Nome = "BANCO MORADA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "066", Nome = "BANCO MORGAN STANLEY S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "243", Nome = "BANCO MAXIMA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "045", Nome = "BANCO OPPORTUNITY S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M17", Nome = "BANCO OURINVEST S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "623", Nome = "BANCO PANAMERICANO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "611", Nome = "BANCO PAULISTA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "613", Nome = "BANCO PECUNIA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "094", Nome = "BANCO PETRA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "643", Nome = "BANCO PINE S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "724", Nome = "BANCO PORTO SEGURO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "735", Nome = "BANCO POTTENCIAL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "638", Nome = "BANCO PROSPER S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M24", Nome = "BANCO PSA FINANCE BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "747", Nome = "BANCO RABOBANK INTERNATIONAL BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "088", Nome = "BANCO RANDON S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "356", Nome = "BANCO REAL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "633", Nome = "BANCO RENDIMENTO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "741", Nome = "BANCO RIBEIRAO PRETO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M16", Nome = "BANCO RODOBENS S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "072", Nome = "BANCO RURAL MAIS S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "453", Nome = "BANCO RURAL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "422", Nome = "BANCO SAFRA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "033", Nome = "BANCO SANTANDER (BRASIL) S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "250", Nome = "BANCO SCHAHIN S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "743", Nome = "BANCO SEMEAR S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "749", Nome = "BANCO SIMPLES S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "366", Nome = "BANCO SOCIETE GENERALE BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "637", Nome = "BANCO SOFISA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "012", Nome = "BANCO STANDARD DE INVESTIMENTOS S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "464", Nome = "BANCO SUMITOMO MITSUI BRASILEIRO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "082", Nome = "BANCO TOPASIO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M20", Nome = "BANCO TOYOTA DO BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M13", Nome = "BANCO TRICURY S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "634", Nome = "BANCO TRIAULO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M14", Nome = "BANCO VOLKSWAGEN S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "M23", Nome = "BANCO VOLVO (BRASIL) S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "655", Nome = "BANCO VOTORANTIM S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "610", Nome = "BANCO VR S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "370", Nome = "BANCO WESTLB DO BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "021", Nome = "BANESTES S.A. BANCO DO ESTADO DO ESPÍRIT" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "719", Nome = "BANIF-BANCO INTERNACIONAL DO FUNCHAL (BR" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "755", Nome = "BANK OF AMERICA MERRILL LYNCH BANCO MULT" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "744", Nome = "BANKBOSTON N.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "073", Nome = "BB BANCO POPULAR DO BRASIL S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "078", Nome = "BES INVESTIMENTO DO BRASIL S.A.-BANCO DE" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "069", Nome = "BPN BRASIL BANCO MULTIPLO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "070", Nome = "BRB - BANCO DE BRASÍLIA S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "092", Nome = "BRICKELL S.A. CREDITO, FINANCIAMENTO E I" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "104", Nome = "CAIXA ECONOMICA FEDERAL" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "477", Nome = "CITIBANK N.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "081", Nome = "CONCORDIA BANCO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "097", Nome = "COOPERATIVA CENTRAL DE CREDITO NOROESTE " });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "085", Nome = "COOPERATIVA CENTRAL DE CREDITO URBANO-CE" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "099", Nome = "COOPERATIVA CENTRAL DE ECONOMIA E CREDIT" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "090", Nome = "COOPERATIVA CENTRAL DE ECONOMIA E CREDIT" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "089", Nome = "COOPERATIVA DE CREDITO RURAL DA REGIAO D" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "087", Nome = "COOPERATIVA UNICRED CENTRAL SANTA CATARI" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "098", Nome = "CREDICOROL COOPERATIVA DE CREDITO RURAL" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "487", Nome = "DEUTSCHE BANK S.A. - BANCO ALEMAO" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "751", Nome = "DRESDNER BANK BRASIL S.A. - BANCO MULTIP" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "064", Nome = "GOLDMAN SACHS DO BRASIL BANCO MULTIPLO S" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "062", Nome = "HIPERCARD BANCO MULTIPLO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "399", Nome = "HSBC BANK BRASIL S.A. - BANCO MULTIPLO" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "168", Nome = "HSBC FINANCE (BRASIL) S.A. - BANCO MULTI" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "492", Nome = "ING BANK N.V." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "652", Nome = "ITAU UNIBANCO HOLDING S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "341", Nome = "ITAU UNIBANCO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "079", Nome = "JBS BANCO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "488", Nome = "JPMORGAN CHASE BANK" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "014", Nome = "NATIXIS BRASIL S.A. BANCO MULTIPLO" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "753", Nome = "NBC BANK BRASIL S.A. - BANCO MULTIPLO" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "086", Nome = "OBOE CREDITO FINANCIAMENTO E INVESTIMENT" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "254", Nome = "PARANA BANCO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "409", Nome = "UNIBANCO - UNIAO DE BANCOS BRASILEIROS S" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "230", Nome = "UNICARD BANCO MULTIPLO S.A." });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "091", Nome = "UNICRED CENTRAL DO RIO GRANDE DO SUL" });
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "084", Nome = "UNICRED NORTE DO PARANA" });

                context.SaveChanges();
            }

            if (!context.Bancos.Any(x => x.Codigo == "999"))
            {
                context.Bancos.AddOrUpdate(new Banco() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "999", Nome = "Outros" });
                context.SaveChanges();
            }

            if (!context.Bancos.Any(x => x.Codigo == "001" && x.EmiteBoleto == true))
            {
                context.Bancos.AddOrUpdate(x => x.Codigo,
                  new Banco() { DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "001", EmiteBoleto = true, Nome = "BANCO DO BRASIL S.A." }
                , new Banco() { DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "033", EmiteBoleto = true, Nome = "BANCO SANTANDER (BRASIL) S.A." }
                , new Banco() { DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "237", EmiteBoleto = true, Nome = "BANCO BRADESCO S.A." }
                , new Banco() { DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "104", EmiteBoleto = true, Nome = "CAIXA ECONOMICA FEDERAL" }
                , new Banco() { DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "341", EmiteBoleto = true, Nome = "ITAU UNIBANCO S.A." }
                , new Banco() { DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Codigo = "041", EmiteBoleto = true, Nome = "BANCO DO ESTADO DO RIO GRANDE DO SUL S.A" });

                context.SaveChanges();
            }
        }
    }
}