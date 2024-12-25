import { createWebHistory, createRouter } from "vue-router";
import routes from './routes';
import appConfig from "../../app.config";


const router = createRouter({
    history: createWebHistory("/vue/"),
    routes,
  
});

router.beforeResolve(async (routeTo, routeFrom, next) => {
    // Tạo một hook `BeforeResolve`, nó sẽ kích hoạt bất cứ khi nào
    // `BeforeRouteEnter` và `BeforeRouteUpdate` sẽ. Cái này
    // Cho phép chúng tôi đảm bảo dữ liệu được tìm nạp ngay cả khi thông số thay đổi,
    // Nhưng tuyến đường đã giải quyết thì không. Chúng tôi đặt nó trong `meta` để
    // Chỉ ra rằng đó là một hook mà chúng ta đã tạo ra, chứ không phải là một phần của
    // Bộ định tuyến Vue (chưa?).
    try {
        // Đối với mỗi tuyến đường phù hợp...
        for (const route of routeTo.matched) {
            await new Promise((resolve, reject) => {
                // If a `beforeResolve` hook is defined, call it with
                // the same arguments as the `beforeEnter` hook.
                if (route.meta && route.meta.beforeResolve) {
                    route.meta.beforeResolve(routeTo, routeFrom, (...args) => {
                        // If the user chose to redirect...
                        if (args.length) {
                            // If redirecting to the same route we're coming from...
                            // Complete the redirect.
                            next(...args);
                            reject(new Error('Redirected'));
                        } else {
                            resolve();
                        }
                    });
                } else {
                    // Otherwise, continue resolving the route.
                    resolve();
                }
            });
        }
        // If a `beforeResolve` hook chose to redirect, just return.
    } catch (error) {
        return;
    }
    document.title = routeTo.meta.title + ' | ' + appConfig.title;
    // If we reach this point, continue resolving the route.
    next();
});

export default router;