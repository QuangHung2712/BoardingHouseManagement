<style scoped>
    .auth-wrapper{
        background-image: url('../../../../public/images/background.jpg');
        background-size: cover;
        background-position: center;
        background-repeat: no-repeat;
    }
</style>
<script>
import Rightbar from "@/components/right-bar.vue"
import apiClient from "@/plugins/axios"
import Swal from "sweetalert2";
import Axios from "axios";



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
            formAddress: false,
            viewDiaLogAddress: false,
            tinhData: [
                    
            ],
            huyenData: [
                
            ],
            phuongData: [
                
            ],
            selectTinh: null,
            selectHuyen: null,
            selectPhuong: null,
            address: '',
            addressConfirm: '',
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
        },
        btnAddress(index){
            Axios.get(`https://esgoo.net/api-tinhthanh/1/0.htm`)
                .then(response=>{
                    this.tinhData =  response.data.data;
                })
                .catch(error=>{
                    this.message = "Lấy danh sách tỉnh thành bị lỗi " + error;
                    this.snackbar = true;
                    this.snackbarColor = 'red';
                })
                this.indexAddress = index;
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
            this.addressConfirm = this.address + this.onPhuongChange();
        },
        btnSaveAddress(){
            this.register.address = this.addressConfirm;
            this.viewDiaLogAddress = false
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
                                :rules="[rules.required]" 
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
                                :rules="[rules.required]" 
                                v-model="register.cCCD"
                            ></v-text-field>
                            <v-text-field 
                                type="text" 
                                label="Địa chỉ thường trú"  
                                variant="outlined" 
                                placeholder="Địa chỉ thường trú" 
                                :rules="[rules.required]" 
                                readonly
                                @click="(viewDiaLogAddress = !viewDiaLogAddress) && btnAddress()"
                                v-model="register.address"
                            ></v-text-field>
                            <v-text-field 
                                type="text" 
                                label="Số điện thoại đăng ký Zalo"  
                                variant="outlined" 
                                placeholder="Số điện thoại đăng ký Zalo" 
                                :rules="[rules.required,]"
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
        <BModal v-model="viewDiaLogAddress" hide-footer title="Chọn địa chỉ" modal-class="fadeInRight"
            class="v-modal-custom" centered size="lg" >
            <div class="card-body">
                <v-form v-model="formAddress" ref="formChange">
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
                            <BCol class="col-lg-12">
                                <div class="form-group">
                                <label class="form-label">Địa chỉ:</label>
                                <v-text-field
                                    v-model="addressConfirm"
                                    variant="outlined"
                                    readonly
                                    class="input-control"
                                    :rules="[required]"
                                ></v-text-field>
                                </div>
                            </BCol>
                        </BRow>
                    </transition>
                </v-form>
            </div>
            <div class="modal-footer v-modal-footer">
                <BButton type="button" variant="light" @click="viewDiaLogAddress = false">Close
                </BButton>
                <BButton type="button" variant="primary" @click="btnSaveAddress()" :disabled="!formAddress">Save Changes</BButton>
            </div>
        </BModal>
    </div>
    <Rightbar />
</template>