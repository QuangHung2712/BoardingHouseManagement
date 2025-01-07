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
            previewImage: null,
            dataUpdatePayment:{
                bank: null,
                stk: '',
                paymentQRImageLink: null,
                img: null
            },
            infoUser:{
                fullName: '',
                doB: null,
                phoneNumber: '',
                email: '',
                cccd: '',
                address: '',
                sdtZalo: '',
            },
            imgAvatar: null,
            form: false,
            landlordId: 0,
            tinhData: [
                    
            ],
            huyenData: [
                
            ],
            phuongData: [
                
            ],
            selectTinh: null,
            selectHuyen: null,
            selectPhuong: null,
            handleIconClick: false,
            address: '',
            rules: {
                required: v => !!v || 'Vui lòng không để trống',
                validEmail: v => /^[a-zA-Z0-9._%+-]+@gmail\.com$/.test(v) || 'Email phải hợp lệ và kết thúc bằng @gmail.com',
                validPassword: v=> /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/.test(v) || 'Mật khẩu phải có ít nhất 8 ký tự, bao gồm ít nhất 1 chữ cái và 1 chữ số',
                matchPassword: (v) =>v === this.changePassword.passwordNew || 'Mật khẩu xác nhận không khớp',
            },
        };
    },
    created(){
        this.landlordId =  localStorage.getItem('landlordId');
        this.GetInfo();

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
        async btnInfo(){
            this.previewImage = null
            apiClient.get(`/Landlord/GetDetail/${this.landlordId}`)
                    .then(response=>{
                        this.infoUser = response.data;

                    })
                    .catch(error=>{
                        this.message = "Lấy thông tin người dùng bị lỗi: " + error.response.data.message;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
            this.imgAvatar = await this.loadImages(this.infoUser.pathAvatar);
            const dateTime = new Date(this.infoUser.doB); // Ví dụ có giờ
            dateTime.setDate(dateTime.getDate() + 1); // Cộng thêm 1 ngày
            this.infoUser.doB = dateTime.toISOString().split('T')[0]; // Kết quả sẽ là "2025-01-07"
            await Axios.get(`https://esgoo.net/api-tinhthanh/1/0.htm`)
                            .then(response=>{
                                this.tinhData =  response.data.data;
                            })
                            .catch(error=>{
                                this.message = "Lấy danh sách tỉnh thành bị lỗi " + error;
                                this.snackbar = true;
                                this.snackbarColor = 'red';
                            })
        },
        async btnEditBank(){
            this.previewImage = null
            await Axios.get(`https://api.vietqr.io/v2/banks`)
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
            await apiClient.get(`/Landlord/GetInfoPayment?id=${this.landlordId}`)
                            .then(response=>{
                                this.dataUpdatePayment = response.data;
                            })
                            .catch(error=>{
                                this.message = "Lấy thông tin thanh toán bị lỗi: " + error.response.data.message;
                                this.snackbar = true;
                                this.snackbarColor = 'red';
                            })
                            console.log(this.dataUpdatePayment.paymentQRImageLink);
            this.dataUpdatePayment.img = await this.loadImages(this.dataUpdatePayment.paymentQRImageLink)
        },
        SignOut(){
            this.$store.dispatch('logout');
        },
        btnChangePassword(){
            this.changePassword.id = store.getters['getUserId'];
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
        },
        btnViewChangePass(){
            this.changePassword.passwordOld ='';
            this.changePassword.passwordNew='';
            this.passwordConfirm = '';
        },
        onFileSelected(event) {
            const file = event.target.files[0];
            if (file) {
                this.dataUpdatePayment.img = file;
                this.imgAvatar = file;
                this.previewImage = URL.createObjectURL(file); // Tạo URL để xem trước
            }
        },
        UpdateInfoPayment(){
            // Tạo FormData để gửi dữ liệu theo dạng multipart/form-data
            const formData = new FormData();
            formData.append("id",this.landlordId);
            formData.append("bank", this.dataUpdatePayment.bank);
            formData.append("sTK", this.dataUpdatePayment.stk);
            if (this.dataUpdatePayment.img) {
                formData.append("paymentQRImageLink", this.dataUpdatePayment.img);
            }
            console.log(this.dataUpdatePayment.img);
            apiClient.put(`/Landlord/UpdateInfoPayment`,formData)
                    .then(()=>{
                        this.snackbarColor = 'green';
                        this.snackbar = true;
                        this.message = 'Cập nhật thông tin thanh toán thành công!';
                        this.viewdialogEditBank = false;
                    })
                    .catch(error=>{
                        this.message = `Đã xảy ra lỗi: ${error.response?.data?.message || error.message}`;
                        this.snackbarColor = 'red';
                        this.snackbar = true;
                    })
        },
        UpdateInfo(){
            const formData = new FormData();
            formData.append("landlordId",this.landlordId);
            formData.append("fullName", this.infoUser.fullName);
            formData.append("doB", this.infoUser.doB);
            formData.append("PhoneNumber",this.infoUser.phoneNumber);
            formData.append("email", this.infoUser.email);
            formData.append("cccd", this.infoUser.cccd);
            formData.append("address",this.infoUser.address);
            formData.append("sdtZalo", this.infoUser.sdtZalo);
            formData.append("imgAvatar", this.imgAvatar);
            apiClient.put(`/Landlord/UpdateLandlord`,formData)
                    .then(()=>{
                        this.snackbarColor = 'green';
                        this.snackbar = true;
                        this.message = 'Cập nhật thông tin người dùng thành công!';
                        this.viewdialogInfo = false;
                        window.location.reload();
                    })
                    .catch(error=>{
                        this.message = `Đã xảy ra lỗi: ${error.response?.data?.message || error.message}`;
                        this.snackbarColor = 'red';
                        this.snackbar = true;
                    })
        },
        GetInfo(){
            apiClient.get(`/Landlord/GetInfoContact?id=${this.landlordId}`)
                    .then(response=>{
                        this.infoUser.pathAvatar = response.data.pathAvatar;
                    })
                    .catch(error=>{
                        this.message = `Đã xảy ra lỗi: ${error.response?.data?.message || error.message}`;
                        this.snackbarColor = 'red';
                        this.snackbar = true;
                    })
        },
        async loadImages(path){
            if(path)
            try {
                const response = await fetch(path);
                const blob = await response.blob(); // Chuyển phản hồi thành Blob
                const file = new File([blob], path.split('/').pop(), { type: blob.type }); // Tạo File từ Blob
                return file;
            } catch (error) {
                this.message = `Lỗi khi tải ảnh :`, error;
                this.snackbarColor = 'red';
                this.snackbar = true;
                return null; // Trả về null nếu lỗi
            }
        },
        onTinhChange(){
            Axios.get(`https://esgoo.net/api-tinhthanh/2/${this.selectTinh}.htm`)
                        .then(response=>{
                            this.huyenData =  response.data.data;
                        })
                        .catch(error=>{
                            this.message = "Lấy danh sách tỉnh thành bị lỗi " + error;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
        },
        onHuyenChange(){
            Axios.get(`https://esgoo.net/api-tinhthanh/3/${this.selectHuyen}.htm`)
                        .then(response=>{
                            this.phuongData =  response.data.data;
                        })
                        .catch(error=>{
                            this.message = "Lấy danh sách tỉnh thành bị lỗi " + error;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
        },
        onPhuongChange(){
            const tinh = this.tinhData.find(item => item.id === this.selectTinh);
            const huyen = this.huyenData.find(item => item.id === this.selectHuyen);
            const phuong = this.phuongData.find(item => item.id === this.selectPhuong);
            if (tinh && huyen && phuong) {
                return ` ${phuong.full_name} ${huyen.full_name} ${tinh.full_name}`;
            } else {
                return ''; // Trả về chuỗi rỗng nếu có một trong các biến là null hoặc undefined
            }
        },
        FormatAddress(){
            this.infoUser.address = this.address + this.onPhuongChange();
        },
    }
            
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
                <!-- <BDropdown v-model="show" variant="link-secondary" auto-close="outside" class="card-header-dropdown pb-0 pt-3" toggle-class="text-reset dropdown-btn arrow-none me-0" menu-class="dropdown-menu-end dropdown-notification pc-h-dropdown" aria-haspopup="true" :offset="{ alignmentAxis: -145, crossAxis: 0, mainAxis: 20 }">
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
                </BDropdown> -->
                <BDropdown variant="link-secondary" auto-close="outside" class="card-header-dropdown py-0" toggle-class="text-reset dropdown-btn arrow-none me-0" menu-class="dropdown-menu-end dropdown-user-profile dropdown-menu-end pc-h-dropdown" aria-haspopup="true" :offset="{ alignmentAxis: -145, crossAxis: 0, mainAxis: 20 }">
                    <template #button-content><span class="text-muted"> <img :src="infoUser.pathAvatar" alt="user-image" class="user-avtar">
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
                                                <img :src="infoUser.pathAvatar" alt="user-image" class="wid-50 rounded-circle">
                                            </div>
                                            <div class="flex-grow-1 mx-3">
                                                <h5 class="mb-0">Phạm Quang Hưng</h5>
                                            </div>
                                            <span class="badge bg-primary">PRO</span>
                                        </div>
                                    </li>
                                    <li class="list-group-item">
                                        <div class="dropdown-item" @click="(viewdialogChangePassword = !viewdialogChangePassword) && (btnViewChangePass())">
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
                                                <span>Thông tin thanh toán</span>
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
                    <BCol class="col-lg-12 ">
                        <div class="form-group">
                            <label class="form-label">Ảnh đại diện:</label>
                            <div class="d-flex align-items-center">
                                <v-avatar :image="previewImage || infoUser.pathAvatar" size="80"></v-avatar>
                                <input type="file" @change="onFileSelected" accept="image/*" class="input-control ml-2" />
                            </div>
                        </div>
                    </BCol>
                    <BCol class="col-lg-4">
                        <div class="form-group">
                            <label class="form-label">Họ và tên:</label>
                            <v-text-field v-model="infoUser.fullName" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào họ và tên" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-4">
                        <div class="form-group">
                            <label class="form-label">Ngày sinh:</label>
                            <input type="date" class="form-control" id="example-datemin" :rules="[required]" v-model="infoUser.doB" min="2000-01-02">
                        </div>
                    </BCol>
                    <BCol class="col-lg-4">
                        <div class="form-group">
                            <label class="form-label">Số điện thoại:</label>
                            <v-text-field v-model="infoUser.phoneNumber" :rules="[required]" type="text" variant="outlined" clearable placeholder="Nhập vào số điện thoại"></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-4">
                        <div class="form-group">
                            <label class="form-label">Số điện thoại đăng ký Zalo:</label>
                            <v-text-field v-model="infoUser.sdtZalo" :rules="[required]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào số điện thoại đăng ký zalo" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-4">
                        <div class="form-group">
                            <label class="form-label">CCCD:</label>
                            <v-text-field v-model="infoUser.cccd" :rules="[required]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào căn cước công dân" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-4">
                        <div class="form-group">
                            <label class="form-label">Email:</label>
                            <v-text-field v-model="infoUser.email" :rules="[required]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào email" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                    <div v-show="handleIconClick" v-cloak>
                        <transition name="slide-up">
                            <BRow>
                                <BCol class="col-lg-4">
                                    <div class="form-group">
                                        <label class="form-label">Tỉnh</label>
                                        <v-autocomplete
                                            v-model="selectTinh"
                                            :items="tinhData"
                                            item-title="full_name"
                                            item-value="id"
                                            outlined
                                            dense
                                            clearable
                                            @update:modelValue="onTinhChange"
                                        ></v-autocomplete>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-4">
                                    <div class="form-group">
                                        <label class="form-label">Quận, huyện</label>
                                        <v-autocomplete
                                            v-model="selectHuyen"
                                            :items="huyenData"
                                            item-title="full_name"
                                            item-value="id"
                                            outlined
                                            dense
                                            clearable
                                            :disabled="!selectTinh"
                                            @update:modelValue="onHuyenChange"
                                        ></v-autocomplete>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-4">
                                    <div class="form-group">
                                        <label class="form-label">Phường, xã</label>
                                        <v-autocomplete
                                            v-model="selectPhuong"
                                            :items="phuongData"
                                            item-title="full_name"
                                            item-value="id"
                                            outlined
                                            dense
                                            clearable
                                            :disabled="!selectHuyen"
                                            @update:modelValue="onPhuongChange"
                                        ></v-autocomplete>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-12">
                                    <div class="form-group">
                                    <label class="form-label">Số nhà:</label>
                                    <v-text-field
                                        v-model="address"
                                        variant="outlined"
                                        :disabled="!selectPhuong"
                                        clearable
                                        placeholder="Nhập vào tên dịch vụ"
                                        class="input-control"
                                        @input="FormatAddress"
                                    ></v-text-field>
                                    </div>
                                </BCol>
                            </BRow>
                        </transition>
                    </div>
                    <BCol class="col-lg-12">
                        <div class="form-group">
                            <label class="form-label">Địa chỉ:</label>
                            <v-text-field v-model="infoUser.address" :rules="[required]" type="text" readonly @click="handleIconClick = !handleIconClick" variant="outlined" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                </BRow>
            </v-form>
        </div>
        <div class="modal-footer v-modal-footer">
            <BButton type="button" variant="light" @click="viewdialogChangePassword = false">Close
            </BButton>
            <BButton type="button" variant="primary" @click="UpdateInfo()" :disabled="!form">Save Changes</BButton>
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
                                v-model="dataUpdatePayment.bank"
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
                            <v-text-field v-model="dataUpdatePayment.stk" :rules="[required]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào tên dịch vụ" class="input-control"></v-text-field>
                        </div>
                    </BCol>
                    <BCol class="col-lg-12">
                        <div class="form-group m-0">
                            <label class="form-label w-100">Ảnh QR ngân hàng:</label>
                            <input type="file" @change="onFileSelected" accept="image/*" class="input-control" />
                        </div>
                        <v-img
                            :src="previewImage || dataUpdatePayment.paymentQRImageLink"
                            alt="Ảnh QR thanh toán"
                            max-height="300"
                            max-width="100%"
                            class="rounded-lg"
                        ></v-img>
                        
                    </BCol>
                </BRow>
            </v-form>
        </div>
        <div class="modal-footer v-modal-footer">
            <BButton type="button" variant="light" @click="viewdialogEditBank = false">Close
            </BButton>
            <BButton type="button" variant="primary" @click="UpdateInfoPayment()" :disabled="!form">Save Changes</BButton>
        </div>
    </BModal>
</template>