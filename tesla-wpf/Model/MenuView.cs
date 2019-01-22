using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace tesla_wpf.Model {
    public abstract class MenuView : UserControl, IMenuView {
        public MenuView() {

        }


        public abstract void LazyInitialize();
    }
}
