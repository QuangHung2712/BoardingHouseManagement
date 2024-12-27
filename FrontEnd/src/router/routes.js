export default [
    {
        path: "/tower",
        name: "tower",
        meta: { title: "Tòa nhà" },
        component: () => import("../views/Manage/TowerView.vue"),
    },
    {
        path: '/:idtower',
        name: "towerDetails",
        meta: { title: "Tòa nhà" },
        component: () => import("../views/Manage/TowerDetails.vue"),
        redirect: { name: "homepage" },
        children:[
            {
                path: "homepage",
                name: "homepage",
                meta: { title: "Trang chủ" },
                component: () => import("../views/Manage/Page/HomePage.vue"),  
            },
            {
                path: "room",
                name: "room",
                meta: { title: "Phòng" },
                component: () => import("../views/Manage/Page/Room.vue"),
            },
            {
                path: "arise",
                name: "arise",
                meta: { title: "Phát sinh" },
                component: () => import("../views/Manage/Page/Arise.vue"),
            },
            {
                path: "bill",
                name: "bill",
                meta: { title: "Hóa đơn" },
                component: () => import("../views/Manage/Page/Bill.vue"),
            },
            {
                path: "contract",
                name: "contract",
                meta: { title: "Hợp đồng" },
                component: () => import("../views/Manage/Page/Contract/ContractView.vue"),
            },
            {
                path: "createEdit/:idcontract",
                name: "createEdit",
                meta: { title: "Chỉnh sửa hợp đồng" },
                component: () => import("../views/Manage/Page/Contract/CreateEditContract.vue"),
                props: (route) => ({
                    idtower: route.params.idtower,
                }),
            },
            {
                path: "report",
                name: "report",
                meta: { title: "Báo cáo" },
                component: () => import("../views/Manage/Page/Report.vue"),  
            },
            {
                path: "service",
                name: "service",
                meta: { title: "Dịch vụ" },
                component: () => import("../views/Manage/Page/Service.vue"),
            },
        ]
    },
    {
        path: "/login",
        name: "login",
        meta: { title: "Đăng nhập" },
        component: () => import("../views/Manage/Auth/Login.vue"),
    },
    {
        path: "/register",
        name: "register",
        meta: { title: "Đăng ký" },
        component: () => import("../views/Manage/Auth/register.vue"),
    },
    {
        path: "/forgot",
        name: "forgot",
        meta: { title: "Quên mật khẩu" },
        component: () => import("../views/Manage/Auth/forgot-password.vue"),
    },
    {
        path: "/verification",
        name: "verification",
        meta: { title: "Quên mật khẩu" },
        component: () => import("../views/Manage/Auth/code-verification.vue"),
    },
    {
        path: "/EnterBill/:idbill",
        name: "EnterBill",
        meta: { title: "Nhập thông tin hoá đơn" },
        component: () => import("../views/Manage/Page/Bill/EnterBillInformation.vue"),
    },
    {
        path: "/",
        name: "Home",
        meta: { title: "Trang chủ" },
        component: () => import("../views/FindRoom/index.vue"),
        children: [
            {
                path: "/",
                name: "FindARoom",
                meta: { title: "Tìm kiếm nhà trọ" },
                component: () => import("../views/FindRoom/FindRoom.vue"),
            },
            {
                path: "/detail",
                name: "detail",
                meta: { title: "Chi tiết phòng trọ" },
                component: () => import("../views/FindRoom/DetailRoom.vue"),
            }
        ]
    },
]