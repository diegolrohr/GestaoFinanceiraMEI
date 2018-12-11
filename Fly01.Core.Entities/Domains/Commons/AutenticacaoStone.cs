using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class AutenticacaoStone
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password{ get; set; }
    }
}
