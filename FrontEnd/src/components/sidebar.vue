<style scoped>
    .navbar-wrapper{
        background-color: #023573;
    }
    .pc-link{
        color: azure;
        font-size: 20px;
    }
</style>
<script>
import { ref } from 'vue';
import simplebar from "simplebar-vue"
export default {
    setup() {
        const logo = ref(null);
        // let isDarkTheme = document.body.getAttribute("data-pc-layout") === "dark";

        // const updateLogo = () => {
        //     if (isDarkTheme) {
        //         logo.value.src = require('@/assets/images/logo-white.svg');
        //     } else {
        //         logo.value.src = require('@/assets/images/logo-dark.svg');
        //     }
        // };

        // onMounted(() => {
        //     updateLogo();
        // });

        // watch(() => isDarkTheme, () => {
        //     updateLogo();
        // });

        return { logo };
    },
    components: {
        simplebar
    },
    data() {
        return {
            towerId : 0,
        };
    },
    methods: {
        changeLayoutType(layoutType) {
            // Update the layout type in the store
            this.$store.commit('changeLayoutType', { layoutType });

            // Set the layout attribute based on the layout type
            document.body.setAttribute('data-pc-layout', layoutType);
        },
    },
    computed: {
        // ...layoutComputed,
        layoutType: {
            get() {
                return this.$store.state.layout.layoutType;
            },
            set(layoutType) {
                this.$store.commit('changeLayoutType', { layoutType });
            },
        },
    },
    watch: {
        layoutType: {
            immediate: true,
            deep: true,
            handler(newVal, oldVal) {
                if (newVal !== oldVal) {
                    switch (newVal) {
                        case "horizontal":
                            document.body.setAttribute(
                                "data-pc-layout",
                                "horizontal"
                            );
                            break;
                        case "vertical":
                            document.body.setAttribute("data-pc-layout", "vertical");
                    }
                }
            },
        },
    },
    mounted() {
        const idtower = this.$route.params.idtower;
          //Giải mã
          //const DecodingIdTower = CryptoJS.enc.Utf8.stringify(CryptoJS.enc.Base64.parse(idtower));
        this.towerId = idtower;
        this.isDarkTheme = document.body.getAttribute("data-pc-layout") === "dark";
        const activeListItem = document.querySelector("li.active");
        if (activeListItem) {
            const parentElementOrSelf = activeListItem?.parentElement ? activeListItem.parentElement : activeListItem;
            if (parentElementOrSelf && !parentElementOrSelf.classList.contains("pc-navbar")) {
                const closestItem = parentElementOrSelf.parentElement.closest(".pc-item");
                if (closestItem) {
                    closestItem.classList.add("active");
                }
            }
        } else {
            console.error("No list item with class 'active' found.");
        }
    }
};
</script>

<template>
    <div class="navbar-wrapper" id="navbar-wrapper">
        <div class="m-header">
            <router-link to="/" class="b-brand text-primary mt-lg-4">
                <!-- ========   Change your logo from here   ============ -->
                <!-- <img ref="logo" alt="logo image" class="logo-lg custom_logo"> -->
                <!-- <img :src="isDarkTheme ? '@/assets/images/logo-dark.svg' : '@/assets/images/logo-white.svg'" alt="logo image" class="logo-lg custom_logo"> -->
                <!-- <img src="@/assets/images/logo-dark.svg" alt="" class="logo logo-lg">
                <img src="@/assets/images/logo-white.svg" alt="" class="logo logo-lg"> -->
                <!-- <img src="@/assets/images/favicon.svg" alt="" class="logo logo-sm"> <span class="badge bg-brand-color-2 rounded-pill ms-2 theme-version">v1.0</span> -->
                <h3 style="color: azure ;">QUẢN LÝ NHÀ TRỌ</h3>
            </router-link>
        </div>
        <simplebar data-simplebar style="height: 760px" class="mt-4">
            <div class="navbar-content">
                <ul class="pc-navbar">
                    <!-- <li class="pc-item" :class="{ 'active': this.$route.path === '/dashboard' }">
                        <router-link to="/dashboard" class="pc-link">
                            <span class="pc-micon"><i class="ph-duotone ph-gauge"></i></span><span class="pc-mtext"> Dashboard</span><span class="pc-badge">2</span>
                        </router-link>
                    </li> -->
                    <li class="pc-item " :class="{ 'active': this.$route.path === `/${towerId}/homepage` }">
                        <router-link :to="`/${towerId}/homepage`" class="pc-link white">
                            <span class="pc-micon">
                                <v-icon>mdi-car-cruise-control</v-icon>
                            </span>
                            <span class="pc-mtext"> Trang chủ</span>
                        </router-link>
                    </li>
                    <li class="pc-item" :class="{ 'active': this.$route.path === `/${towerId}/room` }">
                        <router-link :to="`/${towerId}/room`" class="pc-link">
                            <span class="pc-micon">
                                <v-icon>mdi-home</v-icon>
                            </span>
                            <span class="pc-mtext"> Phòng</span>
                        </router-link>
                    </li>
                    <li class="pc-item" :class="{ 'active': this.$route.path === `/${towerId}/service` }">
                        <router-link :to="`/${towerId}/service`" class="pc-link">
                            <span class="pc-micon">
                                <i class="ph-duotone ph-database"></i>
                            </span>
                            <span class="pc-mtext"> Dịch vụ</span>
                        </router-link>
                    </li>
                    <li class="pc-item" :class="{ 'active': this.$route.path === `/${towerId}/arise` }">
                        <router-link :to="`/${towerId}/arise`" class="pc-link">
                            <span class="pc-micon">
                                <v-icon icon="mdi-cash-plus"></v-icon>
                            </span>
                            <span class="pc-mtext"> Phát sinh</span>
                        </router-link>
                    </li>
                    <!-- <li class="pc-item">
                        <a href="../elements/bc_alert.html" class="pc-link" target="_blank"><span class="pc-micon">
                                <i class="ph-duotone ph-compass-tool"></i></span><span class="pc-mtext">Components</span></a>
                    </li> -->
                    <li class="pc-item" :class="{ 'active': this.$route.path === `/${towerId}/bill` }">
                        <router-link :to="`/${towerId}/bill`" class="pc-link">
                            <span class="pc-micon">
                                <v-icon icon="mdi-currency-usd"></v-icon>
                            </span>
                            <span class="pc-mtext">Hóa đơn</span>
                        </router-link>
                    </li>
                    <li class="pc-item" :class="{ 'active': this.$route.path === `/${towerId}/contract` }">
                        <router-link :to="`/${towerId}/contract`" class="pc-link">
                            <span class="pc-micon">
                                <v-icon icon="mdi-file-document-edit-outline"></v-icon>
                            </span><span class="pc-mtext">Hợp đồng</span></router-link>
                    </li>
                    <li class="pc-item" :class="{ 'active': $route.path === `/${towerId}/report` }">
                        <router-link :to="`/${towerId}/report`" class="pc-link">
                            <span class="pc-micon">
                                <v-icon icon="mdi-chart-line"></v-icon>
                            </span>
                            <span class="pc-mtext">Báo cáo</span></router-link>
                    </li>
                    <li class="pc-item" :class="{ 'active': this.$route.path === '/tower' }">
                        <router-link to="/tower" class="pc-link">
                            <span class="pc-micon">
                                <v-icon icon="mdi-home-city"></v-icon>
                            </span>
                            <span class="pc-mtext">Khu nhà</span>
                        </router-link>
                    </li>
                    <!-- <li class="pc-item" :class="{ 'active': this.$route.path === '/image-croper' }"><router-link to="/image-croper" class="pc-link">
                            <span class="pc-micon">
                                <i class="ph-duotone ph-crop"></i>
                            </span>
                            <span class="pc-mtext"> Image cropper</span></router-link>
                    </li> -->
                </ul>

                <!-- <div class="card nav-action-card bg-brand-color-4">
                    <div class="card-body" :style="{ 'background-image': 'url(' + require('@/assets/images/layout/nav-card-bg.svg') + ')' }">
                        <h5 class="text-dark">Help Center</h5>
                        <p class="text-dark text-opacity-75">Please contact us for more questions.</p>
                        <BLink href="#" class="btn btn-primary" target="_blank">Go to help
                            Center</BLink>
                    </div>
                </div> -->
            </div>
        </simplebar>
    </div>
    <BCard no-body class="pc-user-card">
        <BCardBody>
            <div class="d-flex align-items-center">
                <div class="flex-shrink-0">
                    <img src="/images/avatar/324413887_476531747779411_8255796672765316904_n.jpg" alt="user-image" class="user-avtar wid-45 rounded-circle">
                </div>
                <div class="flex-grow-1 ms-3 me-2">
                    <h6 class="mb-0">Phạm Quang Hưng</h6>
                    <small>Chủ nhà</small>
                </div>
                <BDropdown variant="purple" dropup no-caret>
                    <template v-slot:button-content>
                        <span class="btn btn-icon btn-link-secondary avtar arrow-none dropdown-toggle">
                            <i class="ph-duotone ph-windows-logo"></i>
                        </span>
                    </template>
                    <BRow xl="6">
                        <BCol xl="6">
                            <BDropdownItem class="pc-user-links p-0" >
                                <div class="align-middle">
                                    <i class="ph-duotone ph-user"></i>
                                    <br>
                                    <span>Tài khoản</span>
                                </div>
                            </BDropdownItem>
                        </BCol>
                        <BCol xl="6">
                            <BDropdownItem class="pc-user-links p-0">
                                <i class="ph-duotone ph-gear"></i> <br>
                                <span>Cấu hình</span>
                            </BDropdownItem>
                            <BDropdownItem class="pc-user-links p-0">
                                <router-link to="/login">
                                    <i class="ph-duotone ph-power"></i> <br>
                                    <span>Đăng xuất</span>
                                </router-link>
                            </BDropdownItem>
                        </BCol>
                    </BRow>
                </BDropdown>
            </div>
        </BCardBody>
    </BCard>
    <!-- <div class="dropdown">
                <BLink class="btn btn-icon btn-link-secondary avtar arrow-none dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false" data-bs-offset="0,20">
                    <i class="ph-duotone ph-windows-logo"></i>
                </BLink>
                <div class="dropdown-menu">
                    <ul>
                        <li>
                            <BLink class="pc-user-links">
                                <i class="ph-duotone ph-user"></i>
                                <span>My Account</span>
                            </BLink>
                        </li>
                        <li>
                            <BLink class="pc-user-links">
                                <i class="ph-duotone ph-gear"></i>
                                <span>Settings</span>
                            </BLink>
                        </li>
                        <li>
                            <BLink class="pc-user-links">
                                <i class="ph-duotone ph-lock-key"></i>
                                <span>Lock Screen</span>
                            </BLink>
                        </li>
                        <li>
                            <BLink class="pc-user-links">
                                <i class="ph-duotone ph-power"></i>
                                <span>Logout</span>
                            </BLink>
                        </li>
                    </ul>
                </div>
            </div> -->
</template>

<style>
.pc-sidebar .card.pc-user-card {
    z-index: 1;
}
</style>