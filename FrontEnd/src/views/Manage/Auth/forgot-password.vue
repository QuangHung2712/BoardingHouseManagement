<script>
import Rightbar from "@/components/right-bar.vue"
//import apiClient from "@/plugins/axios";

export default {
    name: "FORGOT-PASSWORD",
    components: {
        Rightbar
    },
    data(){
        return{
            email:'',
            IsEmail: false,
            IsCode: false,
            password: '',
            passwordConfirm: '',
            inputs: Array(6).fill(''),
            code: '',
            message: '',
            snackbar: false,
            snackbarColor: '',
            rules: {
                validEmail: v => /^[a-zA-Z0-9._%+-]+@gmail\.com$/.test(v) || 'Email phải hợp lệ và kết thúc bằng @gmail.com',
                validPassword: v=> /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/.test(v) || 'Mật khẩu phải có ít nhất 8 ký tự, bao gồm ít nhất 1 chữ cái và 1 chữ số',
                matchPassword: (v) =>v === this.password || 'Mật khẩu xác nhận không khớp',
            },
            showpassword: false,
        }
    },
    methods:{
        btnSendEmail(){
            this.IsEmail = true;
            // apiClient.post(`/Landlord/ForGotPassword/${this.email}`)
            //         .then(response=> {
            //             this.code = response.data;
                        
            //         })
            //         .catch(error=>{
            //             this.message = 'Đã xảy ra lỗi: ' + error.response.data.message;
            //             this.snackbarColor = 'red'
            //             this.snackbar = true
            //         })
        },
        btnContinue(){
            this.IsEmail = false;
            this.IsCode = true;
        },
        moveToNext(index) {
            // Nếu ký tự đã được nhập và chưa phải là ô cuối cùng
            if (this.inputs[index].length === 1 && index < this.inputs.length - 1) {
                // Lấy danh sách các trường input
                const nextInput = this.$el.querySelectorAll('input')[index + 1];
                if (nextInput) {
                nextInput.focus(); // Di chuyển focus sang ô tiếp theo
                }
            }
        },
    }
}
</script>

<template>
    <div class="auth-main v2">
        <v-snackbar
            v-model="snackbar"
            :timeout="10000"
            class="custom-snackbar"
            :color="snackbarColor"
        >
            <h5 class="text-center">{{ message }}</h5>
        </v-snackbar>
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
                        <h4 class="f-w-500 mb-1">Quên mật khẩu</h4>
                        <p class="mb-3">Quay lại <router-link to="/login" class="link-primary">Đăng nhập</router-link></p>
                        <div class="form-group">
                            <label class="form-label">Email</label>
                            <v-text-field 
                                type="text"  
                                variant="outlined" 
                                placeholder="Nhập vào Email" 
                                v-model="email"
                            ></v-text-field>
                        </div>
                        <div   class="d-grid">
                            <button type="button" v-show="!IsEmail && !IsCode" class="btn btn-primary" @click="btnSendEmail">Gửi Email</button>
                        </div>
                        <div v-show="IsEmail">
                            <p class="mb-0">Chúng tôi đã gửi mã đến Email: qua****qn@gmail.com</p>
                            <p class="mb-3">Không nhận được Email?<a href="#" class="link-primary ms-1">Gửi lại mã</a></p>
                            <BRow class="row my-4 text-center">
                                <BCol class="col" v-for="(input, index) in inputs" :key="index">
                                    <input type="text" class="form-control text-center" placeholder="0" maxlength="1" @input="moveToNext(index)" >
                                </BCol>
                            </BRow>
                            <div  class="d-grid">
                                <button type="button" @click="btnContinue" class="btn btn-primary">Tiếp tục</button>
                            </div>
                        </div>
                        <div v-show="IsCode">
                            <div class="form-group m-0">
                                <label class="form-label">Mật khẩu</label>
                                <v-text-field 
                                    type="text"  
                                    variant="outlined" 
                                    placeholder="Nhập vào mật khẩu"
                                    :rules="[rules.validPassword]" 
                                    v-model="password"
                                ></v-text-field>
                            </div>
                            <div class="form-group m-0">
                                <label class="form-label">Nhập lại mật khẩu</label>
                                <v-text-field 
                                    type="text"  
                                    variant="outlined" 
                                    placeholder="Nhập lại mật khẩu" 
                                    v-model="passwordConfirm"
                                    :rules="[rules.matchPassword]" 
                                ></v-text-field>
                            </div>
                            <div  class="d-grid">
                                <button type="button" @click="btnSave" class="btn btn-primary">Đổi mật khẩu</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <Rightbar />
</template>