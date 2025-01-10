<style scoped>
    .user-avtar{
        width: 40px;
        border-radius: 50%;
    }
    .color-text{
        color: azure;
    }
    .nav-item *{
        color: azure;
    }
    .info ul{
        width: 300px !important;     /* Đảm bảo không kế thừa chiều rộng từ phần tử cha */
    }
</style>
<script>

import Rightbar from "@/components/right-bar.vue"
import { Autoplay, A11y } from 'swiper/modules';

// import { ref } from 'vue';
import AOS from 'aos';
import 'aos/dist/aos.css';

// Import Swiper styles
import 'swiper/css';
import "swiper/css/autoplay";
import 'swiper/css/navigation';
import apiClient from "@/plugins/axios";


export default {
    data() {
        return{
            LoginStatus: false,
            modalLogin:false,
            login:{
                email: '',
                password:'',
            },
            register:{
                fullName:null,
                doB: null

            },
            showPassword: false,
            modalRegister: false,
            form: false,
            message: '',
            snackbar: false,
            snackbarColor: '',
            errorMessage: '',
            remember: false,
            rules: {
                required: v => !!v || 'Vui lòng không để trống',
                validPhone: v => /^0\d{9}$/.test(v) || 'Số điện thoại phải có 10 chữ số và bắt đầu bằng 0',
                validEmail: v => /^[a-zA-Z0-9._%+-]+@gmail\.com$/.test(v) || 'Email phải hợp lệ và kết thúc bằng @gmail.com',
            },
        }
    },
    name: "LANDING",
    components: {
        Rightbar
    },
    created(){
        const token = localStorage.getItem('tokencustomer');
        if (token) {
            // Token tồn tại, chuyển hướng đến trang tower
            this.LoginStatus = true;
        }
    },
    methods: {
        changeMode(mode) {
            this.currentMode = mode;
            if (mode == "dark") {
                document.body.setAttribute("data-pc-theme", "dark");
                document.body.setAttribute("data-topbar", "dark");
                document.body.classList.remove("mode-auto");
            } else if (mode == "auto") {
                document.body.setAttribute("data-pc-theme", "light");
                document.body.setAttribute("data-topbar", "light");
                document.body.classList.add("mode-auto");
            } else {
                document.body.setAttribute("data-pc-theme", "light");
                document.body.setAttribute("data-topbar", "light");
                document.body.classList.remove("mode-auto");
            }
        },
        toggleMenu() {
            const navbar = document.getElementById("navbarTogglerDemo01");
            navbar.classList.toggle("show");

            // Kiểm tra và thêm hoặc xóa sự kiện click bên ngoài
            if (navbar.classList.contains("show")) {
                document.addEventListener("click", this.handleOutsideClick);
            } else {
                document.removeEventListener("click", this.handleOutsideClick);
            }
        },
        handleOutsideClick(event) {
            const navbar = document.getElementById("navbarTogglerDemo01");
            const toggler = document.querySelector(".navbar-toggler");

            // Kiểm tra nếu click nằm ngoài menu và nút toggle
            if (!navbar.contains(event.target) && !toggler.contains(event.target)) {
                navbar.classList.remove("show");
                document.removeEventListener("click", this.handleOutsideClick);
            }
        },
        ViewLogin(){
            this.modalLogin = true;
        },
        toggleModal() {
            this.form = false;
            this.modalLogin = !this.modalLogin;
            this.modalRegister = !this.modalRegister;
        },
        Login(){
            apiClient.post(`/Customer/Login`,this.login)
                    .then(response=>{
                        this.$store.dispatch('loginCustomer',response.data);
                        if(this.remember){
                            localStorage.setItem('tokencustomer',response.data.token)
                            localStorage.setItem('customerId',response.data.userId)
                        }
                        this.modalLogin = false;
                        this.errorMessage = ""
                        this.LoginStatus = true;

                    })
                    .catch(error=>{
                        if (error.response && error.response.status === 401) {
                            this.errorMessage = error.response.data.message ;
                        } else {
                            this.errorMessage = 'Đã xảy ra lỗi hệ thống. '+ error.response.data.message;
                        }
                    })
        },
        SignOut(){
            this.$store.dispatch('logoutCustomer');
            this.LoginStatus = false;
            this.login.email = '';
            this.login.password = '';
        },
        
    },
    setup() {
        return {
            modules: [Autoplay, A11y],
        };
    },
    mounted() {
        AOS.init({
            easing: 'ease-in-out-sine',
            duration: 2000
        }); // Initialize AOS
            // document.body.setAttribute("data-pc-direction", "rtl");
            document.body.classList.add("landing-page");
    }
}
</script>

<template>
    <header id="home">
        <v-snackbar
            v-model="snackbar"
            :timeout="10000"
            class="custom-snackbar"
            :color="snackbarColor"
        >
            <h5 class="text-center">{{ message }}</h5>
        </v-snackbar>
        <BNav style="background-color: #023573; max-height: 75px;" class="navbar navbar-expand-md navbar-light default">
            <div class="container">
                <a class="pc-navbar-brand" href="/">
                    <h3 class="color-text">TÌM KIẾM NHÀ TRỌ</h3>
                </a>
                <button @click="toggleMenu" class="navbar-toggler rounded" type="button" data-bs-toggle="collapse" data-bs-target="#navbarTogglerDemo01" aria-controls="navbarTogglerDemo01" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarTogglerDemo01">
                    <ul class="navbar-nav ms-auto mt-lg-0 mt-2 mb-2 mb-lg-0 align-items-start ">
                        <li class="nav-item px-1">
                            <h5><router-link class="nav-link" to="/">Tìm Phòng trọ</router-link></h5>
                        </li>
                        <li class="nav-item px-1">
                            <h5><router-link class="nav-link" to="/findpeople">Tìm người ở ghép</router-link></h5>
                        </li>
                        <li class="nav-item px-1">
                            <h5><router-link to="/save" class="nav-link" v-show="LoginStatus">Phòng, tin đã lưu</router-link></h5>
                        </li>

                        <li class="nav-item" v-show="LoginStatus">
                            <router-link to="/tower" class="btn btn-primary">Đăng bài</router-link>
                        </li>
                        <li class="nav-item" v-show="!LoginStatus">
                            <button @click="ViewLogin()" class="btn btn-primary mr-4">Đăng nhập</button>
                        </li>
                    </ul>
                    <BDropdown v-show="LoginStatus" variant="link-secondary" auto-close="outside" class="info card-header-dropdown py-0" toggle-class="text-reset dropdown-btn arrow-none me-0" menu-class="dropdown-menu-end dropdown-user-profile dropdown-menu-end pc-h-dropdown" aria-haspopup="true" :offset="{ alignmentAxis: -145, crossAxis: 0, mainAxis: 20 }">
                        <template  #button-content><span class="text-muted"> <img src="/images/UserInformation/1/Avatar.jpg" alt="user-image" class="user-avtar">
                            </span>
                        </template>
                        <div class="dropdown-header d-flex align-items-center justify-content-between">
                            <h4 class="m-0">Thông tin cá nhân</h4>
                        </div>
                        <div class="dropdown-body">
                            <simplebar data-simplebar style="max-height: 500px;">
                                <div class="profile-notification-scroll position-relative" style="max-height: calc(100vh - 225px)">
                                    <ul class="list-group list-group-flush w-100">
                                        <li class="list-group-item">
                                            <div class="d-flex align-items-center">
                                                <div class="flex-shrink-0">
                                                    <img src="/images/UserInformation/1/Avatar.jpg" alt="user-image" class="wid-50 rounded-circle">
                                                </div>
                                                <div class="flex-grow-1 mx-3">
                                                    <h5 class="mb-0">Phạm Quang Hưng</h5>
                                                </div>
                                                <span class="badge bg-primary">PRO</span>
                                            </div>
                                        </li>
                                        <li class="list-group-item">
                                            <div class="dropdown-item" @click="viewdialogChangePassword = !viewdialogChangePassword">
                                                <span class="d-flex align-items-center">
                                                    <i class="ph-duotone ph-key"></i>
                                                    <span>Đổi mật khẩu</span>
                                                </span>
                                            </div>
                                            <div class="dropdown-item d-flex justify-content-between">
                                                <span class="d-flex align-items-center">
                                                    <i class="ph-duotone ph-moon"></i>
                                                    <span>Dark mode</span>
                                                </span>
                                                <div class="form-check form-switch form-check-reverse m-0">
                                                    <input class="form-check-input f-18" id="dark-mode" type="checkbox" @click="changeMode('dark')" role="switch">
                                                </div>
                                            </div>
                                            <div class="dropdown-item"  @click="(viewdialogInfo = !viewdialogInfo) &&(btnInfo())">
                                                <span class="d-flex align-items-center">
                                                    <i class="ph-duotone ph-user-circle"></i>
                                                    <span>Thông tin cá nhân</span>
                                                </span>
                                            </div>
                                            <a href="#" class="dropdown-item">
                                                <span class="d-flex align-items-center">
                                                    <i class="ph-duotone ph-gear-six"></i>
                                                    <span>Quản lý bài đăng</span>
                                                </span>
                                            </a>
                                            <div to="/login" class="dropdown-item" @click="SignOut()">
                                                <span class="d-flex align-items-center">
                                                    <i class="ph-duotone ph-power"></i>
                                                    <span>Đăng xuất</span>
                                                </span>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </simplebar>
                        </div>
                    </BDropdown> 
                    <BModal v-model="modalLogin" hide-footer class="v-modal-custom" id="login-modal" centered>
                        <template #title>
                            <h4 class="f-w-500 mb-1">Đăng nhập bằng Email</h4>
                            <p class="mb-3 pb-0">Chưa có tài khoản?<a href="#" class="link-primary ms-2" 
                                    data-bs-target="#registration-modal" @click="toggleModal">Tạo tài khoản</a></p>
                        </template>
                        <v-form v-model="form">
                            <div class="form-group mb-3">
                                <label class="form-label">Email </label>
                                <v-text-field 
                                        type="email" 
                                        variant="outlined" 
                                        placeholder="Nhập vào Email" 
                                        :rules="[rules.required]" 
                                        v-model="login.email"
                                        prepend-inner-icon="mdi-email-outline"
                                ></v-text-field>
                            </div>
                            <div class="form-group mb-3">
                                <label class="form-label">Mật khẩu</label>
                                <v-text-field 
                                    :type="showPassword ? 'text' : 'password'" 
                                    variant="outlined"
                                    placeholder="Nhập vào mật khẩu" 
                                    :rules="[rules.required]"  
                                    v-model="login.password"
                                    prepend-inner-icon="mdi-lock-outline"
                                    :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                                    @click:append-inner="showPassword = !showPassword"
                                ></v-text-field>
                            </div>
                        </v-form>
                        <div class="d-flex mt-1 justify-content-between align-items-center">
                            <div class="form-check">
                                <input class="form-check-input input-primary" v-model="remember" type="checkbox" id="customCheckc1" checked="">
                                <label class="form-check-label text-muted" for="customCheckc1">Nhớ mật khẩu?</label>
                            </div>
                        </div>
                        <div class="d-grid mt-4">
                            <button type="button" @click="Login()" class="btn btn-primary">Đăng nhập</button>
                            <p v-if="errorMessage" class="text-danger mt-2">{{ errorMessage }}</p>
                        </div>
                    </BModal>
                    <BModal v-model="modalRegister" hide-footer class="modal fade login-modal" id="registration-modal" data-bs-keyboard="false"
                        tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true" size="lg">
                        <template #title>
                            <h4 class="f-w-500 mb-1">Đăng kỳ bằng Email</h4>
                            <p class="mb-3">Đã có tài khoản? <a href="#" class="link-primary"
                                    data-bs-target="#login-modal" @click="toggleModal">Đăng nhập</a></p>
                        </template>
                        <v-form v-model="form">
                            <BRow>
                                <BCol class="col-lg-6">
                                    <div class="form-group m-0">
                                        <label class="form-label">Họ và tên</label>
                                        <v-text-field 
                                            type="text" 
                                            variant="outlined" 
                                            placeholder="Họ và tên" 
                                            :rules="[rules.required]" 
                                            v-model="register.fullName"
                                        ></v-text-field>                        
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Ngày sinh</label>
                                        <input type="date" class="form-control" id="example-datemin" :rules="[required]" v-model="register.doB" min="2000-01-02">
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group m-0">
                                        <label class="form-label">Số điện thoại</label>
                                        <v-text-field 
                                            type="text" 
                                            variant="outlined" 
                                            placeholder="Số điện thoại" 
                                            :rules="[rules.required, rules.validPhone]" 
                                            v-model="register.phoneNumber"
                                        ></v-text-field>                        
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group m-0">
                                        <label class="form-label">Email</label>
                                        <v-text-field 
                                            type="text" 
                                            variant="outlined" 
                                            placeholder="Email" 
                                            :rules="[rules.required, rules.validEmail]"
                                            v-model="register.email"
                                        ></v-text-field>                        
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group m-0">
                                        <label class="form-label">Căn cước công dân</label>
                                        <v-text-field 
                                            type="number" 
                                            variant="outlined" 
                                            placeholder="Căn cược công dân" 
                                            :rules="[rules.required]" 
                                            v-model="register.cCCD"
                                        ></v-text-field>                    
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group m-0">
                                        <label class="form-label">Số điện thoại đăng ký Zalo</label>
                                        <v-text-field 
                                            type="text" 
                                            variant="outlined" 
                                            placeholder="Số điện thoại đăng ký Zalo" 
                                            :rules="[rules.required, rules.validPhone]"
                                            v-model="register.sDTZalo"
                                        ></v-text-field>                        
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group m-0">
                                        <label class="form-label">Nhập lại mật khẩu</label>
                                        <input type="password" class="form-control" placeholder="Enter Confirm Password">
                                    </div>
                                </BCol>
                            </BRow>
                           
                            <div class="d-flex mt-1 justify-content-between align-items-center">
                                <div class="form-check">
                                    <input class="form-check-input input-primary" type="checkbox" id="customCheckc1" checked="">
                                    <label class="form-check-label text-muted" for="customCheckc1">Tôi đồng ý với tất cả các Điều khoản & Điều kiện</label>
                                </div>
                            </div>
                        </v-form>
                        <div class="d-grid mt-4">
                            <button type="button" class="btn btn-primary">Đăng ký</button>
                        </div>
                    </BModal>
                </div>
            </div>
        </BNav>
        <router-view>

        </router-view>
    </header>

    <!-- [ layout-card ] end -->

    <Rightbar />
</template>