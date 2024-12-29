<script>
import Rightbar from "@/components/right-bar.vue"
import apiClient from "@/plugins/axios"
import Swal from "sweetalert2";


export default {
    name: "REGISTER",
    components: {
        Rightbar
    },
    data(){
        return{
            register:{
                fullName: '',
                doB: '',
                phoneNumber: '',
                email: '',
                cCCD: '',
                address: '',
                sDTZalo: '',
                password: '',
            },
            rules: {
                required: v => !!v || 'Vui lòng không để trống',
                validPhone: v => /^0\d{9}$/.test(v) || 'Số điện thoại phải có 10 chữ số và bắt đầu bằng 0',
                validEmail: v => /^[a-zA-Z0-9._%+-]+@gmail\.com$/.test(v) || 'Email phải hợp lệ và kết thúc bằng @gmail.com',
            },
            form: false,
            termsAccepted: false,
            message: '',
            snackbar: false,
            snackbarColor: '',
        }
    },
    methods:{
        Save(){
            apiClient.post(`/Landlord/Create`,this.register)
                    .then(()=>{
                        Swal.fire("Tạo tài khoảng thành công!", "Mật khẩu đã được gửi qua email của bạn. Bạn vui lòng vào Email để lấy mật khẩu đăng nhập", "success")
                            .then(()=>{
                                this.$router.push('/login');
                            })
                    })
                    .catch(error=>{
                        this.message = 'Đã xảy ra lỗi: ' + error.response.data.message;
                        this.snackbarColor = 'red';
                        this.snackbar = true;
                    })
        }      
    },
}
</script>

<template>
    <v-snackbar
        v-model="snackbar"
        :timeout="10000"
        class="custom-snackbar"
        :color="snackbarColor"
    >
        <h5 class="text-center">{{ message }}</h5>
    </v-snackbar>
    <div class="auth-main v2">
        <div class="bg-overlay bg-dark"></div>
        <div class="auth-wrapper">
            <div class="auth-sidecontent">

                <div class="auth-sidefooter">
                    <h4 class="text-white">QUẢN LÝ NHÀ TRỌ</h4>
                    <!-- <img src="" class="img-brand img-fluid" alt="images"> -->
                    <hr class="mb-3 mt-4">
                    <!-- <BRow class="row">
                        <BCol class="col-auto my-1">
                            <ul class="list-inline footer-link mb-0">
                                <li class="list-inline-item"><router-link to="/dashboard">Home</router-link></li>
                                <li class="list-inline-item"><a href="#"
                                        target="_blank">Documentation</a></li>
                                <li class="list-inline-item"><a href="#"
                                        target="_blank">Support</a></li>
                            </ul>
                        </BCol>
                    </BRow> -->
                </div>
            </div>
            <div class="auth-form">
                <div class="card my-5 mx-3">
                    <div class="card-body">
                        <v-form v-model="form">
                            <h4 class="f-w-500 mb-1">Đăng ký tài khoản</h4>
                            <p class="mb-3">Đã có tài khoản? <router-link to="/login" class="link-primary">Đăng nhập</router-link ></p>
                            <v-text-field 
                                type="text" 
                                label="Họ và tên"  
                                variant="outlined" 
                                placeholder="Họ và tên" 
                                :rules="[required]" 
                                v-model="register.fullName"
                            ></v-text-field>
                            <div class="form-group">
                                <label class="form-label">Ngày sinh: </label>
                                <input type="date" class="form-control" id="example-datemin" :rules="[required]" v-model="register.doB" min="2000-01-02">
                            </div>
                            <v-text-field 
                                type="text" 
                                label="Số điện thoại"  
                                variant="outlined" 
                                placeholder="Số điện thoại" 
                                :rules="[rules.required, rules.validPhone]" 
                                v-model="register.phoneNumber"
                            ></v-text-field>
                            <v-text-field 
                                type="text" 
                                label="Email"  
                                variant="outlined" 
                                placeholder="Email" 
                                :rules="[rules.required, rules.validEmail]"
                                v-model="register.email"
                            ></v-text-field>
                            <v-text-field 
                                type="text" 
                                label="Căn cược công dân"  
                                variant="outlined" 
                                placeholder="Căn cược công dân" 
                                :rules="[required]" 
                                v-model="register.cCCD"
                            ></v-text-field>
                            <v-text-field 
                                type="text" 
                                label="Địa chỉ thường trú"  
                                variant="outlined" 
                                placeholder="Địa chỉ thường trú" 
                                :rules="[required]" 
                                v-model="register.address"
                            ></v-text-field>
                            <v-text-field 
                                type="text" 
                                label="Số điện thoại đăng ký Zalo"  
                                variant="outlined" 
                                placeholder="Số điện thoại đăng ký Zalo" 
                                v-model="register.sDTZalo"
                            ></v-text-field>
                            <div class="d-flex mt-1 justify-content-between">
                                <div class="form-check">
                                    <input class="form-check-input input-primary" type="checkbox" id="customCheckc1" v-model="termsAccepted">
                                    <label class="form-check-label text-muted" for="customCheckc1">Tôi đồng ý với tất cả các Điều khoản & Điều kiện</label>
                                </div>
                            </div>
                        </v-form>
                        <div class="d-grid mt-4">
                            <button type="button" class="btn btn-primary" :disabled="!form || !termsAccepted" @click="Save()">Tạo tài khoản</button>
                        </div>
                        <div class="saprator my-3">
                            <span>Đăng nhập bằng</span>
                        </div>
                        <div class="text-center">
                            <ul class="list-inline mx-auto mt-3 mb-0">
                                <li class="list-inline-item">
                                    <a href="https://www.facebook.com/" class="avtar avtar-s rounded-circle bg-facebook"
                                        target="_blank">
                                        <i class="fab fa-facebook-f text-white"></i>
                                    </a>
                                </li>
                                <li class="list-inline-item">
                                    <a href="https://twitter.com/" class="avtar avtar-s rounded-circle bg-twitter"
                                        target="_blank">
                                        <i class="fab fa-twitter text-white"></i>
                                    </a>
                                </li>
                                <li class="list-inline-item">
                                    <a href="https://myaccount.google.com/"
                                        class="avtar avtar-s rounded-circle bg-googleplus" target="_blank">
                                        <i class="fab fa-google text-white"></i>
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <Rightbar />
</template>