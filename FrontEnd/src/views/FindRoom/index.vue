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


export default {
    data() {
        return{
            status: false,
            LoginStatus: false,
        }
    },
    name: "LANDING",
    components: {
        Rightbar
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
        <BNav style="background-color: #023573;" class="navbar navbar-expand-md navbar-light default">
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
                            <router-link to="/tower" class="btn btn-primary mr-4">Đăng nhập</router-link>
                        </li>
                        <li class="nav-item" v-show="!LoginStatus">
                            <router-link to="/tower" class="btn btn-primary">Đăng ký</router-link>
                        </li>
                    </ul>
                    <ul class="navbar-nav mb-2 mb-lg-0 align-items-start ">

                    </ul>
                    <BDropdown v-show="LoginStatus" variant="link-secondary" auto-close="outside" class="info card-header-dropdown py-0" toggle-class="text-reset dropdown-btn arrow-none me-0" menu-class="dropdown-menu-end dropdown-user-profile dropdown-menu-end pc-h-dropdown" aria-haspopup="true" :offset="{ alignmentAxis: -145, crossAxis: 0, mainAxis: 20 }">
                        <template  #button-content><span class="text-muted"> <img src="/images/avatar/324413887_476531747779411_8255796672765316904_n.jpg" alt="user-image" class="user-avtar">
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
                                                    <img src="/images/avatar/324413887_476531747779411_8255796672765316904_n.jpg" alt="user-image" class="wid-50 rounded-circle">
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
                                            <div class="dropdown-item">
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
                                            <router-link to="/login" class="dropdown-item">
                                                <span class="d-flex align-items-center">
                                                    <i class="ph-duotone ph-power"></i>
                                                    <span>Đăng xuất</span>
                                                </span>
                                            </router-link>
                                        </li>
                                    </ul>
                                </div>
                            </simplebar>
                        </div>
                    </BDropdown>
                    <BModal v-model="modalShow" hide-footer class="v-modal-custom" id="login-modal" centered>
                        <template #title>
                            <h4 class="f-w-500 mb-1">Login with your email</h4>
                            <p class="mb-3 pb-0">Don't have an Account?<a href="#" class="link-primary ms-2" data-bs-toggle="modal"
                                    data-bs-target="#registration-modal" @click="toggleModal">Create Account</a></p>
                        </template>
                        <div class="form-group mb-3">
                            <label class="form-label">Email address</label>
                            <input type="email" class="form-control" placeholder="Email Address">
                        </div>
                        <div class="form-group mb-3">
                            <label class="form-label">Password</label>
                            <input type="password" class="form-control" placeholder="Password">
                        </div>
                        <div class="d-flex mt-1 justify-content-between align-items-center">
                            <div class="form-check">
                                <input class="form-check-input input-primary" type="checkbox" id="customCheckc1" checked="">
                                <label class="form-check-label text-muted" for="customCheckc1">Remember me?</label>
                            </div>
                        </div>
                        <div class="d-grid mt-4">
                            <button type="button" class="btn btn-primary">Login</button>
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