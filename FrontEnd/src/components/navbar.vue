<script>
import simplebar from "simplebar-vue"
import Axios from "axios";
import apiClient from "@/plugins/axios";
import store from "../state/store";

export default {
    name: "NAVBAR",
    components: {
        simplebar
    },
    data() {
        return {
            isFullscreen: false,
            isSidebarHidden: false,
            currentMode: 'light',
            viewdialogChangePassword: false,
            changePassword:{
                id: 0,
                passwordOld: '',
                passwordNew: '',
            },
            formChange: false,
            viewdialogInfo: false,
            bankData: [],
            viewdialogEditBank: false,
            passwordConfirm: '',
            showPassword: false,
            showPassword1: false,
            showPassword2: false,
            message: '',
            snackbar: false,
            snackbarColor: '',
            rules: {
                required: v => !!v || 'Vui lòng không để trống',
                validEmail: v => /^[a-zA-Z0-9._%+-]+@gmail\.com$/.test(v) || 'Email phải hợp lệ và kết thúc bằng @gmail.com',
                validPassword: v=> /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/.test(v) || 'Mật khẩu phải có ít nhất 8 ký tự, bao gồm ít nhất 1 chữ cái và 1 chữ số',
                matchPassword: (v) =>v === this.changePassword.passwordNew || 'Mật khẩu xác nhận không khớp',
            },
        };
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
        toggleSidebar() {
            this.$store.commit('toggleSidebar');
        },
        toggleMobileSidebar() {
            this.$store.commit('toggleMobileSidebar');
        },
        required (v) {
            return !!v || 'Vui lòng không để trống'
        },
        btnInfo(){
            
        },
        btnEditBank(){
            Axios.get(`https://api.vietqr.io/v2/banks`)
                    .then(response=>{
                        this.bankData = response.data.data.map(bank => ({
                            name: bank.name,
                            shortName: bank.shortName
                        }));
                    })
                    .catch(error=>{
                        this.message = "Lấy danh sách ngân hàng bị lỗi " + error;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
        },
        SignOut(){
            this.$store.dispatch('logout');
        },
        btnChangePassword(){
            this.changePassword.id = store.getters['getUserId'];
            console.log(this.changePassword);
            apiClient.put(`/Landlord/ChangePassword`,this.changePassword)
                    .then(()=>{
                        this.message = "Đổi mật khẩu thành công ";
                        this.snackbar = true;
                        this.snackbarColor = 'green';
                        this.viewdialogChangePassword =false;
                        
                    })
                    .catch(error=>{
                        this.message = "Đổi mật khẩu bị lỗi: " + error.response.data.message;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
        }
    },
}
</script>

<template>
    <div class="header-wrapper"> <!-- [Mobile Media Block] start -->
        <v-snackbar
            v-model="snackbar"
            :timeout="10000"
            class="custom-snackbar"
            :color="snackbarColor"
        >
            <h5 class="text-center">{{ message }}</h5>
        </v-snackbar>
        <div class="me-auto pc-mob-drp">
            <ul class="list-unstyled">
                <!-- ======= Menu collapse Icon ===== -->
                <li class="pc-h-item pc-sidebar-collapse">
                    <a href="#" class="pc-head-link ms-0" id="sidebar-hide" @click="toggleSidebar">
                        <i class="ti ti-menu-2"></i>
                    </a>
                </li>
                <li class="pc-h-item pc-sidebar-popup">
                    <a href="#" class="pc-head-link ms-0" id="mobile-collapse" @click="toggleMobileSidebar">
                        <i class="ti ti-menu-2"></i>
                    </a>
                </li>
            </ul>
        </div>
        <div class="ms-auto">
            <ul class="list-unstyled">
                <BDropdown variant="link-secondary" class="card-header-dropdown pb-0 pt-2" toggle-class="text-reset dropdown-btn arrow-none" menu-class="dropdown-menu-end" aria-haspopup="true" :offset="{ alignmentAxis: -150, crossAxis: 0, mainAxis: 20 }">
                    <template #button-content><span class="text-muted"><i :class="currentMode === 'dark' ? 'ph-duotone ph-moon' : (currentMode === 'light' ? 'ph-duotone ph-sun-dim' : 'ph-duotone ph-cpu')"></i></span>
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
                        <span>Mặc định</span>
                    </a>
                </BDropdown>
                <BDropdown v-model="show" variant="link-secondary" auto-close="outside" class="card-header-dropdown pb-0 pt-3" toggle-class="text-reset dropdown-btn arrow-none me-0" menu-class="dropdown-menu-end dropdown-notification pc-h-dropdown" aria-haspopup="true" :offset="{ alignmentAxis: -145, crossAxis: 0, mainAxis: 20 }">
                    <template #button-content><span class="text-muted"><i class="ph-duotone ph-bell"></i>
                            <span class="position-absolute topbar-badge translate-middle badge rounded-pill bg-success"><span class="notification-badge">4</span><span class="visually-hidden">unread
                                    messages
                                </span>
                            </span>
                        </span>
                    </template>
                    <BDropdownHeader class=" align-items-center justify-content-between">
                        <BRow class="align-items-center justify-content-between">
                            <BCol xl="8" sm="6">
                                <h5 class="m-0">Thông báo</h5>
                            </BCol>
                            <BCol xl="4" sm="6">
                                <ul class="list-inline ms-auto mb-0">
                                    <li class="list-inline-item">
                                        <router-link to="/mail" class="avtar avtar-s btn-link-hover-primary">
                                            <i class="ti ti-link f-18"></i>
                                        </router-link>
                                    </li>
                                </ul>
                            </BCol>
                        </BRow>

                    </BDropdownHeader>
                    <simplebar data-simplebar style="max-height: 500px;">
                        <div class="dropdown-body text-wrap header-notification-scroll position-relative" style="max-height: calc(100vh - 235px)">
                            <ul class="list-group list-group-flush">
                                <li class="list-group-item">
                                    <p class="text-span">Ngày hôm nay</p>
                                    <div class="d-flex">
                                        <div class="flex-grow-1 ms-3">
                                            <div class="d-flex">
                                                <div class="flex-grow-1 me-3 position-relative">
                                                    <h6 class="mb-0 text-truncate">Phòng 102 đã có người yêu cầu xem phòng</h6>
                                                </div>
                                                <div class="flex-shrink-0">
                                                    <span class="text-sm">2 min ago</span>
                                                </div>
                                            </div>
                                            <p class="position-relative mt-1 mb-2"><br><span class="text-truncate">Số điện thoại của khách yêu cầu xem: 0359988934</span></p>
                                        </div>
                                    </div>
                                </li>
                                <li class="list-group-item">
                                    <div class="d-flex">
                                        <div class="flex-grow-1 ms-3">
                                            <div class="d-flex">
                                                <div class="flex-grow-1 me-3 position-relative">
                                                    <h6 class="mb-0 text-truncate">Phòng 103 đã có người yêu cầu xem phòng</h6>
                                                </div>
                                                <div class="flex-shrink-0">
                                                    <span class="text-sm">2 min ago</span>
                                                </div>
                                            </div>
                                            <p class="position-relative mt-1 mb-2"><br><span class="text-truncate">Số điện thoại của khách yêu cầu xem: 0856456421</span></p>
                                        </div>
                                    </div>
                                </li>
                                <li class="list-group-item">
                                    <p class="text-span">Ngày hôm qua</p>
                                    <div class="d-flex">
                                        <div class="flex-grow-1 ms-3">
                                            <div class="d-flex">
                                                <div class="flex-grow-1 me-3 position-relative">
                                                    <h6 class="mb-0 text-truncate">Phòng 305 đã có người yêu cầu xem phòng</h6>
                                                </div>
                                                <div class="flex-shrink-0">
                                                    <span class="text-sm">2 min ago</span>
                                                </div>
                                            </div>
                                            <p class="position-relative mt-1 mb-2"><br><span class="text-truncate">Số điện thoại của khách yêu cầu xem: 0359988934</span></p>
                                        </div>
                                    </div>
                                </li>
                                <li class="list-group-item">
                                    <div class="d-flex">
                                        <div class="flex-grow-1 ms-3">
                                            <div class="d-flex">
                                                <div class="flex-grow-1 me-3 position-relative">
                                                    <h6 class="mb-0 text-truncate">Phòng 303 đã có người yêu cầu xem phòng</h6>
                                                </div>
                                                <div class="flex-shrink-0">
                                                    <span class="text-sm">2 min ago</span>
                                                </div>
                                            </div>
                                            <p class="position-relative mt-1 mb-2"><br><span class="text-truncate">Số điện thoại của khách yêu cầu xem: 0856456421</span></p>
                                        </div>
                                    </div>
                                </li>
                                <li class="list-group-item">
                                    <div class="d-flex">
                                        <div class="flex-shrink-0">
                                            <div class="avtar avtar-s bg-light-success">
                                                <i class="ph-duotone ph-shield-checkered f-18"></i>
                                            </div>
                                        </div>
                                        <div class="flex-grow-1 ms-3">
                                            <div class="d-flex">
                                                <div class="flex-grow-1 me-3 position-relative">
                                                    <h5 class="mb-0 text-truncate">Security</h5>
                                                </div>
                                                <div class="flex-shrink-0">
                                                    <span class="text-sm text-muted">5 hour ago</span>
                                                </div>
                                            </div>
                                            <p class="position-relative text-muted mt-1 mb-2">Lorem Ipsum is simply
                                                dummy text of the
                                                printing and
                                                typesetting industry. Lorem Ipsum has been the industry's standard
                                                dummy text ever since the 1500s.</p>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </SimpleBar>
                    <div class="dropdown-footer">
                        <BRow class="g-3">
                            <BCol Cols="6">
                                <div class="d-grid"><button class="btn btn-primary">Archive all</button></div>
                            </BCol>
                            <BCol Cols="6">
                                <div class="d-grid"><button class="btn btn-outline-secondary">Mark all as
                                        read</button></div>
                            </BCol>
                        </BRow>
                    </div>
                </BDropdown>
                <BDropdown variant="link-secondary" auto-close="outside" class="card-header-dropdown py-0" toggle-class="text-reset dropdown-btn arrow-none me-0" menu-class="dropdown-menu-end dropdown-user-profile dropdown-menu-end pc-h-dropdown" aria-haspopup="true" :offset="{ alignmentAxis: -145, crossAxis: 0, mainAxis: 20 }">
                    <template #button-content><span class="text-muted"> <img src="/images/avatar/324413887_476531747779411_8255796672765316904_n.jpg" alt="user-image" class="user-avtar">
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
                                        <div @click="(viewdialogEditBank = !iewdialogEditBank) &&(btnEditBank())" class="dropdown-item">
                                            <span class="d-flex align-items-center">
                                                <i class="ph-duotone ph-gear-six"></i>
                                                <span>Cấu hình</span>
                                            </span>
                                        </div>
                                        <router-link to="/login" class="dropdown-item" @click="SignOut()">
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
            </ul>
        </div>
    </div>
    <BModal v-model="viewdialogChangePassword" hide-footer title="Đổi mật khẩu" modal-class="fadeInRight"
        class="v-modal-custom" centered size="md" >
        <div class="card-body">
            <v-form v-model="formChange" ref="formChange">
                <BRow>
                    <BCol class="col-lg-12">
                        <div class="form-group">
                            <label class="form-label">Mật khẩu cũ:</label>
                            <v-text-field 
                                :type="showPassword ? 'text' : 'password'" 
                                v-model="changePassword.passwordOld" 
                                :rules="[rules.required]" 
                                variant="outlined" 
                                clearable 
                                placeholder="Nhập vào mật khẩu cũ" 
                                class="input-control"
                                :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                                @click:append-inner="showPassword = !showPassword"
                            ></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-12">
                        <div class="form-group">
                            <label class="form-label">Mật khẩu mới:</label>
                            <v-text-field 
                                :type="showPassword1 ? 'text' : 'password'" 
                                v-model="changePassword.passwordNew" 
                                :rules="[rules.validPassword]"
                                variant="outlined" 
                                clearable
                                placeholder="Nhập vào mật khẩu mới"
                                class="input-control"
                                :append-inner-icon="showPassword1 ? 'mdi-eye' : 'mdi-eye-off'"
                                @click:append-inner="showPassword1 = !showPassword1"
                            ></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-12">
                        <div class="form-group">
                            <label class="form-label">Mật khẩu mới:</label>
                            <v-text-field 
                                :type="showPassword2 ? 'text' : 'password'" 
                                v-model="passwordConfirm" 
                                :rules="[rules.matchPassword]" 
                                variant="outlined" 
                                clearable 
                                placeholder="Nhập lại mật khẩu" 
                                class="input-control"
                                :append-inner-icon="showPassword2 ? 'mdi-eye' : 'mdi-eye-off'"
                                @click:append-inner="showPassword2 = !showPassword2"
                            ></v-text-field>
                        </div>
                    </BCol>
                </BRow>
            </v-form>
        </div>
        <div class="modal-footer v-modal-footer">
            <BButton type="button" variant="light" @click="viewdialogChangePassword = false">Close
            </BButton>
            <BButton type="button" variant="primary" @click="btnChangePassword()" :disabled="!formChange">Save Changes</BButton>
        </div>
    </BModal>
    <BModal v-model="viewdialogInfo" hide-footer title="Đổi thông tin cá nhân" modal-class="fadeInRight"
        class="v-modal-custom" centered size="xl" >
        <div class="card-body">
            <v-form v-model="form" ref="form">
                <BRow>
                    <BCol class="col-lg-12 text-center">
                        <div class="form-group">
                            <v-avatar image="/images/avatar/324413887_476531747779411_8255796672765316904_n.jpg" size="80"></v-avatar>
                        </div>
                    </BCol>
                    <BCol class="col-lg-6">
                        <div class="form-group">
                            <label class="form-label">Họ và tên:</label>
                            <v-text-field v-model="changePassword.passwordOld" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào họ và tên" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-6">
                        <div class="form-group">
                            <label class="form-label">Ngày sinh:</label>
                            <input type="date" class="form-control" id="example-datemin" :rules="[required]" v-model="changePassword.doB" min="2000-01-02">
                        </div>
                    </BCol>
                    <BCol class="col-lg-4">
                        <div class="form-group">
                            <label class="form-label">Số điện thoại:</label>
                            <v-text-field v-model="changePassword.passwordNew" :rules="[required]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào số điện thoại" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-4">
                        <div class="form-group">
                            <label class="form-label">Số điện thoại đăng ký Zalo:</label>
                            <v-text-field v-model="changePassword.passwordNew" :rules="[required]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào số điện thoại đăng ký zalo" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-4">
                        <div class="form-group">
                            <label class="form-label">CCCD:</label>
                            <v-text-field v-model="changePassword.passwordNew" :rules="[required]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào căn cước công dân" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-5">
                        <div class="form-group">
                            <label class="form-label">Email:</label>
                            <v-text-field v-model="changePassword.passwordNew" :rules="[required]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào email" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-2 d-flex justify-center align-center">
                        <v-btn>Gửi mã</v-btn>
                    </BCol>
                    <BCol class="col-lg-5">
                        <div class="form-group">
                            <label class="form-label">Mã xác nhận:</label>
                            <v-text-field v-model="changePassword.passwordNew" :rules="[required]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào mã xác nhận được gửi qua email" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-12">
                        <div class="form-group">
                            <label class="form-label">Địa chỉ:</label>
                            <v-text-field v-model="passwordConfirm" :rules="[required]" type="text" @input="formatPrice" variant="outlined" clearable class="input-control"></v-text-field>
                        </div>
                    </BCol>
                    
                </BRow>
            </v-form>
        </div>
        <div class="modal-footer v-modal-footer">
            <BButton type="button" variant="light" @click="viewdialogChangePassword = false">Close
            </BButton>
            <BButton type="button" variant="primary" @click="CreateEditService()" :disabled="!formChange">Save Changes</BButton>
        </div>
    </BModal>
    <BModal v-model="viewdialogEditBank" hide-footer title="Chỉnh sửa thông tin ngân hàng" modal-class="fadeInRight"
    class="v-modal-custom" centered size="md">
        <div class="card-body">
            <v-form v-model="form" ref="form">
                <BRow>
                    <BCol class="col-lg-6">
                        <div class="form-group">
                            <label class="form-label">Ngân hàng:</label>
                            <v-autocomplete
                                v-model="selectTinh"
                                :items="bankData"
                                item-title="shortName"
                                item-value="shortName"
                                outlined
                                dense
                                clearable
                                @update:modelValue="onTinhChange"
                            ></v-autocomplete>
                        </div>
                    </BCol>
                    <BCol class="col-lg-6">
                        <div class="form-group">
                            <label class="form-label">Số tài khoản:</label>
                            <v-text-field v-model="changePassword.passwordNew" :rules="[required]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào tên dịch vụ" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                </BRow>
            </v-form>
        </div>
    </BModal>
</template>