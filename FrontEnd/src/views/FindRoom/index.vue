<style scoped>

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
            tinhData: [],
            huyenData: [],
            phuongData: [],
            selectTinh: null,
            selectHuyen: null,
            selectPhuong: null,
            searchPrice: [0,10000000],
            status: false,
            length: 3,
            window: 0,
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
        }
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
        <BNav class="navbar navbar-expand-md navbar-light default">
            <div class="container">
                <a class="pc-navbar-brand" href="/">
                    TÌM KIẾM NHÀ TRỌ
                </a>
                <button @click="toggleMenu" class="navbar-toggler rounded" type="button" data-bs-toggle="collapse" data-bs-target="#navbarTogglerDemo01" aria-controls="navbarTogglerDemo01" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarTogglerDemo01">
                    <ul class="navbar-nav ms-auto mt-lg-0 mt-2 mb-2 mb-lg-0 align-items-start">
                        <li class="nav-item px-1">
                            <h5><router-link class="nav-link" to="/dashboard">Tìm Phòng trọ</router-link></h5>
                        </li>
                        <li class="nav-item px-1">
                            <h5><router-link class="nav-link" to="/components/alert">Tìm người ở ghép</router-link></h5>
                        </li>
                        <li class="nav-item px-1">
                            <h5><a class="nav-link" href="https://moonthemes-3.gitbook.io/moon-able-bootstrap/" target="_blank">Các phòng, đăng bài đã lưu</a></h5>
                        </li>
                        <li class="nav-item px-1">
                            <h5><a class="nav-link" href="https://moonthemes-3.gitbook.io/moon-able-bootstrap/" target="_blank">Bài đăng của tôi</a></h5>
                        </li>
                    </ul>
                    <ul class="navbar-nav mb-2 mb-lg-0 align-items-start">
                        <BDropdown variant="link-secondary" class="card-header-dropdown pb-0 pe-2" toggle-class="text-reset dropdown-btn arrow-none" menu-class="dropdown-menu-end" aria-haspopup="true" :offset="{ alignmentAxis: -150, crossAxis: 0, mainAxis: 20 }">
                            <template #button-content><span class="text-muted"><i :class="currentMode === 'dark' ? 'ph-duotone ph-moon' : (currentMode === 'light' ? 'ph-duotone ph-sun-dim' : 'ph-duotone ph-cpu') "></i></span>
                            </template>
                            <a href="#!" class="dropdown-item" @click="changeMode('dark')">
                                <i class="ph-duotone ph-moon"></i>
                                <span>Dark</span>
                            </a>
                            <a href="#!" class="dropdown-item" @click="changeMode('light')">
                                <i class="ph-duotone ph-sun-dim"></i>
                                <span>Light</span>
                            </a>
                            <a href="#!" class="dropdown-item" @click="changeMode('auto')">
                                <i class="ph-duotone ph-cpu"></i>
                                <span>Default</span>
                            </a>
                        </BDropdown>
                        <li class="nav-item">
                            <router-link to="/tower" class="btn btn-primary">Đăng bài</router-link>
                        </li>
                    </ul>
                </div>
            </div>
        </BNav>
        <router-view>

        </router-view>
    </header>

    <!-- [ layout-card ] end -->

    <Rightbar />
</template>