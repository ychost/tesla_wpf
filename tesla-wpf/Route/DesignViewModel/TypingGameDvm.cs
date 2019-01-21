using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tesla_wpf.Route.ViewModel;

namespace tesla_wpf.Route.DesignViewModel {
    public class TypingGameDvm : TypingGameViewModel {
        public TypingGameDvm() {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) {
             
            }
        }
    }
}
