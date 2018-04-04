using Newtonsoft.Json;

namespace Fly01.Core.JQueryDataTable
{
    public class JQueryDataTableColumn
    {
        public JQueryDataTableColumn()
        {
            this.Searchable = true;
            this.Visible = true;
        }

        /// <summary>
        /// Nome que irá ser apresentado para o usuário
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Orderable = true (padrão do datatable, todos os campos são ordenáveis, mesmo que não informado) || Orderable = false (deve ser informado de forma explícita que um campo não deve ser ordenado)        
        /// </summary>
        [JsonProperty("orderable")]
        public bool? Orderable { get; set; }

        /// <summary>
        /// Campo pode ser informado em (int)px ou em (int)% respeitando limite de 100% da table
        /// </summary>
        [JsonProperty("width")]
        public string Width { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// Campo que servirá para busca
        /// </summary>
        [JsonProperty("searchable")]
        public bool Searchable { get; set; }

        [JsonProperty("visible")]
        public bool Visible { get; set; }


    }
}