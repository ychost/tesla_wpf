using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tesla_wpf.Extensions;
using tesla_wpf.Model;
using tesla_wpf.Model.Setting;
using tesla_wpf.Rest;
using tesla_wpf.Route;

namespace tesla_wpf.Toolkit {
    public static class ConvertToolkit {
        /// <summary>
        /// User 转换
        /// </summary>
        /// <param name="rsUser"></param>
        /// <returns></returns>
        public static User ConvertUser(RsUser rsUser) {
            return new User() {
                Avatar = rsUser.Avatar,
                Name = rsUser.Name,
                Token = rsUser.Token,
                Email = rsUser.Email,
                Phone = rsUser.Phone
            };
        }

        /// <summary>
        /// 拍照 Id 排序
        /// </summary>
        /// <param name="menus"></param>
        public static void SortMenuItems(ObservableCollection<MenuItem> menus) {
            menus.Sort((m1, m2) => m1.Id - m2.Id);
            foreach (var menu in menus) {
                if (menu.SubMenus?.Count > 0) {
                    SortMenuItems(menu.SubMenus);
                }
            }
        }

        /// <summary>
        /// 从服务器拉取的菜单数据转换成普通的菜单数据
        /// </summary>
        /// <param name="rsMenus"></param>
        /// <returns></returns>
        public static ObservableCollection<MenuItem> ConvertMenus(List<RsMenu> rsMenus) {
            var menuItems = new ObservableCollection<MenuItem>();
            foreach (var rm in rsMenus) {
                // 子菜单
                if (rm.Children == null || rm.Children.Count == 0) {
                    if (RouteConfig.MenuConfig.TryGetValue(rm.Link, out var route)) {
                        var view = (IMenu)Activator.CreateInstance(route.ViewType);
                        var item = new MenuItem(rm.Text, view, rm.Icon) {
                            Id = route.Order
                        };
                        menuItems.Add(item);
                    } else {
                        throw new Exception($"菜单 [{rm.Text}:{rm.Link}] 不存在");
                    }
                    // 父菜单
                } else {
                    menuItems.Add(new MenuItem(rm.Text, rm.Icon) {
                        SubMenus = ConvertMenus(rm.Children)
                    });
                }
            }
            SortMenuItems(menuItems);
            return menuItems;
        }

    }
}
