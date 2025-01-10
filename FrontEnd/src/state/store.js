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
        roomId: null,
        tokenCustomer: null,
        customerId: null
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
        clearCustomerAuth(state) {
            state.tokenCustomer = null;
            state.customerId = null;
        },
        setRoom(state,roomId){
            state.roomId = roomId;
        },
        setCustomerToken(state, token) {
            state.tokenCustomer = token;
        },
        setCustomer(state, userId) {
            state.customerId = userId;
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
        loginCustomer({ commit }, Userformation) {
            const { token, userId } = Userformation
            // Lưu token và userId vào Vuex
            commit('setCustomerToken', token);
            commit('setCustomer', userId);
            // Cập nhật token vào header mặc định của axios
            //axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
        },
        logoutCustomer({ commit }) {
            // Xóa dữ liệu trong Vuex
            commit('clearCustomerAuth');

            // Xóa token trong localStorage
            localStorage.removeItem('tokencustomer');
            localStorage.removeItem('customerId');

            // Xóa header Authorization
            //delete axios.defaults.headers.common['Authorization'];

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
        autoLoginCustomer({ commit }) {

            // Kiểm tra token trong localStorage khi tải ứng dụng
            const token = localStorage.getItem('tokencustomer');
            const userId = localStorage.getItem('customerId');
            if (token) {
                commit('setCustomerToken', token);
                // Gọi thêm API để lấy userId nếu cần
            }
            if(userId){
                commit('setCustomer', userId);
            }
        },
    },
    getters: {
        isFixedWidth: state => state.isFixedWidth,
        isAuthenticated(state) {
            return !!state.token; // Trả về true nếu token không null
        },
        isAuthenCustomer(state) {
            return !!state.tokenCustomer; // Trả về true nếu token không null
        },
        getUserId(state) {
            return state.userId;
        },
        getCustomerId(state) {
            return state.customerId;
        },
        GetRoomId(state){
            return state.roomId;
        }
    },
});

export default store;