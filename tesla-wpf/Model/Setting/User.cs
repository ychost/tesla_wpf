using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Vera.Wpf.Lib.Model;

namespace tesla_wpf.Model.Setting {
    [Table("User")]
    public class User : BaseSqliteEntity {
        public string Token { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
    }
}
