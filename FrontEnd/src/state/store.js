import { createStore } from 'vuex';
import modules from './modules';



const store = createStore({
    modules,
    // Enable strict mode in development to get a warning
    // when mutating state outside of a mutation.
    // https://vuex.vuejs.org/guide/strict.html
    strict: process.env.NODE_ENV !== 'production',
    state: {
        isSidebarHidden: false,
        isMobileSidebarActive: false,
        isFixedWidth: false,
        token: null,
        userId: null,
    },
    mutations: {
        toggleSidebar(state) {
            state.isSidebarHidden = !state.isSidebarHidden;
        },
        toggleMobileSidebar(state) {
            state.isMobileSidebarActive = !state.isMobileSidebarActive;
        },
        setFullWidth(state) {
            state.isFixedWidth = false;
        },
        setFixedWidth(state) {
            state.isFixedWidth = true;
        },
        changeLayoutType(state, payload) {
            state.layoutType = payload.layoutType;
        },
        setToken(state, token) {
            state.token = token;
        },
        setUser(state, userId) {
            state.userId = userId;
        },
        clearAuth(state) {
            state.token = null;
            state.userId = null;
        },
    },
    actions: {
        login({ commit }, Userformation) {
            const { token, userId } = Userformation
            // Lưu token và userId vào Vuex
            commit('setToken', token);
            commit('setUser', userId);
            // Cập nhật token vào header mặc định của axios
            //axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
        },
        logout({ commit }) {
            // Xóa dữ liệu trong Vuex
            commit('clearAuth');

            // Xóa token trong localStorage
            localStorage.removeItem('tokenlandlord');
            localStorage.removeItem('landlordId');

            // Xóa header Authorization
            //delete axios.defaults.headers.common['Authorization'];

        },
        autoLogin({ commit }) {

            // Kiểm tra token trong localStorage khi tải ứng dụng
            const token = localStorage.getItem('tokenlandlord');
            const userId = localStorage.getItem('landlordId');
            if (token) {
                commit('setToken', token);
                // Gọi thêm API để lấy userId nếu cần
            }
            if(userId){
                commit('setUser', userId);
            }
        },
    },
    getters: {
        isFixedWidth: state => state.isFixedWidth,
        isAuthenticated(state) {
            return !!state.token; // Trả về true nếu token không null
        },
        getUserId(state) {
            return state.userId;
        },
    },
});

export default store;