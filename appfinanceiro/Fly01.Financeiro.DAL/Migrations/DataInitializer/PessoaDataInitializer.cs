using System;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Financeiro.DAL.Migrations.DataInitializer.Contract;

namespace Fly01.Financeiro.DAL.Migrations.DataInitializer
{
    public class PessoaDataInitializer : IDataInitializer
    {

        #region Nome

        private readonly string _plataformaId;
        private readonly string _usuarioSeed;
        private readonly int _maxRecords;
        private readonly string[,] _nomesFAleatorios;
        private Guid _cidadeDefault;
        private Guid _estadoDefault;

        private static readonly object SyncObj = new object();
        private static Random _random;


        public PessoaDataInitializer(string plataformaId, string usuarioSeed, int maxRecords)
        {
            _plataformaId = plataformaId;
            _usuarioSeed = usuarioSeed;
            _maxRecords = maxRecords;

            _nomesFAleatorios = new[,] {
                {                                                                                                                                  
                    "Caroline",    "Maira",     "Ramon",       "Eduardo",      "Juliana",     "Eduardo",   "Arthur",     "Tiago",      "Yasser",    "Thais",     "Willian",
                    "Roger",       "Rafael",    "Alexandro",   "Gabriel",      "Gustavo",     "Thiago",    "Israel",     "Jackson",    "Matheus",   "Jefferson", "Cassio",
                    "Giovani",     "Mario",     "Rodrigo",     "Aline",        "Ana",         "Bruno",     "Fabiano",    "Fabio",      "Isis",      "Jonatha",   "Lorenzo",
                    "Rodrigo",     "Vinicius",  "Wilson",      "Roberto",      "Nylce",       "Marcos",    "Alexis",     "Michelle",   "Luis",      "Diego",     "Mariel",
                    "Luan",        "Katiucia",  "Lércio",      "Bruna",        "Cristina",    "Aline",     "Mônica",     "Michelle",   "Luis",      "Diego",     "Mariel",
                    "Caroline",    "Maira",     "Ramon",       "Eduardo",      "Juliana",     "Eduardo",   "Arthur",     "Tiago",      "Yasser",    "Thais",     "Willian",
                    "Roger",       "Rafael",    "Alexandro",   "Gabriel",      "Gustavo",     "Thiago",    "Israel",     "Jackson",    "Matheus",   "Jefferson", "Cassio",
                    "Giovani",     "Mario",     "Rodrigo",     "Aline",        "Ana",         "Bruno",     "Fabiano",    "Fabio",      "Isis",      "Jonatha",   "Lorenzo",
                    "Rodrigo",     "Golias",    "Trinity",     "Roberto",      "Morpheu",     "Walter",    "Jô",         "Didi",       "Luis",      "Fidel",     "Gandalf"
                },                                                                                                                                  
                {                                                                                                                                   
                    "",            "Batista",   "Chaves",      "Martins",      "Salgueiro",   "Elen",      "Campão",     "Jardim",     "Anuar",     "Lima",      "Othman",
                    "de Souza",    "Felipe",    "Roberto",     "Honatel",      "Tadashi",     "Wakita",    "Adonis",     "Agostini",   "Geremia",   "Cezar",     "Reichelt",
                    "Rosa",        "Figueredo", "Luis",        "Santos",       "Pereira",     "Luiz",      "Antonio",    "Zolet",      "Cristina",  "Costa",     "de Souza",
                    "Lopes",       "Santos",    "de Oliveira", "Fagundes",     "Madlene",     "Cristiane", "de Jesus",   "Fraga",      "Cardoso",   "Silva",     "Bittencourt",
                    "Jorge",       "Pariz",     "Castro",      "Oliveira",     "Santana",     "Machado",   "de Jesus",   "Fraga",      "Gretchen",  "Silva",     "No Céu Tem Pão",
                    "",            "Batista",   "Chaves",      "Martins",      "Salgueiro",   "Elen",      "Campão",     "Jardim",     "Anuar",     "Lima",      "Othman",
                    "de Souza",    "Felipe",    "Roberto",     "Honatel",      "Tadashi",     "Wakita",    "Adonis",     "Agostini",   "Geremia",   "Cezar",     "Reichelt",
                    "Rosa",        "Figueredo", "Luis",        "Santos",       "Pereira",     "Luiz",      "Antonio",    "Zolet",      "Cristina",  "Costa",     "de Souza",
                    "Lopes",       "Santos",    "de Oliveira", "Fagundes",     "Madlene",     "Cristiane", "de Jesus",   "Fraga",      "Cardoso",   "Silva",     "Bittencourt",
                },
                {
                    "",            "Batista",   "Jandrey",     "Chaves",       "da Silva",    "Salgueiro", "Koch",       "Elen",       "Ouriques",  "Muraro",    "Campão",
                    "Jardim",      "Schäffer",  "Anuar",       "Lima",         "Othman",      "Rachid",    "Felipe",     "Kuhn",       "Roberto",   "Honatel",   "Silva", 
                    "Paviani",     "Agostini",  "Vieira",      "Geremia",      "da Rosa",     "Cezar",     "Custodio",   "Reichelt",   "Emmel",     "Oliveira",  "Santos",
                    "de Borba",    "Quines",    "Rosa",        "de Fraga",     "Figueredo",   "Pereira",   "Luis",       "Follmann",   "Santos",    "Machado",   "Luis",
                    "Machado",     "Tomilin",   "Luis",        "Rohr",         "Luiz",        "da Rocha",  "Antonio",    "dos Santos", "Zolete",    "Busata",    "Tiecher",
                    "Cristina",    "Costa",     "de Almeida",  "de Souza",     "Silva",       "Santana",   "Fernandes",  "Lopes",      "Faleiro",   "Santos",    "Franco",
                    "de Oliveira", "Dias",      "Fagundes",    "Garcia",       "Da Fré",      "Madlene",   "dos Santos", "Silveira",   "Cristiane", "de Jesus",  "Duval",
                    "Fraga",       "Cardoso",   "Silva",       "Presser",      "Bittencourt", "Ramos",     "Jorge",      "Tedokon",    "Pariz",     "Almeida",   "Machado",
                    "Tadashi",     "Castro",    "Soares",      "Adonis",       "White",       "Putin",     "Clinton",    "Branco",     "Pariz",     "Neo",       "Polos Hermanos"
            }};
        }


        #endregion

        public void Initialize(AppDataContext context)
        {
            var estado = context.Estados.FirstOrDefault(e => e.CodigoIbge.Equals("43"));
            if (estado != null)
                _estadoDefault = estado.Id;

            var cidade = context.Cidades.FirstOrDefault(c => c.Nome.Equals("Porto Alegre"));
            if (cidade != null)
                _cidadeDefault = cidade.Id;

            if (!context.Pessoas.Any())
            {
                var population = Enumerable.Range(1, _maxRecords).Select((index, x) => new Pessoa()
                {
                    Id = Guid.NewGuid(),
                    PlataformaId = _plataformaId,
                    DataInclusao = DateTime.Now,
                    UsuarioInclusao = _usuarioSeed,
                    Ativo = true,

                    TipoDocumento = "F",
                    CPFCNPJ = NewCpf(),
                    Nome = NomeAleatorio(_nomesFAleatorios),
                    NomeComercial = NomeAleatorio(_nomesFAleatorios).Substring(0, 3).Trim() + " " + NomeAleatorio(_nomesFAleatorios).Substring(0, 3).Trim(),
                    Cliente = true,
                    Fornecedor = true,
                    Transportadora = true,
                    Vendedor = true,
                    Email = $"cliente_mock_{index}@totvs.com.br",
                    Contato = NomeAleatorio(_nomesFAleatorios),
                    Celular = $"519{index}".PadRight(11, char.Parse(index.ToString().Substring(0, 1))),
                    Telefone = $"513{index}".PadRight(10, char.Parse(index.ToString().Substring(0, 1))),
                    EstadoId = _estadoDefault,
                    CidadeId = _cidadeDefault,
                    Bairro = "Partenon",
                    CEP = "90619900",
                    Endereco = "Av. Ipiranga, 6681",
                    Observacao = $"Dados mock de teste {index}"
                });

                context.Pessoas.AddRange(population);
                context.SaveChanges();
            }
        }

        private static string NomeAleatorio(string[,] nomes)
        {
            var primeiroNome = nomes[0, int.Parse(GenerateRandomNumber(0, nomes.GetUpperBound(1)))];
            var segundoNome = nomes[1, int.Parse(GenerateRandomNumber(0, nomes.GetUpperBound(1)))];
            var terceiroNome = nomes[2, int.Parse(GenerateRandomNumber(0, nomes.GetUpperBound(1)))];
            var nomeCompleto = (primeiroNome + " " + segundoNome + " " + terceiroNome).Replace("  ", " ");

            return nomeCompleto;
        }

        private static string GenerateRandomNumber(int min = 100000000, int max = 999999999)
        {
            lock (SyncObj)
            {
                if (_random == null)
                    _random = new Random(); // Or exception...
                return _random.Next(min, max).ToString();
            }
        }

        public static string NewCpf()
        {
            var sum = 0;
            var multiplier1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplier2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var seed = GenerateRandomNumber();

            for (var i = 0; i < 9; i++)
                sum += int.Parse(seed[i].ToString()) * multiplier1[i];

            var rest = sum % 11;

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            seed += rest;
            sum = 0;

            for (var i = 0; i < 10; i++)
                sum += int.Parse(seed[i].ToString()) * multiplier2[i];

            rest = sum % 11;

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            seed += rest;

            return seed;
        }
    }
}
