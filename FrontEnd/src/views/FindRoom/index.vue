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
import store from "../../state/store";
import Axios from "axios";
import CryptoJS from 'crypto-js';




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
            showPassword1: false,
            showPassword2: false,
            modalRegister: false,
            form: false,
            formRegister: false,
            message: '',
            snackbar: false,
            snackbarColor: '',
            errorMessage: '',
            remember: false,
            viewdialogChangePassword: false,
            formChange: false,
            imgAvatar: null,
            previewImage: null,
            viewdialogInfo: false,
            handleIconClick: false,
            customerId: 0,
            address: '',
            userName:'',
            tinhData: [
                    
            ],
            huyenData: [
                
            ],
            phuongData: [
                
            ],
            selectTinh: null,
            selectHuyen: null,
            selectPhuong: null,
            changePassword:{

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
            rules: {
                required: v => !!v || 'Vui lòng không để trống',
                validPhone: v => /^0\d{9}$/.test(v) || 'Số điện thoại phải có 10 chữ số và bắt đầu bằng 0',
                validEmail: v => /^[a-zA-Z0-9._%+-]+@gmail\.com$/.test(v) || 'Email phải hợp lệ và kết thúc bằng @gmail.com',
                validPassword: v=> /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/.test(v) || 'Mật khẩu phải có ít nhất 8 ký tự, bao gồm ít nhất 1 chữ cái và 1 chữ số',
                matchPassword: (v) =>v === this.changePassword.passwordNew || 'Mật khẩu xác nhận không khớp',
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
            this.customerId = store.getters['getCustomerId'];
            this.GetInfo();
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
            this.login.email = '';
            this.login.password = '';
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
                        this.customerId = response.data.userId;
                        this.GetInfo();
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
            window.location.reload();
        },
        Register(){
            apiClient.post(`/Customer/Create`,this.register)
                    .then(()=>{
                        this.message = `Đăng ký tài khoản thành công. Mật khẩu đã được gửi qua ${this.register.email}. Bạn vui lòng vào email để lấy mật khẩu`;
                        this.snackbar = true;
                        this.snackbarColor = 'green';
                        this.modalRegister = false;
                    })
                    .catch((error) => {
                        // Hiển thị thông báo lỗi nếu có
                        this.message = `Đăng ký tài khoản đã xảy ra lỗi: ${error}`;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    });
        },
        btnChangePassword(){
            this.changePassword.id = this.customerId;
            apiClient.put(`/Customer/ChangePassword`,this.changePassword)
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
        async btnInfo(){
            this.previewImage = null;
            const landlordId = store.getters['getCustomerId'];
            apiClient.get(`/Customer/GetInfo?Id=${landlordId}`)
                    .then(response=>{
                        this.infoUser = response.data;
                        const dateTime = new Date(this.infoUser.doB); // Ví dụ có giờ
                        dateTime.setDate(dateTime.getDate() + 1); // Cộng thêm 1 ngày
                        this.infoUser.doB = dateTime.toISOString().split('T')[0]; // Kết quả sẽ là "2025-01-07"
                    })
                    .catch(error=>{
                        this.message = "Lấy thông tin người dùng bị lỗi: " + error.response.data.message;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
            this.imgAvatar = await this.loadImages(this.infoUser.pathAvatar);
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
        UpdateInfo(){
            const formData = new FormData();
            formData.append("id",this.customerId);
            formData.append("fullName", this.infoUser.fullName);
            formData.append("doB", this.infoUser.doB);
            formData.append("PhoneNumber",this.infoUser.phoneNumber);
            formData.append("email", this.infoUser.email);
            formData.append("cccd", this.infoUser.cccd);
            formData.append("address",this.infoUser.address);
            formData.append("sdtZalo", this.infoUser.sdtZalo);
            formData.append("imgAvatar", this.imgAvatar);
            apiClient.put(`/Customer/Update`,formData)
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
        onFileSelected(event) {
            const file = event.target.files[0];
            if (file) {
                this.imgAvatar = file;
                this.previewImage = URL.createObjectURL(file); // Tạo URL để xem trước
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
        GetInfo(){
            apiClient.get(`/Customer/GetInfoContact?id=${this.customerId}`)
                    .then(response=>{
                        this.infoUser.pathAvatar = response.data.pathAvatar;
                        this.userName = response.data.fullName;
                    })
                    .catch(error=>{
                        this.message = `Đã xảy ra lỗi: ${error.response?.data?.message || error.message}`;
                        this.snackbarColor = 'red';
                        this.snackbar = true;
                    })
        },
        GotoPost(postid){
            const encryptedId = CryptoJS.enc.Base64.stringify(CryptoJS.enc.Utf8.parse(postid));
            this.$router.push({ name: 'editpost', params: { idpost: encryptedId } });
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
                        <li class="nav-item px-1" v-show="LoginStatus">
                            <h5><router-link to="/save" class="nav-link" >Phòng, tin đã lưu</router-link></h5>
                        </li>
                        <li class="nav-item px-1">
                            <h5><router-link to="/viewbill" class="nav-link"> Hoá đơn của bạn</router-link></h5>
                        </li>
                        <li class="nav-item" v-show="LoginStatus">
                            <button @click="GotoPost(0)" class="btn btn-primary">Đăng bài</button>
                        </li>
                        <li class="nav-item" v-show="!LoginStatus">
                            <button @click="ViewLogin()" class="btn btn-primary mr-4">Đăng nhập</button>
                        </li>
                    </ul>
                    <BDropdown v-show="LoginStatus" variant="link-secondary" auto-close="outside" class="info card-header-dropdown py-0" toggle-class="text-reset dropdown-btn arrow-none me-0" menu-class="dropdown-menu-end dropdown-user-profile dropdown-menu-end pc-h-dropdown" aria-haspopup="true" :offset="{ alignmentAxis: -145, crossAxis: 0, mainAxis: 20 }">
                        <template  #button-content><span class="text-muted"> <img :src="infoUser.pathAvatar" alt="user-image" class="user-avtar">
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
                                                    <h5 class="mb-0">{{ userName }}</h5>
                                                </div>
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
                                            <router-link to="/listPost" class="dropdown-item">
                                                <span class="d-flex align-items-center">
                                                    <i class="ph-duotone ph-gear-six"></i>
                                                    <span>Quản lý bài đăng</span>
                                                </span>
                                            </router-link>
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
                            <p class="m-0 pb-0">Chưa có tài khoản?<a href="#" class="link-primary ms-2" 
                                    data-bs-target="#registration-modal" @click="toggleModal">Tạo tài khoản</a></p>
                        </template>
                        <v-form v-model="form">
                            <div class="form-group">
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
                            <div class="form-group">
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
                            <button type="button" @click="Login()" :disabled="!form" class="btn btn-primary">Đăng nhập</button>
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
                                <BCol class="col-lg-12">
                                    <div class="form-group m-0">
                                        <label class="form-label">Địa chỉ</label>
                                        <v-text-field 
                                            type="text" 
                                            variant="outlined" 
                                            placeholder="Địa chỉ thường trù" 
                                            :rules="[rules.required]" 
                                            v-model="register.address"
                                        ></v-text-field>                                     
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
                            <button type="button" @click="Register()" :disabled="!form" class="btn btn-primary">Đăng ký</button>
                        </div>
                    </BModal>
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
                                            <v-text-field v-model="infoUser.fullName" :rules="[rules.required]" variant="outlined" clearable placeholder="Nhập vào họ và tên" class="input-control"></v-text-field>
                                        </div>
                                    </BCol>
                                    <BCol class="col-lg-4">
                                        <div class="form-group">
                                            <label class="form-label">Ngày sinh:</label>
                                            <input type="date" class="form-control" id="example-datemin" :rules="[rules.required]" v-model="infoUser.doB" min="2000-01-02">
                                        </div>
                                    </BCol>
                                    <BCol class="col-lg-4">
                                        <div class="form-group">
                                            <label class="form-label">Số điện thoại:</label>
                                            <v-text-field v-model="infoUser.phoneNumber" :rules="[rules.required]" type="text" variant="outlined" clearable placeholder="Nhập vào số điện thoại"></v-text-field>
                                        </div>
                                    </BCol>
                                    <BCol class="col-lg-4">
                                        <div class="form-group">
                                            <label class="form-label">Số điện thoại đăng ký Zalo:</label>
                                            <v-text-field v-model="infoUser.sdtZalo" :rules="[rules.required]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào số điện thoại đăng ký zalo" class="input-control"></v-text-field>
                                        </div>
                                    </BCol>
                                    <BCol class="col-lg-4">
                                        <div class="form-group">
                                            <label class="form-label">CCCD:</label>
                                            <v-text-field v-model="infoUser.cccd" :rules="[rules.required]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào căn cước công dân" class="input-control"></v-text-field>
                                        </div>
                                    </BCol>
                                    <BCol class="col-lg-4">
                                        <div class="form-group">
                                            <label class="form-label">Email:</label>
                                            <v-text-field v-model="infoUser.email" :rules="[rules.required]" type="text" @input="formatPrice" variant="outlined" clearable placeholder="Nhập vào email" class="input-control"></v-text-field>
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
                                            <v-text-field v-model="infoUser.address" :rules="[rules.required]" type="text" readonly @click="handleIconClick = !handleIconClick" variant="outlined" class="input-control"></v-text-field>
                                        </div>
                                    </BCol>
                                </BRow>
                            </v-form>
                        </div>
                        <div class="modal-footer v-modal-footer">
                            <BButton type="button" variant="light" @click="viewdialogInfo = false">Close
                            </BButton>
                            <BButton type="button" variant="primary" @click="UpdateInfo()" :disabled="!form">Save Changes</BButton>
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