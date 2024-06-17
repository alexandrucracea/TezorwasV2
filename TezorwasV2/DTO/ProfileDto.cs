using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TezorwasV2.Model;

namespace TezorwasV2.DTO
{
    public class ProfileDto
    {
        public string Id { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public List<AchievmentModel>? Achievments { get; set; }
        public string PersonId { get; set; } = string.Empty;
        public int Level { get; set; }
        public List<HabbitModel>? Habbits { get; set; }
        //todo de adaugat un task model si de pus aici proprietate
        //todo de adaugat un friend list model si de pus aici proprietate
        public int Xp { get; set; }
        public List<TaskModel>? Tasks { get; set; }
        public List<ReceiptModel>? Receipts { get; set;}
    }
}
