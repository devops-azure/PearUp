
export class CommonUtility {

    static toQueryString(options) {
        let params = new URLSearchParams();
        for (let key in options) {
            params.set(key, (options[key]))
        }

        return params.toString();
    }

    static parseMenus(menus: any) {
        return this.parseMenusArray(menus.filter(menu => menu.MenuId != -1));
    }

    private static parseMenusArray(menus: any[]) {
        return menus.map(menu => this.parseMenuItem(menu));
    }

    private static parseMenuItem(menu: any) {
        return {
            path: menu.IsExternalUrl ? '' : menu.RouteUrl.split('/'),
            data: {
                menu: {
                    title: menu.MenuName,
                    icon: menu.MenuIcon,
                    selected: menu.IsActive,
                    expanded: false,
                    order: menu.MenuOrder,
                    url: menu.IsExternalUrl ? menu.RouteUrl : undefined,
                    target: menu.IsExternalUrl ? '_blank' : undefined,
                }
            },
            children: (menu.ChildList.length) ? this.parseMenusArray(menu.ChildList) : []
        };
    }
}
